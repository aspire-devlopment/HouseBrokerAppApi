using HouseBrokerApp.Domain.Common;
using HouseBrokerApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HouseBrokerApp.Infrastructure.Persistence;

public class HouseBrokerAppDbContext : IdentityDbContext<ApplicationUser>
{
    public HouseBrokerAppDbContext(DbContextOptions<HouseBrokerAppDbContext> options) : base(options) { }

    public DbSet<PropertyListing> PropertyListings => Set<PropertyListing>();
    public DbSet<CommissionRate> CommissionRates => Set<CommissionRate>();
    public DbSet<PropertyImage> PropertyImages => Set<PropertyImage>();


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = DateTime.UtcNow;

            if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAt = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.BrokerId)
                  .ValueGeneratedOnAdd();

            entity.HasIndex(u => u.BrokerId).IsUnique();
        });

        modelBuilder.Entity<PropertyListing>(entity =>
        {
            entity.Property(e => e.Title).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CommissionAmount).HasColumnType("decimal(18,2)");

            entity.HasMany(p => p.Images)
               .WithOne(i => i.PropertyListing)
               .HasForeignKey(i => i.PropertyListingId);

            entity.HasOne<ApplicationUser>()
             .WithMany(u => u.Listings)
             .HasForeignKey(p => p.BrokerId)
             .HasPrincipalKey(u => u.BrokerId)
             .OnDelete(DeleteBehavior.Cascade);
        });


        modelBuilder.Entity<CommissionRate>(entity =>
        {
            entity.Property(e => e.Rate).HasColumnType("decimal(5,4)"); // e.g. 0.0175 = 1.75%
        });

        modelBuilder.Entity<PropertyImage>(entity =>
        {
            entity.HasOne(e => e.PropertyListing)
                        .WithMany(p => p.Images)
                        .HasForeignKey(e => e.PropertyListingId)
                        .OnDelete(DeleteBehavior.Cascade);

        });


    }

}