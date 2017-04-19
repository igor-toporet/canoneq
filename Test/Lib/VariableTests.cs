using System;
using CanonEq.Lib;
using FluentAssertions;
using Xunit;

namespace CanonEq.Test.Lib
{
    public class VariableTests
    {
        [Theory]
        [InlineData('1')]
        [InlineData(',')]
        [InlineData(' ')]
        public void ctor_NonLetterName_ThrowsArgumentOutOfRangeException(char name)
        {
            Record.Exception(() => new Variable(name))
                .Should().BeOfType<ArgumentOutOfRangeException>()
                .Which.ParamName.Should().Be("name");
        }

        [Fact]
        public void ctor_WithoutPower_DefaultsPowerToOne()
        {
            new Variable('x').Power.Should().Be(1);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ctor_NonPositivePower_ThrowsArgumentOutOfRangeException(int power)
        {
            Record.Exception(() => new Variable('x', power))
                .Should().BeOfType<ArgumentOutOfRangeException>()
                .Which.ParamName.Should().Be("power");
        }

        [Fact]
        public void Parse_NullInput_ThrowsArgumentNullException()
        {
            Record.Exception(() => Variable.Parse(null))
                .Should().BeOfType<ArgumentNullException>()
                .Which.ParamName.Should().Be("input");
        }

        [Theory]
        [InlineData("")]
        [InlineData("a1")]
        [InlineData("b^")]
        public void Parse_MalformedInput_ThrowsFormatException(string input)
        {
            Record.Exception(() => Variable.Parse(input))
                .Should().BeOfType<FormatException>();
        }

        [Theory]
        [InlineData("a", 'a', 1)]
        [InlineData("b^2", 'b', 2)]
        [InlineData("c^34", 'c', 34)]
        public void Parse_ValidInput_ReturnsVariable(
            string input, char expectedName, int expectedPower)
        {
            var variable = Variable.Parse(input);

            variable.Name.Should().Be(expectedName);
            variable.Power.Should().Be(expectedPower);
        }

        [Theory]
        [InlineData('a', 1, "a")]
        [InlineData('b', 2, "b^2")]
        [InlineData('c', 345, "c^345")]
        public void ToString_ReturnsExpectedRepresentation(
            char name, int power, string expected)
        {
            var variable = new Variable(name, power);

            variable.ToString().Should().Be(expected);
        }
    }
}