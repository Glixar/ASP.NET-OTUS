using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.DataAccess.Data;

namespace PromoCodeFactory.DataAccess.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasOne(e => e.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.RoleId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Navigation(e => e.Role).AutoInclude();

            builder.Property(e => e.FirstName)
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(e => e.LastName)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(e => e.Email)
                .HasMaxLength(256)
                .IsRequired();

            builder.HasIndex(e => e.Email).IsUnique();

            builder.HasData(FakeDataFactory.EmployeeSeeds);
        }
    }
}