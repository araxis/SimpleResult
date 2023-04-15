using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace SimpleResult.Tests;

public class RunCatchingExtensionsTest
{
    [Fact]
    public void RunCatching_WhenActionSucceeds_ReturnsSuccess()
    {
        // Arrange
        Action successfulAction = () => { };

        // Act
        var result = successfulAction.RunCatching();

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void RunCatching_WhenActionFails_ReturnsFailure()
    {
        // Arrange
        Action failingAction = () => throw new InvalidOperationException("An error occurred.");

        // Act
        var result = failingAction.RunCatching();

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void RunCatching_WhenActionFails_ReturnsCorrectException()
    {
        // Arrange
        var expectedException = new InvalidOperationException("An error occurred.");
        Action failingAction = () => throw expectedException;

        // Act
        var result = failingAction.RunCatching();

        // Assert
        result.ExceptionOrNull().Should().Be(expectedException);
    }
    [Fact]
    public void RunCatching_WhenFunctionSucceeds_ReturnsSuccess()
    {
        // Arrange
        Func<int> successfulFunction = () => 42;

        // Act
        var result = successfulFunction.RunCatching();

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void RunCatching_WhenFunctionSucceeds_ReturnsCorrectValue()
    {
        // Arrange
        Func<int> successfulFunction = () => 42;

        // Act
        var result = successfulFunction.RunCatching();

        // Assert
        result.GetOrDefault().Should().Be(42);
    }

    [Fact]
    public void RunCatching_WhenFunctionFails_ReturnsFailure()
    {
        // Arrange
        Func<int> failingFunction = () => throw new InvalidOperationException("An error occurred.");

        // Act
        var result = failingFunction.RunCatching();

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void RunCatching_WhenFunctionFails_ReturnsCorrectException()
    {
        // Arrange
        var expectedException = new InvalidOperationException("An error occurred.");
        Func<int> failingFunction = () => throw expectedException;

        // Act
        var result = failingFunction.RunCatching();

        // Assert
        result.ExceptionOrNull().Should().Be(expectedException);
    }
    [Fact]
    public async Task RunCatching_WhenAsyncFunctionSucceeds_ReturnsSuccess()
    {
        // Arrange
        Func<Task<int>> successfulFunction = async () =>
        {
            await Task.Delay(100);
            return 42;
        };

        // Act
        var result = await successfulFunction.RunCatching();

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task RunCatching_WhenAsyncFunctionSucceeds_ReturnsCorrectValue()
    {
        // Arrange
        Func<Task<int>> successfulFunction = async () =>
        {
            await Task.Delay(100);
            return 42;
        };

        // Act
        var result = await successfulFunction.RunCatching();

        // Assert
        result.GetOrDefault().Should().Be(42);
    }

    [Fact]
    public async Task RunCatching_WhenAsyncFunctionFails_ReturnsFailure()
    {
        // Arrange
        Func<Task<int>> failingFunction = async () =>
        {
            await Task.Delay(100);
            throw new InvalidOperationException("An error occurred.");
        };

        // Act
        var result = await failingFunction.RunCatching();

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task RunCatching_WhenAsyncFunctionFails_ReturnsCorrectException()
    {
        // Arrange
        var expectedException = new InvalidOperationException("An error occurred.");
        Func<Task<int>> failingFunction = async () =>
        {
            await Task.Delay(100);
            throw expectedException;
        };

        // Act
        var result = await failingFunction.RunCatching();

        // Assert
        result.ExceptionOrNull().Should().Be(expectedException);
    }
}