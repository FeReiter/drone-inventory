using Microsoft.EntityFrameworkCore;

namespace DroneInventory.Api.Persistence;

public sealed class DroneInventoryDbContext(DbContextOptions<DroneInventoryDbContext> options)
    : DbContext(options)
{
    public DbSet<Drone> Drones => Set<Drone>();
    public DbSet<StudentGroup> StudentGroups => Set<StudentGroup>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<LoanItem> LoanItems => Set<LoanItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Loan>(loan =>
        {
            loan.ToTable(table => table.HasCheckConstraint(
                "CK_Loans_Status", "\"Status\" IN ('Active', 'Returned')"));
            loan.Property(entity => entity.Status).HasConversion<string>();
            loan.HasOne<Drone>().WithMany().HasForeignKey(entity => entity.DroneId)
                .OnDelete(DeleteBehavior.Restrict);
            loan.HasOne<StudentGroup>().WithMany().HasForeignKey(entity => entity.StudentGroupId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<LoanItem>(item =>
        {
            item.ToTable(table =>
            {
                table.HasCheckConstraint("CK_LoanItems_Type", "\"Type\" IN ('Battery', 'Propeller')");
                table.HasCheckConstraint("CK_LoanItems_Quantity",
                    "typeof(\"Quantity\") = 'integer' AND \"Quantity\" BETWEEN 0 AND 2147483647");
            });
            item.Property(entity => entity.Type).HasConversion<string>();
            item.HasOne<Loan>().WithMany().HasForeignKey(entity => entity.LoanId)
                .OnDelete(DeleteBehavior.Restrict);
            item.HasIndex(entity => new { entity.LoanId, entity.Type }).IsUnique();
        });
    }
}
