using System;
using FluentAssertions;
using Xunit;

namespace SimpleResult.Tests
{
    public class ResultShould
    {
        [Fact]
        public void BeInFailureWhenSetToException()
        {
            var resultValue = new Exception("Test");

            Result<string> result = resultValue;

            var isFailure = result.IsFailure;

            isFailure.Should().BeTrue();


        }

        [Fact]
        public void NotBeInFailureWhenSetToException()
        {
            var resultValue = "Test";

            Result<string> result = resultValue;

            var isFailure = result.IsFailure;

            isFailure.Should().BeFalse();


        }
    }
}