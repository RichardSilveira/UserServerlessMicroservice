using System;

namespace UserService.SharedKernel
{
    public abstract class Entity
    {
        public Guid Id { get; private set; }

        protected Entity()
        {
            Id = Guid.NewGuid(); //TODO: Use a Flake ID implementation later
        }
    }
}