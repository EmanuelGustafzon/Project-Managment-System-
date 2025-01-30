using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<CustomerEntity> Customers { get; set; } = null!;
    public DbSet<ServiceEntity> Services { get; set; } = null!;

    public DbSet<CurrencyEntity> Currencies { get; set; } = null!;
    public DbSet<StatusEntity> Statuses { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<ProjectEntity> Projects { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ServiceEntity>()
            .Property(e => e.Unit)
            .HasConversion<string>();

        modelBuilder.Entity<StatusEntity>()
           .Property(e => e.Status)
           .HasConversion<string>();
    }
}