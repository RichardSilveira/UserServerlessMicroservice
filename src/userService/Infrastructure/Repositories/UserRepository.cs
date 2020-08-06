using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Domain;
using UserService.Functions;

namespace UserService.Infrastructure.Repositories
{
    public class UserRepository : EFCRUDRepositoryBase<User, Guid>, IUserRepository
    {
        public UserRepository(UserServiceDbContext context) : base(context)
        {
        }
    }
}