using System.Linq;
using Rpn.Api.Domain.Entities;
using Rpn.Api.Data;

namespace Rpn.Api.Data.Queries
{
    public static class RpnQueries
    {
        public static IEnumerable<Stack> GetStacks(this RpnDbContext dbContext) =>
        dbContext.Stacks;
    }
}
