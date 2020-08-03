using Microsoft.EntityFrameworkCore;
using UserService.Domain;
using UserService.Infrastructure.Repositories.Mappings;

namespace UserService.Infrastructure.Repositories
{
    public class UserServiceDbContext : DbContext
    {
        public DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(
                    "Server=localhost;Database=testedb;Uid=root;Pwd=MySql2019!;"); //TODO: Change connection string to local docker db
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap());

            //base.OnModelCreating(modelBuilder);
        }
    }
}