using System;
using FluentAssertions;
using Xunit;

namespace SimpleResult.Tests;

public class ResultTests
{

    [Fact]
    public void Should_IndicateSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act & Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Should_NotIndicateFailure_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act & Assert
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Should_NotIndicateSuccess_When_ResultIsFailure()
    {
        // Arrange
        var result = Result.Fail(new InvalidOperationException());

        // Act & Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Should_IndicateFailure_When_ResultIsFailure()
    {
        // Arrange
        var result = Result.Fail(new InvalidOperationException());

        // Act & Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Should_ContainException_When_ResultIsFailure()
    {
        // Arrange
        var expectedException = new InvalidOperationException();
        var result = Result.Fail(expectedException);

        // Act
        var exception = result.ExceptionOrNull();

        // Assert
        exception.Should().Be(expectedException);
    }

    [Fact]
    public void Should_NotContainException_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var exception = result.ExceptionOrNull();

        // Assert
        exception.Should().BeNull();
    }

    [Fact]
    public void Should_ContainErrors_When_ResultIsFailureWithErrors()
    {
        // Arrange
        var error1 = new Error("Error 1");
        var error2 = new Error("Error 2");
        var result = Result.Fail(new[] { error1, error2 });

        // Act
        var errors = result.Errors;

        // Assert
        errors.Should().Contain(new[] { error1, error2 });
    }

    [Fact]
    public void Should_CreateFailureResult_When_ImplicitConversionFromExceptionToResult()
    {
        // Arrange
        var exception = new InvalidOperationException();

        // Act
        Result result = exception;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ExceptionOrNull().Should().Be(exception);
    }

    [Fact]
    public void Should_CreateFailureResult_When_ImplicitConversionFromErrorArrayToResult()
    {
        // Arrange
        var errors = new[] { new Error("Error 1"), new Error("Error 2") };

        // Act
        Result result = errors;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(errors);
    }

}