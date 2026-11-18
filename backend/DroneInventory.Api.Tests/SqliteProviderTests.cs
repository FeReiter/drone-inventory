using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DroneInventory.Api.Tests;

public class SqliteProviderTests
{
    [Fact]
    public async Task OpensAndClosesTemporaryDatabaseFile()
    {
        var directory = Directory.CreateTempSubdirectory("drone-inventory-tests-");

        try
        {
            var databasePath = Path.Combine(directory.FullName, "connection.db");
            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = databasePath,
                Pooling = false
            }.ToString();
            var options = new DbContextOptionsBuilder()
                .UseSqlite(connectionString)
                .Options;

            await using var context = new DbContext(options);
            var connection = context.Database.GetDbConnection();

            await context.Database.OpenConnectionAsync();
            Assert.Equal(ConnectionState.Open, connection.State);
            Assert.True(File.Exists(databasePath));

            await context.Database.CloseConnectionAsync();
            Assert.Equal(ConnectionState.Closed, connection.State);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }
}
