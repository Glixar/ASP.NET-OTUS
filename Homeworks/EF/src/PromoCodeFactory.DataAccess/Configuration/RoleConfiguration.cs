using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.DataAccess.Data;

namespace PromoCodeFactory.DataAccess.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(r => r.Name)
            .HasMaxLength(32);
        builder.Property(r => r.Description)
            .HasMaxLength(128);

        builder.HasIndex(r => r.Name)
            .IsUnique();

        builder.HasData(FakeDataFactory.Roles);
    }
}