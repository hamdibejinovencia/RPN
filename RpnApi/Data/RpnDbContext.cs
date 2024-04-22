namespace Rpn.Api.Data;

using Rpn.Api.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Rpn.Api.Domain.Entities;

public partial class RpnDbContext(DbContextOptions<RpnDbContext> options) : DbContext(options)
{
    public DbSet<Stack> T_Stacks { get; set; } = default!;
    //public DbSet<Item> T_Items { get; set; } = default!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=RPN;User Id=sa;Password=HamdiBEJI#200584;trustServerCertificate=true;Initial Catalog=RPN;Encrypt=False");
        base.OnConfiguring(optionsBuilder);
        
    }
    public IList<Stack> Stacks { get; init; } = [];
    public IList<char> Operands { get; init; } = ['+', '-', ':', '*'];

    public async Task<IEnumerable<char>> GetOperands()
    {
        return await Task.Run(() => this.Operands);
    }

    public async Task<IEnumerable<Stack>> GetStacks()
    {
        return await Task.Run(() => this.Stacks);
    }

    public async Task<Stack> CreateStack()
    {
        var stack = await Task.Run(() => new Stack() { StackId = Guid.NewGuid(), Elements = new Stack<string>() });
        this.Stacks.Add(stack);
        return stack;
    }

    public async Task<Stack> GetStack(Guid stackId)
    {
        var stack = await Task.Run(() => this.Stacks.FirstOrDefault(x => x.StackId == stackId));
        if (stack is null)
        {
            throw new UserInputException($"There is no stack with the id : {stackId}.");
        }

        return stack;
    }

    public async Task<Stack> AddValueToStack(Stack stack, string value)
    {
        await Task.Run(() => stack.Elements.Push(value));
        return stack;
    }

    public async Task DeleteStack(Stack stack)
    {
        await Task.Run(() => this.Stacks.Remove(stack));
    }

    public async Task<Stack> ApplyOperand(char op, Stack stack)
    {
        return await Task.Run(() =>
        {
            var op1 = stack.Elements.Pop();
            var op2 = stack.Elements.Pop();
            switch (op)
            {
                case '+':
                    stack.Elements.Push((Convert.ToInt32(op1) + Convert.ToInt32(op2)).ToString());
                    break;
                case '-':
                    stack.Elements.Push((Convert.ToInt32(op1) - Convert.ToInt32(op2)).ToString());
                    break;
                case '*':
                    stack.Elements.Push((Convert.ToInt32(op1) * Convert.ToInt32(op2)).ToString());
                    break;
                case ':':
                    stack.Elements.Push((Convert.ToInt32(op1) / Convert.ToInt32(op2)).ToString());
                    break;
                default:
                    return stack;
                    break;
            }
            return stack;
        });
    }
}