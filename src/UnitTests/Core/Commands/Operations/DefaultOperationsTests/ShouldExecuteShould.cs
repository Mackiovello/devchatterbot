using DevChatter.Bot.Core.Commands.Operations;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests.Core.Commands.Operations.DefaultOperationsTests
{
    public class ShouldExecuteShould
    {
        [Fact]
        public void ReturnTrue()
        {
            var operation = new DefaultOperation();

            bool result = operation.ShouldExecute(null);

            result.Should().BeTrue();
        }
    }
}
