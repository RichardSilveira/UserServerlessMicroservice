using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain;

namespace UserService.Infrastructure.Repositories.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.FirstName).HasMaxLength(200).IsRequired();
            builder.Property(p => p.LastName).HasMaxLength(200).IsRequired();
            builder.Property(p => p.Email).HasMaxLength(250).IsRequired();

            builder.OwnsOne(m => m.Address, address =>
            {
                address.Property(p => p.Country).HasColumnName("Country")
                    .IsRequired(); // MySql EF current provider is ignoring "IsRequired" for ValueObject =\
                
                address.Property(p => p.State).HasColumnName("State").HasMaxLength(200);
                address.Property(p => p.City).HasColumnName("City").HasMaxLength(200);
                address.Property(p => p.Street).HasColumnName("Street").HasMaxLength(200);
            });
        }
    }
}