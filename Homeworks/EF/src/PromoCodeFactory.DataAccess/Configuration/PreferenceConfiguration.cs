using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;

namespace PromoCodeFactory.DataAccess.Configuration;

public class PreferenceConfiguration : IEntityTypeConfiguration<Preference>
{
    public void Configure(EntityTypeBuilder<Preference> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(32)
            .IsRequired();

        builder.HasIndex(p => p.Name)
            .IsUnique();

        builder.HasData(FakeDataFactory.Preferences);
    }
}