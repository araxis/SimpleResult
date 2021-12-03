﻿using System;
using FluentAssertions;
using Xunit;

namespace SimpleResult.Tests;

public class ResultExtensionsTests
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
        var act = () => result.ThrowOnFailure();
        act.Should().Throw<Exception>().Which.Message.Should().Be(exceptionMessage);
    }

    [Fact]
    public void ThrowOnFailureDosNotThrowExceptionWhenResultIfResultSetToAnyThingsElseException()
    {
       
        Result<string> result = "Test";
        var act = () => result.ThrowOnFailure();
        act.Should().NotThrow();
    }

    [Fact]
    public void GetOrThrowThrowExceptionIfResultSetToException()
    {
        Result<string> result = new Exception("Test");
        var act = () => result.GetOrThrow();
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void GetOrThrowDosNotThrowExceptionIfResultSetAnythingsElseException()
    {
        var resultValue = "Test";
        Result<string> result = resultValue;
        var act = () => result.GetOrThrow();
        act.Should().NotThrow();
    }

    [Fact]
    public void OnSuccessCallActionIfIsSuccess()
    {
        var resultValue = "Test";
        var actionCallTest = "";
        Result<string> result = resultValue;

        void Action(string input) => actionCallTest = input;


        result.OnSuccess(Action);
        actionCallTest.Should().Be(resultValue);


    }
    [Fact]
    public void OnSuccessDoNotCallActionIfIsFailure()
    {
        
        var actionCallTest = "";
        Result<string> result = new Exception();

        void Action(string input) => actionCallTest = input;


        result.OnSuccess(Action);
        actionCallTest.Should().BeEmpty();


    }

    [Fact]
    public void OnFailureCallActionIfIsFailure()
    {
        
        Exception exception = null;

        Result<string> result = new Exception();

        void Action(Exception ex) => exception = ex;

        result.OnFailure(Action);
        exception.Should().NotBeNull();

    }

    [Fact]
    public void OnFailureDoNotCallActionIfIsSuccess()
    {
        
        Exception exception = null;
        Result<string> result = "Test";

        void Action(Exception ex) => exception = ex;

        result.OnFailure(Action);
        exception.Should().BeNull();

    }

    [Fact]
    public void FoldReturnOnSuccessReturnIfIsSuccess()
    {
        var resultValue = "Test";
        var foldResult = 0;
        var onSuccessResult = 1;
        Result<string> result = resultValue;
        int OnSuccess(string _) => foldResult = onSuccessResult;
        int OnFailure(Exception ex) => foldResult = 2;
        result.Fold(OnSuccess, OnFailure);
        foldResult.Should().Be(onSuccessResult);
    }

    [Fact]
    public void FoldReturnOnFailureReturnIfIsFailure()
    {
      
        var foldResult = 0;
        var onFailureResult = 2;
        Result<string> result = new Exception();
        int OnSuccess(string _) => foldResult = 1;
        int OnFailure(Exception ex) => foldResult = onFailureResult;
        result.Fold(OnSuccess, OnFailure);
        foldResult.Should().Be(onFailureResult);
    }

    [Fact]
    public void GetOrDefaultReturnResultValueInsteadOfPassedParameterIfIsSuccess()
    {
        var resultValue = "Test";
        Result<string> result = resultValue;
        var defaultValue = "default";
        result.GetOrDefault(defaultValue).Should().Be(resultValue);
    }

    [Fact]
    public void GetOrDefaultReturnPassedParameterIfIsFailure()
    {
        
        Result<string> result = new Exception();
        var defaultValue = "default";
        result.GetOrDefault(defaultValue).Should().Be(defaultValue);
    }

    [Fact]
    public void MapReturnTransformResultIfResultIsSuccess()
    {

        Result<string> result = "Test";
        var transFormResult = 1;
        int Transform(string resultValue) => transFormResult;
        var mapResult = result.Map(Transform);
        mapResult.GetOrDefault().Should().Be(transFormResult);
    }

    [Fact]
    public void MapReturnExceptionResultIfResultIsFailure()
    {
        
        Result<string> result = new Exception();
        var transFormResult = 1;
        int Transform(string resultValue) => transFormResult;
        var mapResult = result.Map(Transform);
        result.ExceptionOrNull().Should().NotBeNull();
    }
}