using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IOptions<JwtConfiguration> _jwtOptions;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly IEmailService _emailService;
        private readonly IOptions<ClientConfiguration> _clientOptions;
        private readonly ClientConfiguration _clientConfiguration;
        private readonly IOptions<SuperAdminConfiguration> _adminOptions;
        private readonly SuperAdminConfiguration _adminConfiguration;

        private User? _user;

        public AuthenticationService(
            ILoggerManager logger,
            IMapper mapper,
            UserManager<User> userManager,
            IOptions<JwtConfiguration> jwtOptions,
            IEmailService emailService,
            IOptions<ClientConfiguration> clientOptions,
            IOptions<SuperAdminConfiguration> adminOptions)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _jwtOptions = jwtOptions;
            _jwtConfiguration = _jwtOptions.Value;
            _emailService = emailService;
            _clientOptions = clientOptions;
            _clientConfiguration = _clientOptions.Value;
            _adminOptions = adminOptions;
            _adminConfiguration = _adminOptions.Value;
        }

        public async Task<IdentityResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);
            user.UserName = user.Email;

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);

            if(result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var link = $"{_clientConfiguration.Url}/verifyemail?code={code}&userid={user.Id}";

                await _emailService.SendAsync(user.Email, "Verify Your Email To Login", $"<h1>Email Verification Pending</h1><p>Click <a href=\"{link}\">here</a> to verify your email and continue logging in.</p>", true);
            }

            return result;
        }

        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth)
        {
            _user = await _userManager.FindByEmailAsync(userForAuth.Email);

            if (_user != null && !(await _userManager.IsEmailConfirmedAsync(_user)))
            {
                throw new EmailNotConfirmedUnauthorizedException(_user.Email);
            }

            var result = (await _userManager.CheckPasswordAsync(_user, userForAuth.Password));
            if (!result)
            {
                _logger.LogWarn($"{nameof(ValidateUser)}: Authentication failed. Wrong email name or password.");
            }

            if (_user.IsBlocked)
            {
                throw new BlockedUserForbiddenRequest();
            }

            return result;
        }

        public async Task<TokenDto> CreateToken(bool populateExp)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            var refreshToken = GenerateRefreshToken();

            _user.RefreshToken = refreshToken;

            if (populateExp)
            {
                _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            }

            await _userManager.UpdateAsync(_user);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenDto(accessToken, refreshToken);
        }

        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var user = await _userManager.FindByEmailAsync(principal.FindFirst(ClaimTypes.Email).Value);

            if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new RefreshTokenBadRequestException();
            }

            _user = user;

            return await CreateToken(false);
        }

        public async Task RevokeRefresh(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var user = await _userManager.FindByEmailAsync(principal.FindFirst(ClaimTypes.Email).Value);

            if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new RefreshTokenBadRequestException();
            }

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> VerifyEmailAsync(VerifyEmailDto emailDto)
        {
            var user = await _userManager.FindByIdAsync(emailDto.UserId);

            if (user == null)
            {
                _logger.LogWarn($"User with id : {emailDto.UserId} does not exist.");
                throw new UserNotFoundException(emailDto.UserId);
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(emailDto.Code));

            var result = await _userManager.ConfirmEmailAsync(user, code);

            return result;
        }

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogWarn($"User with email : {email} does not exist.");
                throw new UserNotFoundException(email);
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var link = $"{_clientConfiguration.Url}/resetpassword?code={code}&userid={user.Id}";

            await _emailService.SendAsync(user.Email, "Reset Password", $"<h1>Reset Password</h1><p>Click <a href=\"{link}\">here</a> to reset your password.</p>", true);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto resetDto)
        {
            var user = await _userManager.FindByIdAsync(resetDto.UserId);

            if (user == null)
            {
                _logger.LogWarn($"User with id : {resetDto.UserId} does not exist.");
                throw new UserNotFoundException(resetDto.UserId);
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetDto.Code));

            var result = await _userManager.ResetPasswordAsync(user, code, resetDto.NewPassword);

            if (result.Succeeded)
            {
                await _emailService.SendAsync(user.Email, "Password Reset Complete", $"<p>Your password has been reset successfully.</p>", true);
            }

            return result;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var role = _user!.Email == _adminConfiguration.Email ? Constants.GLOBAL_ROLE_SUPERADMIN : Constants.GLOBAL_ROLE_USER;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, _user.Email),
                new Claim(ClaimTypes.Name, $"{_user.FirstName} {_user.LastName}"),
                new Claim(ClaimTypes.NameIdentifier, _user.Id),
                new Claim(ClaimTypes.Role, role)
            };

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken
            (
                issuer: _jwtConfiguration.ValidIssuer,
                audience: _jwtConfiguration.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
                signingCredentials: signingCredentials
            );
            return tokenOptions;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"))),
                ValidateLifetime = false,
                ValidIssuer = _jwtConfiguration.ValidIssuer,
                ValidAudience = _jwtConfiguration.ValidAudience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
