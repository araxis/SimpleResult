using FluentAssertions;
using System;
using Xunit;

namespace SimpleResult.Tests;

public class MapResultTests
{
    [Fact(DisplayName = "Switch_OnSuccessExecuted_WhenResultIsSuccess"), Trait("Category", "Switch")]
    public void Switch_OnSuccessExecuted_WhenResultIsSuccess()
    {
        // Arrange
        var successResult = Result.Success();
        var successCalled = false;
        var failureCalled = false;

        // Act
        successResult.Switch(() => successCalled = true, ex => failureCalled = true);

        // Assert
        successCalled.Should().BeTrue();
        failureCalled.Should().BeFalse();
    }

    [Fact(DisplayName = "Switch_OnFailureExecuted_WhenResultIsFailure"), Trait("Category", "Switch")]
    public void Switch_OnFailureExecuted_WhenResultIsFailure()
    {
        // Arrange
        var exception = new InvalidOperationException();
        var failureResult = Result.Fail(exception);
        var successCalled = false;
        var failureCalled = false;

        // Act
        failureResult.Switch(() => successCalled = true, ex => failureCalled = true);

        // Assert
        successCalled.Should().BeFalse();
        failureCalled.Should().BeTrue();
    }

    [Fact(DisplayName = "Switch_OnSuccessExecutedWithResult_WhenResultIsSuccess"), Trait("Category", "Switch")]
    public void Switch_OnSuccessExecutedWithResult_WhenResultIsSuccess()
    {
        // Arrange
        var expectedResult = "success";
        var successResult = Result<string>.Success(expectedResult);
        var actualResult = "";
        var failureCalled = false;

        // Act
        successResult.Switch(
            res => actualResult = res,
            ex => failureCalled = true);

        // Assert
        actualResult.Should().Be(expectedResult);
        failureCalled.Should().BeFalse();
    }

    [Fact(DisplayName = "Switch_OnFailureExecutedWithException_WhenResultIsFailure"), Trait("Category", "Switch")]
    public void Switch_OnFailureExecutedWithException_WhenResultIsFailure()
    {
        // Arrange
        var exception = new InvalidOperationException();
        var failureResult = Result.Fail(exception);
        var successCalled = false;
        var actualException = new Exception();

        // Act
        failureResult.Switch(
            () => successCalled = true,
            ex => actualException = ex);

        // Assert
        successCalled.Should().BeFalse();
        actualException.Should().Be(exception);
    }

    [Fact(DisplayName = "Fold_WhenSuccess_ReturnsOnSuccessResult")]
    [Trait("Category", "Fold")]
    public void Fold_WhenSuccess_ReturnsOnSuccessResult()
    {
        // Arrange
        var result = Result.Success();
        Func<string> onSuccess = () => "success";
        Func<Exception, string> onFailure = _ => "failure";

        // Act
        var output = result.Fold(onSuccess, onFailure);

        // Assert
        output.Should().Be("success");
    }

    [Fact(DisplayName = "Fold_WhenFailure_ReturnsOnFailureResult")]
    [Trait("Category", "Fold")]
    public void Fold_WhenFailure_ReturnsOnFailureResult()
    {
        // Arrange
        var exception = new Exception("test exception");
        var result = Result.Fail(exception);
        Func<string> onSuccess = () => "success";
        Func<Exception, string> onFailure = ex => ex.Message;

        // Act
        var output = result.Fold(onSuccess, onFailure);

        // Assert
        output.Should().Be(exception.Message);
    }

    [Fact(DisplayName = "Fold_WhenSuccess_ReturnsOnSuccessResultWithGenericTypes")]
    [Trait("Category", "Fold")]
    public void Fold_WhenSuccess_ReturnsOnSuccessResultWithGenericTypes()
    {
        // Arrange
        var result = Result<int>.Success(5);
        Func<int, string> onSuccess = value => $"success {value}";
        Func<Exception, string> onFailure = _ => "failure";

        // Act
        var output = result.Fold(onSuccess, onFailure);

        // Assert
        output.Should().Be("success 5");
    }

    [Fact(DisplayName = "Fold_WhenFailure_ReturnsOnFailureResultWithGenericTypes")]
    [Trait("Category", "Fold")]
    public void Fold_WhenFailure_ReturnsOnFailureResultWithGenericTypes()
    {
        // Arrange
        var exception = new Exception("test exception");
        var result = Result<int>.Fail(exception);
        Func<int, string> onSuccess = value => $"success {value}";
        Func<Exception, string> onFailure = ex => ex.Message;

        // Act
        var output = result.Fold(onSuccess, onFailure);

        // Assert
        output.Should().Be(exception.Message);
    }

    [Fact(DisplayName = "Map_WhenSuccess_ReturnsSuccessResultWithTransformedValue")]
    [Trait("Category", "Map")]
    public void Map_WhenSuccess_ReturnsSuccessResultWithTransformedValue()
    {
        // Arrange
        var result = Result<int>.Success(5);

        // Act
        var output = result.Map(value => $"success {value}");

        // Assert
        output.IsSuccess.Should().BeTrue();
        output.GetOrDefault().Should().Be("success 5");
    }

    [Fact(DisplayName = "Map_WhenFailure_ReturnsFailureResult")]
    [Trait("Category", "Map")]
    public void Map_WhenFailure_ReturnsFailureResult()
    {
        // Arrange
        var exception = new Exception("test exception");
        var result = Result<int>.Fail(exception);

        // Act
        var output = result.Map(value => $"success {value}");

        // Assert
        output.IsFailure.Should().BeTrue();
        output.ExceptionOrNull().Should().Be(exception);
    }
}