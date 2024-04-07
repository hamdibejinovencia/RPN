namespace Rpn.Api.WebApi.V1;
using Rpn.Api.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Rpn.Api.Domain.Entities;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/rpn/")]
public class RpnController : Controller
{
    private readonly IRpnService rpnService;

    public RpnController(IRpnService rpnService) => this.rpnService = rpnService;
    
    #region operands
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "List all the operands", OperationId = "get-operands", Tags = new[] { "Operands" })]
    [SwaggerResponse(StatusCodes.Status200OK, "List the operands", typeof(IEnumerable<string>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Problem with user's input", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something has gone wrong on the web api’s server", typeof(ProblemDetails))]
    [HttpGet("op")]
    public async Task<IActionResult> GetOperands()
    {
        var operands = await this.rpnService.GetOperands();
        return this.Ok(operands);
    }
    #endregion
    
    #region stacks
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "Apply an operand to a stack", OperationId = "apply-operand", Tags = new[] { "Stacks" })]
    [SwaggerResponse(StatusCodes.Status201Created, "Successful operand's application")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Problem with user's input", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something has gone wrong on the web api’s server", typeof(ProblemDetails))]
    [HttpPost("rpn/op/{op}/stack/{stackId}")]
    public async Task<IActionResult> PushValue(char op, Guid stackId)
    {
        var stack = await this.rpnService.ApplyOperand(op,stackId);
        return this.Created("", new { stack });
    }
    
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "Create new stack", OperationId = "create-stack", Tags = new[] { "Stacks" })]
    [SwaggerResponse(StatusCodes.Status201Created, "Successful creation of the stack")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something has gone wrong on the web api’s server", typeof(ProblemDetails))]
    [HttpPost("stack")]
    public async Task<IActionResult> PostStack()
    {
        var createdStack = await this.rpnService.CreateStack();
        return this.Created("", new { createdStack });
    }
    
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "List the available stacks", OperationId = "get-stacks", Tags = new[] { "Stacks" })]
    [SwaggerResponse(StatusCodes.Status200OK, "List the available stacks", typeof(IEnumerable<Stack>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Problem with user's input", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something has gone wrong on the web api’s server", typeof(ProblemDetails))]
    [HttpGet("stack")]
    public async Task<IActionResult> GetStacks()
    {
        var stacks = await this.rpnService.GetStacks();
        return this.Ok(stacks);
    }
    
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "Delete a stack", OperationId = "delete-stacks", Tags = new[] { "Stacks" })]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Stack have been deleted")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Problem with user's input", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something has gone wrong on the web api’s server", typeof(ProblemDetails))]
    [HttpDelete("rpn/stack/{stackId}")]
    public async Task<IActionResult> DeleteEntities(Guid stackId)
    {
        await this.rpnService.DeleteStack(stackId);
        return this.NoContent();
    }
    
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "Push a new value to a stack", OperationId = "push-new-value", Tags = new[] { "Stacks" })]
    [SwaggerResponse(StatusCodes.Status201Created, "Successful push of the value")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Problem with user's input", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something has gone wrong on the web api’s server", typeof(ProblemDetails))]
    [HttpPost("rpn/stack/{stackId}")]
    public async Task<IActionResult> PushValue(Guid stackId, [FromBody] string value)
    {
        var stack = await this.rpnService.AddValueToStack(stackId,value);
        return this.Created("", new { stack });
    }
    
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "Get a stack", OperationId = "get-stack", Tags = new[] { "Stacks" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Stack", typeof(Stack))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Problem with user's input", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something has gone wrong on the web api’s server", typeof(ProblemDetails))]
    [HttpGet("stack/{stackId}")]
    public async Task<IActionResult> GetStack(Guid stackId)
    {
        var stacks = await this.rpnService.GetStack(stackId);
        return this.Ok(stacks);
    }
    #endregion
}

