namespace Rpn.Api.Domain.Services;

using Rpn.Api.Data.Exceptions;
using Rpn.Api.Data;
using Rpn.Api.Domain.Entities;

public interface IRpnService
{
    Task<IEnumerable<char>> GetOperands();
    Task<IEnumerable<Stack>> GetStacks();
    Task<Stack> CreateStack();
    Task<Stack> AddValueToStack(Guid stackId, string value);
    Task DeleteStack(Guid stackId);
    Task<Stack> ApplyOperand(char op, Guid stackId);
    Task<Stack> GetStack(Guid stackId);
}

public class RpnService : IRpnService
{
    private readonly ILogger logger;
    private readonly RpnDbContext _dbContext;

    public RpnService(ILogger<RpnService> logger, RpnDbContext dbContext)
    {
        this.logger = logger;
        this._dbContext = dbContext;
    }

    public async Task<IEnumerable<char>> GetOperands()
    {
        return await _dbContext.GetOperands();
    }

    public async Task<IEnumerable<Stack>> GetStacks()
    {
        return await _dbContext.GetStacks();
    }

    public async Task<Stack> CreateStack()
    {
        return await _dbContext.CreateStack();
    }

    public async Task<Stack> AddValueToStack(Guid stackId, string value)
    {
        // The value to push must be numeric
        var successfullyParsed = int.TryParse(value, out var result);
        if (!successfullyParsed)
        {
            throw new UserInputException($"The value you push must be numeric.");
        }

        var stack = await _dbContext.GetStack(stackId);
        if (stack is null)
        {
            throw new UserInputException($"There is no stack with the id : {stackId}.");
        }

        return await _dbContext.AddValueToStack(stack, value);
    }

    public async Task DeleteStack(Guid stackId)
    {
        var stack = await _dbContext.GetStack(stackId);
        if (stack is null)
        {
            throw new UserInputException($"There is no stack with the id : {stackId}.");
        }

        await _dbContext.DeleteStack(stack);
    }

    public async Task<Stack> ApplyOperand(char op, Guid stackId)
    {
        // The value to push must be an operator (+,-,/,*)
        var operands = await _dbContext.GetOperands();
        if (!operands.Any(x => x == op))
        {
            throw new UserInputException($"The operator you have entered is not valid.");
        }

        var stack = await _dbContext.GetStack(stackId);
        if (stack is null)
        {
            throw new UserInputException($"There is no stack with the id : {stackId}.");
        }

        // We have to pop from stack if it contains at least two elements
        if (stack.Elements.Count < 2)
        {
            throw new UserInputException($"You can't pop from stack as it contains less than two elements.");
        }

        return await _dbContext.ApplyOperand(op, stack);
    }

    public async Task<Stack> GetStack(Guid stackId)
    {
        return await _dbContext.GetStack(stackId);
    }
}