using FluentAssertions;
using Xunit;

namespace SimpleResult.Tests;

public class SucceedResultTests
{
    [Fact(DisplayName = "OnSuccess: When result is success, calls action")]
    [Trait("Category", "OnSuccess")]
    public void OnSuccess_Success_CallsAction()
    {
        // Arrange
        var result = Result.Success();
        bool actionCalled = false;

        // Act
        result.OnSuccess(() => actionCalled = true);

        // Assert
        actionCalled.Should().BeTrue();
    }

    [Fact(DisplayName = "OnSuccess: When result is failure, does not call action")]
    [Trait("Category", "OnSuccess")]
    public void OnSuccess_Failure_DoesNotCallAction()
    {
        // Arrange
        var result = Result.Fail(new CustomError("error"));
        bool actionCalled = false;

        // Act
        result.OnSuccess(() => actionCalled = true);

        // Assert
        actionCalled.Should().BeFalse();
    }

    [Fact(DisplayName = "OnSuccess: When result is success, calls action with value")]
    [Trait("Category", "OnSuccess")]
    public void OnSuccessT_Success_CallsActionWithValue()
    {
        // Arrange
        var result = Result<int>.Success(42);
        int value = 0;

        // Act
        result.OnSuccess(x => value = x);

        // Assert
        value.Should().Be(42);
    }

    [Fact(DisplayName = "OnSuccess: When result is failure, does not call action with value")]
    [Trait("Category", "OnSuccess")]
    public void OnSuccessT_Failure_DoesNotCallActionWithValue()
    {
        // Arrange
        var result = Result<int>.Fail(new CustomError("error"));
        int value = 0;

        // Act
        result.OnSuccess(x => value = x);

        // Assert
        value.Should().Be(0);
    }
}