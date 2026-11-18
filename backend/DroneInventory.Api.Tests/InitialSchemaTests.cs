using DroneInventory.Api.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DroneInventory.Api.Tests;

public sealed class InitialSchemaTests : IDisposable
{
    private readonly DirectoryInfo directory = Directory.CreateTempSubdirectory("drone-inventory-schema-");

    private DroneInventoryDbContext OpenContext()
    {
        var connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = Path.Combine(directory.FullName, "schema.db"),
            ForeignKeys = true,
            Pooling = false
        }.ToString();

        return new DroneInventoryDbContext(
            new DbContextOptionsBuilder<DroneInventoryDbContext>().UseSqlite(connectionString).Options);
    }

    [Fact]
    public async Task MigrationPreservesLoansAndItemsAfterReopeningDatabase()
    {
        var loanDate = new DateOnly(2026, 8, 19);
        var returnDate = new DateOnly(2026, 8, 20);

        await using (var context = OpenContext())
        {
            Assert.False(context.Database.HasPendingModelChanges());
            await context.Database.MigrateAsync();

            var drone = new Drone { Name = "TELLO-07", Description = "SSID: TELLO-9FA21B" };
            var group = new StudentGroup { Name = "Group 3" };
            context.AddRange(drone, group);
            await context.SaveChangesAsync();

            var returned = new Loan
            {
                DroneId = drone.Id, StudentGroupId = group.Id, LoanDate = loanDate,
                Status = LoanStatus.Returned, ReturnDate = returnDate
            };
            var active = new Loan
            {
                DroneId = drone.Id, StudentGroupId = group.Id, LoanDate = returnDate,
                Status = LoanStatus.Active
            };
            context.AddRange(returned, active);
            await context.SaveChangesAsync();

            context.LoanItems.AddRange(
                new LoanItem { LoanId = returned.Id, Type = LoanItemType.Battery, Quantity = 4 },
                new LoanItem { LoanId = returned.Id, Type = LoanItemType.Propeller, Quantity = 8 });
            await context.SaveChangesAsync();
        }

        await using var reopened = OpenContext();
        var storedDrone = await reopened.Drones.SingleAsync();
        var storedGroup = await reopened.StudentGroups.SingleAsync();
        Assert.Equal("TELLO-07", storedDrone.Name);
        Assert.Equal("SSID: TELLO-9FA21B", storedDrone.Description);
        Assert.Equal("Group 3", storedGroup.Name);

        var loans = await reopened.Loans.ToListAsync();
        Assert.Equal(2, loans.Count);
        Assert.All(loans, loan =>
        {
            Assert.Equal(storedDrone.Id, loan.DroneId);
            Assert.Equal(storedGroup.Id, loan.StudentGroupId);
        });
        var storedReturned = Assert.Single(loans, loan => loan.Status == LoanStatus.Returned);
        Assert.Equal(loanDate, storedReturned.LoanDate);
        Assert.Equal(returnDate, storedReturned.ReturnDate);
        var storedActive = Assert.Single(loans, loan => loan.Status == LoanStatus.Active);
        Assert.Equal(returnDate, storedActive.LoanDate);
        Assert.Null(storedActive.ReturnDate);

        var items = await reopened.LoanItems.OrderBy(item => item.Type).ToListAsync();
        Assert.All(items, item => Assert.Equal(storedReturned.Id, item.LoanId));
        Assert.Collection(items,
            item => Assert.Equal((LoanItemType.Battery, 4), (item.Type, item.Quantity)),
            item => Assert.Equal((LoanItemType.Propeller, 8), (item.Type, item.Quantity)));
    }

    [Fact]
    public async Task IndexAllowsOnlyOneActiveLoanPerDroneAfterUpgrade()
    {
        await using var context = OpenContext();
        var migrator = context.GetService<IMigrator>();
        await migrator.MigrateAsync("20261118083000_InitialSchema");

        var firstDrone = new Drone { Name = "TELLO-07" };
        var secondDrone = new Drone { Name = "TELLO-02" };
        var firstGroup = new StudentGroup { Name = "Group 3" };
        var secondGroup = new StudentGroup { Name = "Group 1" };
        context.AddRange(firstDrone, secondDrone, firstGroup, secondGroup);
        await context.SaveChangesAsync();

        var loanDate = new DateOnly(2026, 8, 19);
        var firstActive = new Loan
        {
            DroneId = firstDrone.Id, StudentGroupId = firstGroup.Id,
            LoanDate = loanDate, Status = LoanStatus.Active
        };
        context.Loans.AddRange(firstActive, new Loan
        {
            DroneId = firstDrone.Id, StudentGroupId = firstGroup.Id,
            LoanDate = loanDate.AddDays(-2), ReturnDate = loanDate.AddDays(-1),
            Status = LoanStatus.Returned
        });
        await context.SaveChangesAsync();

        await context.Database.MigrateAsync();
        context.Loans.AddRange(new Loan
        {
            DroneId = firstDrone.Id, StudentGroupId = secondGroup.Id,
            LoanDate = loanDate.AddDays(-2), ReturnDate = loanDate.AddDays(-1),
            Status = LoanStatus.Returned
        }, new Loan
        {
            DroneId = secondDrone.Id, StudentGroupId = firstGroup.Id,
            LoanDate = loanDate, Status = LoanStatus.Active
        });
        await context.SaveChangesAsync();

        context.Loans.Add(new Loan
        {
            DroneId = firstDrone.Id, StudentGroupId = secondGroup.Id,
            LoanDate = loanDate, Status = LoanStatus.Active
        });
        var error = await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
        var sqliteError = Assert.IsType<SqliteException>(error.InnerException);
        Assert.Equal(2067, sqliteError.SqliteExtendedErrorCode); // SQLITE_CONSTRAINT_UNIQUE

        await using var reopened = OpenContext();
        var firstDroneLoans = await reopened.Loans.Where(loan => loan.DroneId == firstDrone.Id).ToListAsync();
        Assert.Equal(2, firstDroneLoans.Count(loan => loan.Status == LoanStatus.Returned));
        Assert.Equal(firstActive.Id, Assert.Single(firstDroneLoans, loan => loan.Status == LoanStatus.Active).Id);
        Assert.Equal(LoanStatus.Active, (await reopened.Loans.SingleAsync(loan => loan.DroneId == secondDrone.Id)).Status);
    }

    // Raw SQL exercises SQLite's constraints independently of EF's C# property types.
    [Theory]
    [InlineData("INSERT INTO Loans (DroneId, StudentGroupId, LoanDate, Status) VALUES (99, 1, '2026-08-19', 'Active')", 787)]
    [InlineData("UPDATE Loans SET StudentGroupId = 99 WHERE Id = 1", 787)]
    [InlineData("INSERT INTO LoanItems (LoanId, Type, Quantity) VALUES (99, 'Battery', 1)", 787)]
    [InlineData("DELETE FROM Drones WHERE Id = 1", 1811)]
    [InlineData("DELETE FROM StudentGroups WHERE Id = 1", 1811)]
    [InlineData("DELETE FROM Loans WHERE Id = 1", 1811)]
    [InlineData("UPDATE Loans SET Status = 'Unknown' WHERE Id = 1", 275)]
    [InlineData("UPDATE LoanItems SET Type = 'Cable' WHERE Id = 1", 275)]
    [InlineData("UPDATE LoanItems SET Quantity = -1 WHERE Id = 1", 275)]
    [InlineData("UPDATE LoanItems SET Quantity = 1.5 WHERE Id = 1", 275)]
    [InlineData("UPDATE LoanItems SET Quantity = 'invalid' WHERE Id = 1", 275)]
    [InlineData("UPDATE LoanItems SET Quantity = 2147483648 WHERE Id = 1", 275)]
    [InlineData("INSERT INTO LoanItems (LoanId, Type, Quantity) VALUES (1, 'Battery', 2)", 2067)]
    public async Task MigrationRejectsInvalidDatabaseWrites(string sql, int extendedErrorCode)
    {
        await using var context = OpenContext();
        await context.Database.MigrateAsync();
        await context.Database.ExecuteSqlRawAsync("INSERT INTO Drones (Id, Name) VALUES (1, 'TELLO-07')");
        await context.Database.ExecuteSqlRawAsync("INSERT INTO StudentGroups (Id, Name) VALUES (1, 'Group 3')");
        await context.Database.ExecuteSqlRawAsync(
            "INSERT INTO Loans (Id, DroneId, StudentGroupId, LoanDate, Status) VALUES (1, 1, 1, '2026-08-19', 'Active')");
        await context.Database.ExecuteSqlRawAsync(
            "INSERT INTO LoanItems (Id, LoanId, Type, Quantity) VALUES (1, 1, 'Battery', 1)");

        var exception = await Assert.ThrowsAsync<SqliteException>(
            () => context.Database.ExecuteSqlRawAsync(sql));

        Assert.Equal(extendedErrorCode, exception.SqliteExtendedErrorCode);
    }

    public void Dispose() => directory.Delete(recursive: true);
}
