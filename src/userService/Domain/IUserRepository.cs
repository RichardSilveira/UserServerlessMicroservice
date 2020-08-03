using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.SharedKernel;

namespace UserService.Domain
{
    public interface IUserRepository
    {
        void Add(User entity);

        void Update(User entity);

        Task Delete(Guid Id);

        Task<User> GetById(Guid Id);

        Task<IEnumerable<User>> GetAll();
    }
}