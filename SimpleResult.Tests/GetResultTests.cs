using FluentAssertions;
using System;
using Xunit;

namespace SimpleResult.Tests;

public class GetResultTests
{
    [Fact(DisplayName = "ThrowOnFailure throws exception when IResult is failure")]
    [Trait("Category", "OnFailure")]
    public void ThrowOnFailure_WhenIResultIsFailure_ThrowsException()
    {
        // Arrange
        var result = Result.Fail(new Exception("Something went wrong"));

        // Act and assert
        Assert.Throws<Exception>(() => result.ThrowOnFailure());
    }

    [Fact(DisplayName = "ThrowOnFailure does not throw exception when IResult is success")]
    [Trait("Category", "OnFailure")]
    public void ThrowOnFailure_WhenIResultIsSuccess_DoesNotThrowException()
    {
        // Arrange
        var result = Result.Success();

        // Act
        result.ThrowOnFailure();

        // Assert
        Assert.True(true); // No exception was thrown
    }

    [Fact(DisplayName = "GetOrThrow throws exception when IResult is failure")]
    [Trait("Category", "OnFailure")]
    public void GetOrThrow_WhenIResultIsFailure_ThrowsException()
    {
        // Arrange
        var result = Result<int>.Fail(new Exception("Something went wrong"));

        // Act and assert
        Assert.Throws<Exception>(() => result.GetOrThrow());
    }

    [Fact(DisplayName = "GetOrThrow returns the value when IResult is success")]
    [Trait("Category", "OnSuccess")]
    public void GetOrThrow_WhenIResultIsSuccess_ReturnsValue()
    {
        // Arrange
        var expectedValue = 42;
        var result = Result<int>.Success(expectedValue);

        // Act
        var actualValue = result.GetOrThrow();

        // Assert
        actualValue.Should().Be(expectedValue);
    }

    [Fact(DisplayName = "GetOrDefault returns the default value when IResult is failure")]
    [Trait("Category", "OnFailure")]
    public void GetOrDefault_WhenIResultIsFailure_ReturnsDefaultValue()
    {
        // Arrange
        var defaultValue = 0;
        var result = Result<int>.Fail(new Exception("Something went wrong"));

        // Act
        var actualValue = result.GetOrDefault(defaultValue);

        // Assert
        actualValue.Should().Be(defaultValue);
    }

    [Fact(DisplayName = "GetOrDefault returns the value when IResult is success")]
    [Trait("Category", "OnSuccess")]
    public void GetOrDefault_WhenIResultIsSuccess_ReturnsValue()
    {
        // Arrange
        var expectedValue = 42;
        var defaultValue = 0;
        var result = Result<int>.Success(expectedValue);

        // Act
        var actualValue = result.GetOrDefault(defaultValue);

        // Assert
        actualValue.Should().Be(expectedValue);
    }
}