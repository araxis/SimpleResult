using FluentAssertions;
using System;
using Xunit;

namespace SimpleResult.Tests;

public class GenericResultTests
{
    [Fact]
    public void Should_IndicateSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<string>.Success("Success");

        // Act & Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Should_NotIndicateFailure_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<string>.Success("Success");

        // Act & Assert
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Should_NotIndicateSuccess_When_ResultIsFailure()
    {
        // Arrange
        var result = Result<string>.Fail(new InvalidOperationException());

        // Act & Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Should_IndicateFailure_When_ResultIsFailure()
    {
        // Arrange
        var result = Result<string>.Fail(new InvalidOperationException());

        // Act & Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Should_ContainException_When_ResultIsFailure()
    {
        // Arrange
        var expectedException = new InvalidOperationException();
        var result = Result<string>.Fail(expectedException);

        // Act
        var exception = result.ExceptionOrNull();

        // Assert
        exception.Should().Be(expectedException);
    }

    [Fact]
    public void Should_NotContainException_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<string>.Success("Success");

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
        var result = Result<string>.Fail(new[] { error1, error2 });

        // Act
        var errors = result.Errors;

        // Assert
        errors.Should().Contain(new[] { error1, error2 });
    }

    [Fact]
    public void Should_NotContainErrors_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<string>.Success("Success");

        // Act
        var errors = result.Errors;

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Should_ReturnValue_When_GetOrDefaultIsCalledOnSuccess()
    {
        // Arrange
        var expectedResult = "Success";
        var result = Result<string>.Success(expectedResult);

        // Act
        var value = result.GetOrDefault();

        // Assert
        value.Should().Be(expectedResult);
    }

    [Fact]
    public void Should_ReturnDefault_When_GetOrDefaultIsCalledOnFailure()
    {
        // Arrange
        var result = Result<string>.Fail(new InvalidOperationException());

        // Act
        var value = result.GetOrDefault();

        // Assert
        value.Should().Be(default(string));
    }

    [Fact]
    public void Should_CreateSuccessResult_When_ImplicitConversionFromValueToResult()
    {
        // Arrange
        string value = "Success";

        // Act
        Result<string> result = value;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.GetOrDefault().Should().Be(value);
    }
    [Fact]
    public void Should_CreateFailureResult_When_ImplicitConversionFromExceptionToResult()
    {
        // Arrange
        var exception = new InvalidOperationException();

        // Act
        Result<string> result = exception;

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
        Result<string> result = errors;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(errors);
    }
}