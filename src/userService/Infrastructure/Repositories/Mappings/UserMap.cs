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

            builder.OwnsOne(m => m.Address, address =>
            {
                address.Property(p => p.Country).HasColumnName("Country").IsRequired();
                address.Property(p => p.State).HasColumnName("State").HasMaxLength(200);
                address.Property(p => p.City).HasColumnName("City").HasMaxLength(200);
                address.Property(p => p.Street).HasColumnName("Street").HasMaxLength(200);
            });
        }
    }
}
/*
CREATE TABLE User (
    Id varchar(255) NOT NULL,
    LastName varchar(200) NOT NULL,
    FirstName varchar(200),
    Country varchar(200),
    State varchar(200),
    City varchar(200),
    Street varchar(200),    
    PRIMARY KEY (Id)
);
*/