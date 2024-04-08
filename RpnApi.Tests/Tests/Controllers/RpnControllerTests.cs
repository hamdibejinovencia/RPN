namespace RpnApi.Tests.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Rpn.Api.Data;
using Rpn.Api.Data.Exceptions;
using Rpn.Api.Domain.Entities;
using Rpn.Api.Domain.Services;
using Rpn.Api.WebApi.V1;

public class RpnControllerTests
{
    private readonly RpnController _controller;
    private readonly RpnService _service;
    private readonly ILogger<RpnService> _logger;
    private readonly RpnContext _dbContext;
    private static Random _random = new Random();

    private static string RandomNumber(int length)
    {
        const string chars = "0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[_random.Next(s.Length)]).ToArray());
    }

    public RpnControllerTests()
    {
        this._logger = Substitute.For<ILogger<RpnService>>();
        this._dbContext = DbContextMocker.GetRpnDbContext(Guid.NewGuid().ToString());
        this._service = new RpnService(this._logger, this._dbContext);
        this._controller = new RpnController(this._service);
    }

    [Fact]
    public async Task GetStacks_WhenCalled_ReturnsOkResultAsync()
    {
        // Arrange

        // Act
        var response = await this._controller.GetStacks() as OkObjectResult;

        // Assert
        Assert.Equal(200, response.StatusCode);
    }

    [Fact]
    public async Task GetStacks_WhenCalled_ReturnsAllItemsAsync()
    {
        // Arrange
        await this._service.CreateStack();
        await this._service.CreateStack();

        // Act
        var result = await this._controller.GetStacks() as OkObjectResult;
        var values = result?.Value as List<Stack>;

        // Assert
        Assert.Equal(2, values?.Count);
    }

    [Fact]
    public async Task GetStack_WhenCalledWithRightGuid_ReturnsTheRightOne()
    {
        // Arrange
        var stack = await this._service.CreateStack();

        // Act
        var result = await this._controller.GetStack(stack.StackId) as OkObjectResult;
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
        async Task Act() => await this._controller.GetStack(guid);

        // Assert
        var exception = await Assert.ThrowsAsync<UserInputException>(Act);
        Assert.Equal($"There is no stack with the id : {guid}.", exception.Message);
    }

    [Fact]
    public async Task PushValue_WhenCalled_ReturnsTheRightStackWithSameValues()
    {
        // Arrange
        var stack = await this._service.CreateStack();

        // Act
        var actual = await this._service.AddValueToStack(stack.StackId, RandomNumber(3));

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
        async Task Act() => await this._controller.DeleteStack(guid);

        // Assert
        var exception = await Assert.ThrowsAsync<UserInputException>(Act);
        Assert.Equal($"There is no stack with the id : {guid}.", exception.Message);
    }

    [Fact]
    public async Task PushValue_WhenInexistingGuid_ThrowsUserInputException()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        async Task Act() => await this._controller.PushValue(guid, "1");

        // Assert
        var exception = await Assert.ThrowsAsync<UserInputException>(Act);
        Assert.Equal($"There is no stack with the id : {guid}.", exception.Message);
    }

    [Fact]
    public async Task PushValue_WhenInvalidOperand_ThrowsUserInputException()
    {
        // Arrange
        var stack = await this._service.CreateStack();

        // Act
        async Task Act() => await this._controller.PushValue(stack.StackId, string.Empty);

        // Assert
        var exception = await Assert.ThrowsAsync<UserInputException>(Act);
        Assert.Equal($"The value you push must be numeric.", exception.Message);
    }

    [Fact]
    public async Task ApplyOperand_WhenInvalidOperator_ThrowsUserInputException()
    {
        // Arrange
        var stack = await this._service.CreateStack();

        // Act
        async Task Act() => await this._controller.PushValue('%', stack.StackId);

        // Assert
        var exception = await Assert.ThrowsAsync<UserInputException>(Act);
        Assert.Equal($"The operator you have entered is not valid.", exception.Message);
    }

    [Fact]
    public async Task ApplyOperand_WhenInexistingGuid_ThrowsUserInputException()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        async Task Act() => await this._controller.PushValue('+', guid);

        // Assert
        var exception = await Assert.ThrowsAsync<UserInputException>(Act);
        Assert.Equal($"There is no stack with the id : {guid}.", exception.Message);
    }

    [Fact]
    public async Task ApplyOperand_WhenLessThenTwoItems_ThrowsUserInputException()
    {
        // Arrange
        var stack = await this._service.CreateStack();

        // Act
        async Task Act() => await this._controller.PushValue('+', stack.StackId);

        // Assert
        var exception = await Assert.ThrowsAsync<UserInputException>(Act);
        Assert.Equal($"You can't pop from stack as it contains less than two elements.", exception.Message);
    }

    [Fact]
    public async Task ApplyOperand_WhenTwoValidOperandsAndAdditionOperand_DoesAdditionOperationCorrectly()
    {
        // Arrange
        var stack = await this._service.CreateStack();
        var op1 = RandomNumber(3);
        var op2 = RandomNumber(3);
        await this._service.AddValueToStack(stack.StackId, op1);
        await this._service.AddValueToStack(stack.StackId, op2);

        // Act
        var actual = await this._service.ApplyOperand('+', stack.StackId);

        // Assert
        Assert.Equal((Convert.ToInt32(op2) + Convert.ToInt32(op1)).ToString(), actual.Elements.Pop());
    }

    [Fact]
    public async Task ApplyOperand_WhenTwoValidOperandsAndSubstractionOperand_DoesSubstractionOperationCorrectly()
    {
        // Arrange
        var stack = await this._service.CreateStack();
        var op1 = RandomNumber(3);
        var op2 = RandomNumber(3);
        await this._service.AddValueToStack(stack.StackId, op1);
        await this._service.AddValueToStack(stack.StackId, op2);

        // Act
        var actual = await this._service.ApplyOperand('-', stack.StackId);

        // Assert
        Assert.Equal((Convert.ToInt32(op2) - Convert.ToInt32(op1)).ToString(), actual.Elements.Pop());
    }

    [Fact]
    public async Task ApplyOperand_WhenTwoValidOperandsAndDivisionOperand_DoesDivisionOperationCorrectly()
    {
        // Arrange
        var stack = await this._service.CreateStack();
        var op1 = RandomNumber(3);
        var op2 = RandomNumber(3);
        await this._service.AddValueToStack(stack.StackId, op1);
        await this._service.AddValueToStack(stack.StackId, op2);

        // Act
        var actual = await this._service.ApplyOperand(':', stack.StackId);

        // Assert
        Assert.Equal((Convert.ToInt32(op2) / Convert.ToInt32(op1)).ToString(), actual.Elements.Pop());
    }

    [Fact]
    public async Task ApplyOperand_WhenTwoValidOperandsAndMultiplicationOperand_DoesMultiplicationOperationCorrectly()
    {
        // Arrange
        var stack = await this._service.CreateStack();
        var op1 = RandomNumber(3);
        var op2 = RandomNumber(3);
        await this._service.AddValueToStack(stack.StackId, op1);
        await this._service.AddValueToStack(stack.StackId, op2);

        // Act
        var actual = await this._service.ApplyOperand('*', stack.StackId);

        // Assert
        Assert.Equal((Convert.ToInt32(op2) * Convert.ToInt32(op1)).ToString(), actual.Elements.Pop());
    }

    [Fact]
    public async Task GetOperands_WhenCalled_ReturnsOkResultAsync()
    {
        // Arrange

        // Act
        var response = await this._controller.GetOperands() as OkObjectResult;

        // Assert
        Assert.Equal(200, response?.StatusCode);
    }

    [Fact]
    public async Task GetOperands_WhenCalled_ReturnsFourOperands()
    {
        // Arrange

        // Act
        var response = await this._controller.GetOperands() as OkObjectResult;
        var values = response?.Value as IEnumerable<char>;

        // Assert
        Assert.Equal(new List<char>() { '+', '-', ':', '*' }, values);
    }
}