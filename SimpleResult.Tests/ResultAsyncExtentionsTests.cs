using FluentAssertions;
using System.Threading.Tasks;
using System;
using Xunit;

namespace SimpleResult.Tests;

public class ResultAsyncExtensionsTests
{
      private static async Task<string> SuccessAsyncOperation()
    {
        await Task.Delay(50);
        return "Success";
    }

    private static async Task<string> FailureAsyncOperation()
    {
        await Task.Delay(50);
        throw new InvalidOperationException("Failure");
    }

    [Fact]
    [Trait("Category", "OnSuccessAsync")]
    public async Task OnSuccessAsync_ShouldExecuteAction_WhenResultIsSuccess()
    {
        var result = Result<string>.Success("Success");
        bool executed = false;

        await result.OnSuccess(async _ =>
        {
            executed = true;
            await Task.CompletedTask;
        });

        executed.Should().BeTrue();
    }

    [Fact]
    [Trait("Category", "OnSuccessAsync")]
    public async Task OnSuccessAsync_ShouldNotExecuteAction_WhenResultIsFailure()
    {
        var result = Result<string>.Fail(new InvalidOperationException("Failure"));
        bool executed = false;

        await result.OnSuccess(async _ =>
        {
            executed = true;
            await Task.CompletedTask;
        });

        executed.Should().BeFalse();
    }

    [Fact]
    [Trait("Category", "OnFailureAsync")]
    public async Task OnFailureAsync_ShouldExecuteAction_WhenResultIsFailure()
    {
        var result = Result<string>.Fail(new InvalidOperationException("Failure"));
        bool executed = false;

        await result.OnFailure(async (ex, errors) =>
        {
            executed = true;
            await Task.CompletedTask;
        });

        executed.Should().BeTrue();
    }

    [Fact]
    [Trait("Category", "OnFailureAsync")]
    public async Task OnFailureAsync_ShouldNotExecuteAction_WhenResultIsSuccess()
    {
        var result = Result<string>.Success("Success");
        bool executed = false;

        await result.OnFailure(async (ex, errors) =>
        {
            executed = true;
            await Task.CompletedTask;
        });

        executed.Should().BeFalse();
    }
    [Fact]
    [Trait("Category", "OnExceptionAsync")]
    public async Task OnExceptionAsync_ShouldExecuteAction_WhenResultHasException()
    {
        var result = Result<string>.Fail(new InvalidOperationException("Failure"));
        bool executed = false;

        await result.OnException<InvalidOperationException>(async (ex, errors) =>
        {
            executed = true;
            await Task.CompletedTask;
        });

        executed.Should().BeTrue();
    }

    [Fact]
    [Trait("Category", "OnExceptionAsync")]
    public async Task OnExceptionAsync_ShouldNotExecuteAction_WhenResultHasNoException()
    {
        var result = Result<string>.Success("Success");
        bool executed = false;

        await result.OnException<InvalidOperationException>(async (ex, errors) =>
        {
            executed = true;
            await Task.CompletedTask;
        });

        executed.Should().BeFalse();
    }

    [Fact]
    [Trait("Category", "OnInnerExceptionAsync")]
    public async Task OnInnerExceptionAsync_ShouldExecuteAction_WhenResultHasInnerException()
    {
        var innerException = new InvalidOperationException("Inner Failure");
        var outerException = new ApplicationException("Outer Failure", innerException);
        var result = Result<string>.Fail(outerException);
        bool executed = false;

        await result.OnInnerException<InvalidOperationException>(async (ex, errors) =>
        {
            executed = true;
            await Task.CompletedTask;
        });

        executed.Should().BeTrue();
    }

    [Fact]
    [Trait("Category", "OnInnerExceptionAsync")]
    public async Task OnInnerExceptionAsync_ShouldNotExecuteAction_WhenResultHasNoInnerException()
    {
        var result = Result<string>.Fail(new InvalidOperationException("Failure"));
        bool executed = false;

        await result.OnInnerException<InvalidOperationException>(async (ex, errors) =>
        {
            executed = true;
            await Task.CompletedTask;
        });

        executed.Should().BeFalse();
    }

    [Fact]
    [Trait("Category", "SwitchAsync")]
    public async Task SwitchAsync_ShouldExecuteOnSuccessAction_WhenResultIsSuccess()
    {
        var result = Result<string>.Success("Success");
        bool onSuccessExecuted = false;
        bool onFailureExecuted = false;

        await result.Switch(
            async () =>
            {
                onSuccessExecuted = true;
                await Task.CompletedTask;
            },
            async (ex, errors) =>
            {
                onFailureExecuted = true;
                await Task.CompletedTask;
            });

        onSuccessExecuted.Should().BeTrue();
        onFailureExecuted.Should().BeFalse();
    }

    [Fact]
    [Trait("Category", "SwitchAsync")]
    public async Task SwitchAsync_ShouldExecuteOnFailureAction_WhenResultIsFailure()
    {
        var result = Result<string>.Fail(new InvalidOperationException("Failure"));
        bool onSuccessExecuted = false;
        bool onFailureExecuted = false;

        await result.Switch(
            async () =>
            {
                onSuccessExecuted = true;
                await Task.CompletedTask;
            },
            async (ex, errors) =>
            {
                onFailureExecuted = true;
                await Task.CompletedTask;
            });

        onSuccessExecuted.Should().BeFalse();
        onFailureExecuted.Should().BeTrue();
    }
    [Fact]
    [Trait("Category", "FoldAsync")]
    public async Task FoldAsync_ShouldExecuteOnSuccessFunc_WhenResultIsSuccess()
    {
        var result = Result<string>.Success("Success");

        string returnedValue = await result.Fold(
            async () =>
            {
                await Task.CompletedTask;
                return "OnSuccess";
            },
            async (ex, errors) =>
            {
                await Task.CompletedTask;
                return "OnFailure";
            });

        returnedValue.Should().Be("OnSuccess");
    }

    [Fact]
    [Trait("Category", "FoldAsync")]
    public async Task FoldAsync_ShouldExecuteOnFailureFunc_WhenResultIsFailure()
    {
        var result = Result<string>.Fail(new InvalidOperationException("Failure"));

        string returnedValue = await result.Fold(
            async () =>
            {
                await Task.CompletedTask;
                return "OnSuccess";
            },
            async (ex, errors) =>
            {
                await Task.CompletedTask;
                return "OnFailure";
            });

        returnedValue.Should().Be("OnFailure");
    }

    [Fact]
    [Trait("Category", "MapAsync")]
    public async Task MapAsync_ShouldTransformResultValue_WhenResultIsSuccess()
    {
        var result = Result<int>.Success(2);

        var newResult = await result.Map(async value =>
        {
            await Task.CompletedTask;
            return value * 2;
        });

        newResult.IsSuccess.Should().BeTrue();
        newResult.GetOrDefault().Should().Be(4);
    }

    [Fact]
    [Trait("Category", "MapAsync")]
    public async Task MapAsync_ShouldNotTransformResultValue_WhenResultIsFailure()
    {
        var result = Result<int>.Fail(new InvalidOperationException("Failure"));

        var newResult = await result.Map(async value =>
        {
            await Task.CompletedTask;
            return value * 2;
        });

        newResult.IsFailure.Should().BeTrue();
    }
}

