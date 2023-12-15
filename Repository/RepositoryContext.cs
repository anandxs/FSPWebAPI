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
        }

        public DbSet<Project>? Projects { get; set; }
        public DbSet<Role>? ProjectRoles { get; set; }
        public DbSet<Stage>? Stages { get; set; }
        public DbSet<TaskType>? TaskTypes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProjectMember>? ProjectMembers { get; set; }
    }
}
