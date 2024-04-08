using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Rpn.Api.Data;
using Rpn.Api.Data.Exceptions;
using Rpn.Api.Domain.Entities;
using Rpn.Api.Domain.Services;
using Rpn.Api.WebApi.V1;

namespace RpnApi.Tests.Tests.Controllers;

public class RpnControllerTests
{
    private readonly RpnController controller;
    private readonly RpnService service;
    private readonly ILogger<RpnService> logger;
    private readonly RpnContext dbContext;
    private static Random random = new Random();

    public static string RandomNumber(int length)
    {
        const string chars = "0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public RpnControllerTests()
    {
        this.logger = Substitute.For<ILogger<RpnService>>();
        this.dbContext = DbContextMocker.GetRpnDbContext(Guid.NewGuid().ToString());
        this.service = new RpnService(this.logger, this.dbContext);
        this.controller = new RpnController(this.service);
    }

    [Fact]
    public async Task GetStacks_WhenCalled_ReturnsOkResultAsync()
    {
        // Arrange

        // Act
        var response = await this.controller.GetStacks() as OkObjectResult;

        // Assert
        Assert.Equal(200, response.StatusCode);
    }

    [Fact]
    public async Task GetStacks_WhenCalled_ReturnsAllItemsAsync()
    {
        // Arrange
        await this.service.CreateStack();
        await this.service.CreateStack();

        // Act
        var result = await this.controller.GetStacks() as OkObjectResult;
        var values = result?.Value as List<Stack>;

        // Assert
        Assert.Equal(2, values?.Count);
    }

    [Fact]
    public async Task GetStack_WhenCalledWithRightGuid_ReturnsTheRightOne()
    {
        // Arrange
        var stack = await this.service.CreateStack();

        // Act
        var result = await this.controller.GetStack(stack.StackId) as OkObjectResult;
        var value = result?.Value as Stack;

        // Assert
        Assert.Equal(stack, value);
    }

    [Fact]
    public async Task GetStack_WhenCalledWithWrongGuid_ThrowsUserInputException()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        async Task act() => await this.controller.GetStack(guid);

        // Assert
        var exception = await Assert.ThrowsAsync<UserInputException>(act);
        Assert.Equal($"There is no stack with the id : {guid}.", exception.Message);
    }

    [Fact]
    public async Task PushValue_WhenCalled_ReturnsTheRightStackWithSameValues()
    {
        // Arrange
        var stack = await this.service.CreateStack();

        // Act
        var actual = await this.service.AddValueToStack(stack.StackId, RandomNumber(3));

        // Assert
        Assert.Equal(stack, actual);
        Assert.Equal(stack.StackId, actual.StackId);
        Assert.Equal(stack.Elements, actual.Elements);
        Assert.True(stack.Elements.SequenceEqual(actual.Elements));
    }

    [Fact]
    public async Task DeleteStack_WhenInexistingGuid_ThrowsUserInputException()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        async Task act() => await this.controller.DeleteStack(guid);

        // Assert
        var exception = await Assert.ThrowsAsync<UserInputException>(act);
        Assert.Equal($"There is no stack with the id : {guid}.", exception.Message);
    }

    [Fact]
    public async Task PushValue_WhenInexistingGuid_ThrowsUserInputException()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        async Task act() => await this.controller.PushValue(guid, "1");

        // Assert
        var exception = await Assert.ThrowsAsync<UserInputException>(act);
        Assert.Equal($"There is no stack with the id : {guid}.", exception.Message);
    }

    [Fact]
    public async Task PushValue_WhenInvalidOperand_ThrowsUserInputException()
    {
        // Arrange
        var stack = await this.service.CreateStack();

        // Act
        async Task act() => await this.controller.PushValue(stack.StackId, string.Empty);

        // Assert
        var exception = await Assert.ThrowsAsync<UserInputException>(act);
        Assert.Equal($"The value you push must be numeric.", exception.Message);
    }

    [Fact]
    public async Task ApplyOperand_WhenInvalidOperator_ThrowsUserInputException()
    {
        // Arrange
        var stack = await this.service.CreateStack();

        // Act
        async Task act() => await this.controller.PushValue('%', stack.StackId);

        // Assert
        var exception = await Assert.ThrowsAsync<UserInputException>(act);
        Assert.Equal($"The operator you have entered is not valid.", exception.Message);
    }

    [Fact]
    public async Task ApplyOperand_WhenInexistingGuid_ThrowsUserInputException()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        async Task act() => await this.controller.PushValue('+', guid);

        // Assert
        var exception = await Assert.ThrowsAsync<UserInputException>(act);
        Assert.Equal($"There is no stack with the id : {guid}.", exception.Message);
    }

    [Fact]
    public async Task ApplyOperand_WhenLessThenTwoItems_ThrowsUserInputException()
    {
        // Arrange
        var stack = await this.service.CreateStack();

        // Act
        async Task act() => await this.controller.PushValue('+', stack.StackId);

        // Assert
        var exception = await Assert.ThrowsAsync<UserInputException>(act);
        Assert.Equal($"You can't pop from stack as it contains less than two elements.", exception.Message);
    }

    [Fact]
    public async Task ApplyOperand_WhenTwoValidOperandsAndAdditionOperand_DoesAdditionOperationCorrectly()
    {
        // Arrange
        var stack = await this.service.CreateStack();
        var op1 = RandomNumber(3);
        var op2 = RandomNumber(3);
        await this.service.AddValueToStack(stack.StackId, op1);
        await this.service.AddValueToStack(stack.StackId, op2);

        // Act
        var actual = await this.service.ApplyOperand('+', stack.StackId);

        // Assert
        Assert.Equal((Convert.ToInt32(op2) + Convert.ToInt32(op1)).ToString(), actual.Elements.Pop());
    }

    [Fact]
    public async Task ApplyOperand_WhenTwoValidOperandsAndSubstractionOperand_DoesSubstractionOperationCorrectly()
    {
        // Arrange
        var stack = await this.service.CreateStack();
        var op1 = RandomNumber(3);
        var op2 = RandomNumber(3);
        await this.service.AddValueToStack(stack.StackId, op1);
        await this.service.AddValueToStack(stack.StackId, op2);

        // Act
        var actual = await this.service.ApplyOperand('-', stack.StackId);

        // Assert
        Assert.Equal((Convert.ToInt32(op2) - Convert.ToInt32(op1)).ToString(), actual.Elements.Pop());
    }

    [Fact]
    public async Task ApplyOperand_WhenTwoValidOperandsAndDivisionOperand_DoesDivisionOperationCorrectly()
    {
        // Arrange
        var stack = await this.service.CreateStack();
        var op1 = RandomNumber(3);
        var op2 = RandomNumber(3);
        await this.service.AddValueToStack(stack.StackId, op1);
        await this.service.AddValueToStack(stack.StackId, op2);

        // Act
        var actual = await this.service.ApplyOperand('/', stack.StackId);

        // Assert
        Assert.Equal((Convert.ToInt32(op2) / Convert.ToInt32(op1)).ToString(), actual.Elements.Pop());
    }

    [Fact]
    public async Task ApplyOperand_WhenTwoValidOperandsAndMultiplicationOperand_DoesMultiplicationOperationCorrectly()
    {
        // Arrange
        var stack = await this.service.CreateStack();
        var op1 = RandomNumber(3);
        var op2 = RandomNumber(3);
        await this.service.AddValueToStack(stack.StackId, op1);
        await this.service.AddValueToStack(stack.StackId, op2);

        // Act
        var actual = await this.service.ApplyOperand('/', stack.StackId);

        // Assert
        Assert.Equal((Convert.ToInt32(op2) / Convert.ToInt32(op1)).ToString(), actual.Elements.Pop());
    }

    [Fact]
    public async Task GetOperands_WhenCalled_ReturnsOkResultAsync()
    {
        // Arrange

        // Act
        var response = await this.controller.GetOperands() as OkObjectResult;

        // Assert
        Assert.Equal(200, response?.StatusCode);
    }

    [Fact]
    public async Task GetOperands_WhenCalled_ReturnsFourOperands()
    {
        // Arrange

        // Act
        var response = await this.controller.GetOperands() as OkObjectResult;
        var values = response?.Value as IEnumerable<char>;

        // Assert
        Assert.Equal(new List<char>() { '+', '-', '/', '*' }, values);
    }
}