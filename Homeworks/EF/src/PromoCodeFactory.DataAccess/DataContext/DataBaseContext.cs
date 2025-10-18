using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Configuration;

namespace PromoCodeFactory.DataAccess.DataContext;

public class DataBaseContext : DbContext
{
       

    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {
    }
       
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Preference> Preferences { get; set; }
        
    public DbSet<PromoCode> PromoCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new PreferenceConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerPreferenceConfiguration());
        modelBuilder.ApplyConfiguration(new PromoCodeConfiguration());
    }

}