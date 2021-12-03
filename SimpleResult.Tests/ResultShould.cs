using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace SimpleResult.Tests
{
    public class ResultShould
    {
        [Fact]
        public void InFailureEqualsTrueAndIsSuccessEqualsFalseWhenSetToException()
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
        public void InFailureEqualsFalseAndIsSuccessEqualsTrueWhenSetValue()
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
        public void ExceptionOrNullReturnNullWhenResultSetToAnyThingsElseException()
        {
            var resultValue = "Test";

            Result<string> result = resultValue;



            result.ExceptionOrNull().Should().BeNull();


        }
    }
}