using Rpn.Api.Data.Exceptions;

namespace Rpn.Api.Data;
using Microsoft.EntityFrameworkCore;
using Rpn.Api.Domain.Entities;
public partial class RpnContext : DbContext
{
    public IList<Stack> Stacks { get; set; } = [];
    public IList<char> Operands { get; set; } = ['+','-','/','*'];

    public RpnContext(DbContextOptions<RpnContext> options)
        : base(options)
    {
        //Stacks = new List<Stack>();
    }
    
    public async Task<IEnumerable<char>> GetOperands()
    {
        return this.Operands;
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

    public async Task<Stack> GetStack(Guid stackId)
    {
        var stack = this.Stacks.FirstOrDefault(x => x.StackId == stackId);
        if (stack is null)
        {
            throw new UserInputException($"There is no stack with the id : {stackId}.");
        }
        return stack;
    }
    
    public async Task<Stack> AddValueToStack(Stack stack, string value)
    {
        stack.Elements.Push(value);
        return stack;
    }

    public async Task DeleteStack(Stack stack)
    {
        this.Stacks.Remove(stack);
    }
    
    public async Task<Stack> ApplyOperand(char op, Stack stack)
    {
        var op1 = stack.Elements.Pop();
        var op2 = stack.Elements.Pop();
        switch (op)
        {
            case '+':stack.Elements.Push((Convert.ToInt32(op1)+Convert.ToInt32(op2)).ToString());
                break;
            case '-':stack.Elements.Push((Convert.ToInt32(op1)-Convert.ToInt32(op2)).ToString());
                break;
            case '*':stack.Elements.Push((Convert.ToInt32(op1)*Convert.ToInt32(op2)).ToString());
                break;
            case '/':stack.Elements.Push((Convert.ToInt32(op1)/Convert.ToInt32(op2)).ToString());
                break;
            default: return stack;
                break;
        }
        return stack;
    }
}