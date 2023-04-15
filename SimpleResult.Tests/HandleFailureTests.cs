using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace SimpleResult.Tests;

public class HandleFailureTests
{

    [Fact]
    [Trait("Category", "OnFailure")]
    public void Should_InvokeAction_When_ResultIsFailureWithException()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");
        var result = Result.Fail(exception);
        var actionCalled = false;

        // Act
        result.OnFailure((ex, errs) =>
        {
            actionCalled = true;
            ex.Should().Be(exception);
            errs.Should().BeEmpty();
        });

        // Assert
        actionCalled.Should().BeTrue();
    }

    [Fact]
    [Trait("Category", "OnFailure")]
    public void Should_InvokeAction_When_ResultIsFailureWithoutException()
    {
        // Arrange
        var error = new Error("Test error");
        var result = Result.Fail(error);
        var actionCalled = false;

        // Act
        result.OnFailure((ex, errs) =>
        {
            actionCalled = true;
            ex.Should().BeNull();
            errs.Should().HaveCount(1);
            errs.First().Should().Be(error);
        });

        // Assert
        actionCalled.Should().BeTrue();
    }

    [Fact]
    [Trait("Category", "OnFailure")]
    public void Should_InvokeAction_When_ResultIsFailureWithExceptionAndErrors()
    {
        // Arrange
        var error = new Error("Test error");
        var exception = new InvalidOperationException("Test exception");
        var result = Result.Fail(exception, error);
        var actionCalled = false;

        // Act
        result.OnFailure((ex, errs) =>
        {
            actionCalled = true;
            ex.Should().Be(exception);
            errs.Should().HaveCount(1);
            errs.First().Should().Be(error);
        });

        // Assert
        actionCalled.Should().BeTrue();
    }

    [Fact]
    [Trait("Category", "OnFailure")]
    public void Should_InvokeAction_When_ResultIsFailureWithNoExceptionAndNoErrors()
    {
        // Arrange
        var result = Result.Fail(Array.Empty<IError>());
        var actionCalled = false;

        // Act
        result.OnFailure((ex, errs) =>
        {
            actionCalled = true;
            ex.Should().BeNull();
            errs.Should().BeEmpty();
        });

        // Assert
        actionCalled.Should().BeTrue();
    }

    [Fact]
    [Trait("Category", "OnFailure")]
    public void Should_NotInvokeAction_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();
        var actionCalled = false;

        // Act
        result.OnFailure((ex, errs) =>
        {
            actionCalled = true;
        });

        // Assert
        actionCalled.Should().BeFalse();
    }

    [Fact]
    [Trait("Category", "OnFailure")]
    public void OnFailure_TError_InvokesAction_When_ResultIsFailureWithExceptionAndSpecifiedErrorType()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");
        var error = new CustomError("Test error");
        var result = Result.Fail(exception, error);
        var actionCalled = false;
        Exception? capturedException = null;
        IReadOnlyCollection<IError> capturedErrors = new List<IError>();

        // Act
        result.OnFailure<CustomError>((ex, errs) =>
        {
            actionCalled = true;
            capturedException = ex;
            capturedErrors = errs;
        });

        // Assert
        actionCalled.Should().BeTrue();
        capturedException.Should().Be(exception);
        capturedErrors.Should().HaveCount(1);
        capturedErrors.First().Should().Be(error);
    }

    [Fact]
    [Trait("Category", "OnFailure")]
    public void OnFailure_TError_InvokesAction_When_ResultIsFailureWithErrorsOfDifferentTypes()
    {
        // Arrange
        var customError = new CustomError("Test custom error");
        var anotherError = new Error("Test another error");
        var result = Result.Fail(customError, anotherError);
        var actionCalled = false;
        Exception? capturedException = null;
        IReadOnlyCollection<IError> capturedErrors = new List<IError>();

        // Act
        result.OnFailure<CustomError>((ex, errs) =>
        {
            actionCalled = true;
            capturedException = ex;
            capturedErrors = errs;
        });

        // Assert
        actionCalled.Should().BeTrue();
        capturedException.Should().BeNull();
        capturedErrors.Should().HaveCount(1);
        capturedErrors.First().Should().Be(customError);
    }

    [Fact]
    [Trait("Category", "OnFailure")]
    public void OnFailure_TError_DoesNotInvokeAction_When_ResultIsFailureWithExceptionAndWithoutSpecifiedErrorType()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");
        var result = Result.Fail(exception);
        var actionCalled = false;

        // Act
        result.OnFailure<CustomError>((ex, errs) =>
        {
            actionCalled = true;
        });

        // Assert
        actionCalled.Should().BeFalse();
    }

    [Fact]
    [Trait("Category", "OnFailure")]
    public void OnFailure_TError_DoesNotInvokeAction_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();
        var actionCalled = false;

        // Act
        result.OnFailure<CustomError>((ex, errs) =>
        {
            actionCalled = true;
        });

        // Assert
        actionCalled.Should().BeFalse();
    }

    [Fact]
    [Trait("Category", "OnException")]
    public void OnException_InvokesAction_When_ResultIsFailureWithException()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");
        var result = Result.Fail(exception);
        var actionCalled = false;
        Exception? capturedException = null;
        IReadOnlyCollection<IError> capturedErrors = new List<IError>();

        // Act
        result.OnException((ex, errs) =>
        {
            actionCalled = true;
            capturedException = ex;
            capturedErrors = errs;
        });

        // Assert
        actionCalled.Should().BeTrue();
        capturedException.Should().Be(exception);
        capturedErrors.Should().BeEmpty();
    }

    [Fact]
    [Trait("Category", "OnException")]
    public void OnException_DoesNotInvokeAction_When_ResultIsFailureWithoutException()
    {
        // Arrange
        var error = new CustomError("Test error");
        var result = Result.Fail(error);
        var actionCalled = false;

        // Act
        result.OnException((ex, errs) =>
        {
            actionCalled = true;
        });

        // Assert
        actionCalled.Should().BeFalse();
    }

    [Fact]
    [Trait("Category", "OnException")]
    public void OnException_DoesNotInvokeAction_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();
        var actionCalled = false;

        // Act
        result.OnException((ex, errs) =>
        {
            actionCalled = true;
        });

        // Assert
        actionCalled.Should().BeFalse();
    }


    [Fact]
    [Trait("Category", "OnExceptionTyped")]
    public void OnExceptionTyped_InvokesAction_When_ResultIsFailureWithMatchingExceptionType()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");
        var result = Result.Fail(exception);
        var actionCalled = false;
        Exception? capturedException = null;
        IReadOnlyCollection<IError> capturedErrors = new List<IError>();

        // Act
        result.OnException<InvalidOperationException>((ex, errs) =>
        {
            actionCalled = true;
            capturedException = ex;
            capturedErrors = errs;
        });

        // Assert
        actionCalled.Should().BeTrue();
        capturedException.Should().Be(exception);
        capturedErrors.Should().BeEmpty();
    }

    [Fact]
    [Trait("Category", "OnExceptionTyped")]
    public void OnExceptionTyped_DoesNotInvokeAction_When_ResultIsFailureWithNonMatchingExceptionType()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");
        var result = Result.Fail(exception);
        var actionCalled = false;

        // Act
        result.OnException<ArgumentNullException>((ex, errs) =>
        {
            actionCalled = true;
        });

        // Assert
        actionCalled.Should().BeFalse();
    }

    [Fact]
    [Trait("Category", "OnExceptionTyped")]
    public void OnExceptionTyped_DoesNotInvokeAction_When_ResultIsFailureWithoutException()
    {
        // Arrange
        var error = new CustomError("Test error");
        var result = Result.Fail(error);
        var actionCalled = false;

        // Act
        result.OnException<InvalidOperationException>((ex, errs) =>
        {
            actionCalled = true;
        });

        // Assert
        actionCalled.Should().BeFalse();
    }

    [Fact]
    [Trait("Category", "OnExceptionTyped")]
    public void OnExceptionTyped_DoesNotInvokeAction_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();
        var actionCalled = false;

        // Act
        result.OnException<InvalidOperationException>((ex, errs) =>
        {
            actionCalled = true;
        });

        // Assert
        actionCalled.Should().BeFalse();
    }


    [Trait("Category", "OnInnerException")]
    [Fact]
    public void OnInnerException_InvokesAction_When_ResultIsFailureWithMatchingInnerExceptionType()
    {
        // Arrange
        var innerException = new InvalidOperationException("Inner exception");
        var exception = new Exception("Test exception", innerException);
        var result = Result.Fail(exception);
        var actionCalled = false;
        Exception? capturedException = null;
        IReadOnlyCollection<IError> capturedErrors = new List<IError>();

        // Act
        result.OnInnerException<InvalidOperationException>((ex, errs) =>
        {
            actionCalled = true;
            capturedException = ex;
            capturedErrors = errs;
        });

        // Assert
        actionCalled.Should().BeTrue();
        capturedException.Should().Be(innerException);
        capturedErrors.Should().BeEmpty();
    }

    [Trait("Category", "OnInnerException")]
    [Fact]
    public void OnInnerException_DoesNotInvokeAction_When_ResultIsFailureWithNonMatchingInnerExceptionType()
    {
        // Arrange
        var innerException = new InvalidOperationException("Inner exception");
        var exception = new Exception("Test exception", innerException);
        var result = Result.Fail(exception);
        var actionCalled = false;

        // Act
        result.OnInnerException<ArgumentNullException>((ex, errs) =>
        {
            actionCalled = true;
        });

        // Assert
        actionCalled.Should().BeFalse();
    }

    [Trait("Category", "OnInnerException")]
    [Fact]
    public void OnInnerException_DoesNotInvokeAction_When_ResultIsFailureWithoutInnerException()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");
        var result = Result.Fail(exception);
        var actionCalled = false;

        // Act
        result.OnInnerException<InvalidOperationException>((ex, errs) =>
        {
            actionCalled = true;
        });

        // Assert
        actionCalled.Should().BeFalse();
    }

    [Trait("Category", "OnInnerException")]
    [Fact]
    public void OnInnerException_DoesNotInvokeAction_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();
        var actionCalled = false;

        // Act
        result.OnInnerException<InvalidOperationException>((ex, errs) =>
        {
            actionCalled = true;
        });

        // Assert
        actionCalled.Should().BeFalse();
    }
}