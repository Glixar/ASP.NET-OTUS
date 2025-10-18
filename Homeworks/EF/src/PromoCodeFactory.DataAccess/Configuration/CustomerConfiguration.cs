using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;

namespace PromoCodeFactory.DataAccess.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.FirstName)
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(c => c.LastName)
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasMaxLength(128)
                .IsRequired();

            builder.HasIndex(c => c.Email)
                .IsUnique();

            builder.Property(c => c.Nickname)
                .HasMaxLength(32);

            builder.HasIndex(c => c.Nickname)
                .IsUnique();

            builder.HasMany(c => c.Preferences)
                .WithOne(cp => cp.Customer)
                .HasForeignKey(cp => cp.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.PromoCodes)
                .WithOne(pc => pc.Customer)
                .HasForeignKey(pc => pc.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(FakeDataFactory.Customers);
        }
    }
}