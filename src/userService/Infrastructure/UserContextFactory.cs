using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using UserService.Infrastructure.Repositories;

namespace UserService.Infrastructure
{
    public class UserContextFactory : IDesignTimeDbContextFactory<UserContext>
    {
        public UserContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
            optionsBuilder.UseMySql(ConfigurationService.BuildConfiguration("local")["UserServiceDbContextConnectionString"]);

            return new UserContext(optionsBuilder.Options);
        }
    }
}