using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class ProjectMember
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(User))]
        public string? MemberId { get; set; }
        public User? User { get; set; }
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        [ForeignKey(nameof(Role))]
        public Guid? RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
