using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.Stage)
                .WithMany(s => s.Tasks)
                .HasForeignKey(t => t.StageId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<Project>? Projects { get; set; }
        public DbSet<Role>? ProjectRoles { get; set; }
        public DbSet<Stage>? Stages { get; set; }
        public DbSet<TaskType>? TaskTypes { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<ProjectTask>? Tasks { get; set; }
        public DbSet<TaskComment>? Comments { get; set; }
        public DbSet<ProjectMember>? ProjectMembers { get; set; }
        public DbSet<UserInvite>? UserInvites { get; set; }
    }
}
