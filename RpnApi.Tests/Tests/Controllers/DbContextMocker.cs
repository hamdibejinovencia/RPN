using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Rpn.Api.Data;

namespace RpnApi.Tests.Tests.Controllers;

public static class DbContextMocker
{
    public static RpnContext GetRpnDbContext(string dbName)
    {
        // Create options for DbContext instance
        var options = new DbContextOptionsBuilder<RpnContext>()
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).EnableSensitiveDataLogging().Options;

        // Create instance of DbContext
        var dbContext = new RpnContext(options);
        return dbContext;
    }
}