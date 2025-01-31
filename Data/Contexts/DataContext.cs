using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<ContactPersonEntity> ContactPersons { get; set; } = null!;
    public DbSet<StatusEntity> Statuses { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<ProductEntity> Products { get; set; } = null!;
    public DbSet<CustomerEntity> Customers { get; set; } = null!;
    public DbSet<ProjectEntity> Projects { get; set; } = null!;
    public DbSet<ProjectProductEntity> ProjectProducts { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProjectProductEntity>()
            .HasKey(ps => new { ps.ProjectId, ps.ProductId });

        modelBuilder.Entity<ProjectProductEntity>()
            .HasOne(ps => ps.Project)
            .WithMany(p => p.ProjectProducts)
            .HasForeignKey(ps => ps.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Restricting delete behavior if Service is connected
        modelBuilder.Entity<ProjectProductEntity>()
            .HasOne(ps => ps.Product)
            .WithMany(s => s.ProjectProducts)
            .HasForeignKey(ps => ps.ProductId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<ProductEntity>().HasData(
            new ProductEntity { ProductId = 1, ProductName = "Consulting", Price = 1000, Currency = "SEK" },
            new ProductEntity { ProductId = 2, ProductName = "Development", Price = 1500, Currency = "SEK" },
            new ProductEntity { ProductId = 3, ProductName = "Testing", Price = 1000, Currency = "SEK" },
            new ProductEntity { ProductId = 4, ProductName = "Design", Price = 1200, Currency = "SEK" },
            new ProductEntity { ProductId = 5, ProductName = "Maintenance", Price = 800, Currency = "SEK" },
            new ProductEntity { ProductId = 6, ProductName = "Training", Price = 1600, Currency = "SEK" },
            new ProductEntity { ProductId = 7, ProductName = "Support", Price = 700, Currency = "SEK" }
            );

        modelBuilder.Entity<StatusEntity>().HasData(
            new StatusEntity { StatusId = 1, StatusName = "Not started" },
            new StatusEntity { StatusId = 2, StatusName = "Ongoing" },
            new StatusEntity { StatusId = 3, StatusName = "Completed" }
        );

        base.OnModelCreating(modelBuilder);
    }
}
