namespace Rpn.Api.WebApi.V1;
using Rpn.Api.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Rpn.Api.Domain.Entities;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/rpn/stack")]
public class RpnController : Controller
{
    private readonly IRpnService rpnService;

    public RpnController(IRpnService rpnService) => this.rpnService = rpnService;

    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "Get stacks", OperationId = "get-stacks", Tags = new[] { "Stacks" })]
    [SwaggerResponse(StatusCodes.Status200OK, "List the available stacks", typeof(IEnumerable<Stack<Item>>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Problem with user's input", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something has gone wrong on the web api’s server", typeof(ProblemDetails))]
    [HttpGet]
    public async Task<IActionResult> GetStacks()
    {
        var stacks = await this.rpnService.GetStacks();
        return this.Ok(stacks);
    }
    
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "Create new stack", OperationId = "create-stack", Tags = new[] { "Stacks" })]
    [SwaggerResponse(StatusCodes.Status201Created, "Successful creation of the stack")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something has gone wrong on the web api’s server", typeof(ProblemDetails))]
    [HttpPost]
    public async Task<IActionResult> PostStack()
    {
        var createdStack = await this.rpnService.CreateStack();
        return this.Created("", new { createdStack });
    }
}

