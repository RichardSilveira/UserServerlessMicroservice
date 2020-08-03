using Microsoft.EntityFrameworkCore;
using UserService.Domain;
using UserService.Infrastructure.Repositories.Mappings;

namespace UserService.Infrastructure.Repositories
{
    public class UserServiceDbContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public UserServiceDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap());
        }
    }
}