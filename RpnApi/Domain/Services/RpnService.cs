namespace Rpn.Api.Domain.Services;
using Rpn.Api.Data;
using Rpn.Api.Domain.Entities;

public interface IRpnService
{
    Task<IEnumerable<Stack>> GetStacks();
    Task<Stack> CreateStack();
}

public class RpnService : IRpnService
{
    private readonly ILogger logger;
    private readonly RpnContext context;

    public RpnService(ILogger<RpnService> logger, RpnContext context)
    {
        this.logger = logger;
        this.context = context;
    }

    public async Task<IEnumerable<Stack>> GetStacks()
    {
        return await context.GetStacks();
    }
    
    public async Task<Stack> CreateStack()
    {
        return await context.CreateStack();
    }
}
