using System;
using Microsoft.EntityFrameworkCore;
using UserService.Domain;
using UserService.Infrastructure.Repositories.Mappings;

namespace UserService.Infrastructure.Repositories
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }

        public UserContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap());
        }

        public static class Factory
        {
            public static UserContext CreateNew(Action<DbContextOptionsBuilder<UserContext>> options)
            {
                var optionsBuilder = new DbContextOptionsBuilder<UserContext>();

                options?.Invoke(optionsBuilder);
                var context = new UserContext(optionsBuilder.Options);

                return context;
            }
        }
    }
}