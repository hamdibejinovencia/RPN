namespace Rpn.Api.Data;
using Microsoft.EntityFrameworkCore;
using Rpn.Api.Domain.Entities;
public partial class RpnContext : DbContext
{
    public IList<Stack> Stacks { get; set; } = [];

    public RpnContext(DbContextOptions<RpnContext> options)
        : base(options)
    {
        //Stacks = new List<Stack>();
    }
    
    public async Task<IEnumerable<Stack>> GetStacks()
    {
        return this.Stacks;
    }

    public async Task<Stack> CreateStack()
    {
        var stack = new Stack() { StackId = Guid.NewGuid(), Elements = new Stack<string>() };
        this.Stacks.Add(stack);
        return stack;
    }
}