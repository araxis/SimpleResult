using System;
using FluentAssertions;
using Xunit;

namespace SimpleResult.Tests;

public class ResultExtensionsShould
{

    [Fact]
    public void RunCatchingReturnResultWithFuncReturnValueWhenFuncDoseNotThrowException()
    {
        var funcReturn = "Test";
        string Func() => funcReturn;

        var result = ResultExtensions.RunCatching(Func);

        result.Should().BeOfType<Result<string>>().Which.GetOrDefault().Should().Be(funcReturn);
    }

    [Fact]
    public void RunCatchingReturnResultWithFuncExceptionWhenFuncThrowException()
    {
        var funcException = new Exception("Func Exception");
        string Func() => throw funcException;

        var result = ResultExtensions.RunCatching(Func);

        result.Should().BeOfType<Result<string>>().Which.ExceptionOrNull().Should().Be(funcException);
    }

    [Fact]
    public void ThrowOnFailureThrowExceptionWhenResultIfResultSetToException()
    {
        var exceptionMessage = "Test";
        Result<string> result = new Exception(exceptionMessage);
      //  var x = result.GetOrDefault();
        var act = () => result.ThrowOnFailure();
        act.Should().Throw<Exception>().Which.Message.Should().Be(exceptionMessage);
    }
}