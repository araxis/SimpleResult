using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace SimpleResult.Tests
{
    public class ResultTests
    {

        [Fact]
        public void IsFailureEqualsTrueAndIsSuccessEqualsFalseWhenCreateResultByFailMethod()
        {
            var exception = new Exception("Test");

            Result<string> result =Result<string>.Fail(exception);

            using (new AssertionScope())
            {
                result.IsFailure.Should().BeTrue();
                result.IsSuccess.Should().BeFalse();
            }


        }

        [Fact]
        public void IsFailureEqualsFalseAndIsSuccessEqualsTrueWhenCreateResultBySuccessMethod()
        {
            var resultValue = "Test";

            Result<string> result = Result<string>.Success(resultValue);


            using (new AssertionScope())
            {
                result.IsFailure.Should().BeFalse();
                result.IsSuccess.Should().BeTrue();
            }
            


        }

 
        [Fact]
        public void IsFailureEqualsTrueAndIsSuccessEqualsFalseWhenSetToExceptionDirectly()
        {
            var resultValue = new Exception("Test");

            Result<string> result = resultValue;

            using (new AssertionScope())
            {
                result.IsFailure.Should().BeTrue();
                result.IsSuccess.Should().BeFalse();
            }


        }

        [Fact]
        public void IsFailureEqualsFalseAndIsSuccessEqualsTrueWhenSetValueDirectly()
        {
            var resultValue = "Test";

            Result<string> result = resultValue;


            using (new AssertionScope())
            {
                result.IsFailure.Should().BeFalse();
                result.IsSuccess.Should().BeTrue();
            }
            


        }

        [Fact]
        public void GetOrDefaultReturnDefaultWhenResultSetToException()
        {
            var resultValue = new Exception("Test");

            Result<int> result = resultValue;

            var value = result.GetOrDefault();

            value.Should().Be(default);


        }

        [Fact]
        public void GetOrDefaultReturnValueWhenResultSetToAnyThingsElseException()
        {
            var resultValue = "Test";

            Result<string> result = resultValue;

            var value = result.GetOrDefault();

            value.Should().Be(resultValue);


        }

        [Fact]
        public void ExceptionOrNullReturnExceptionWhenResultSetToException()
        {
            var resultValue = new Exception("Test");

            Result<string> result = resultValue;
            result.ExceptionOrNull().Should().Be(resultValue);


        }

        [Fact]
        public void ExceptionOrNullReturnNullWhenResultSetToAnyThingsElseException()
        {
            var resultValue = "Test";

            Result<string> result = resultValue;
            result.ExceptionOrNull().Should().BeNull();

        }

        [Fact]
        public void ExceptionOrNullReturnNullWhenResultIsError()
        {
            var result = Result<string>.Error(new ErrorInfo("", "", ""));

            result.ExceptionOrNull().Should().BeNull();
        }

        [Fact]
        public void ErrorsIsEmptyWhenResultIsSucceed()
        {
            var result = Result<string>.Success("ok");

            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ErrorsIsEmptyWhenResultIsFailed()
        {
            var result = Result<string>.Fail(new Exception());

            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ErrorsIsNotEmptyWhenResultIsError()
        {
            var result = Result<string>.Error(new ErrorInfo("", "", ""));

            result.Errors.Should().HaveCount(1);
        }
    }
}