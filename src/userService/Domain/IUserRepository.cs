﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.SharedKernel;

namespace UserService.Domain
{
    public interface IUserRepository : IRepository<User>
    {
        void Add(User entity);

        void Update(User entity);

        void Delete(User entity);

        Task<User> GetByIdAsync(Guid Id);
    }
}