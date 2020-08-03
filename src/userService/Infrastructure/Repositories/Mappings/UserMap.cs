using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain;

namespace UserService.Infrastructure.Repositories.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.FirstName).HasMaxLength(200).IsRequired();
            builder.Property(p => p.LastName).HasMaxLength(200).IsRequired();
        }
    }
}
/*
CREATE TABLE User (
    Id varchar(255) NOT NULL,
    LastName varchar(255) NOT NULL,
    FirstName varchar(255),
    PRIMARY KEY (Id)
);
*/