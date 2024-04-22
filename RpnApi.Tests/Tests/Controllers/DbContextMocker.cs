namespace RpnApi.Tests.Tests.Controllers;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Rpn.Api.Data;

public static class DbContextMocker
{
    public static RpnDbContext GetRpnDbContext(string dbName)
    {
        // Create options for DbContext instance
        var options = new DbContextOptionsBuilder<RpnDbContext>()
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).EnableSensitiveDataLogging().Options;

        // Create instance of DbContext
        var dbContext = new RpnDbContext(options);
        return dbContext;
    }
}