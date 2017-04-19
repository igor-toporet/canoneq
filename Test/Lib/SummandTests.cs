using System;
using CanonEq.Lib;
using FluentAssertions;
using Xunit;

namespace CanonEq.Test.Lib
{
    public class SummandTests
    {
        [Fact]
        public void Parse_NullInput_ThrowsArgumentNullException()
        {
            Record.Exception(() => Summand.Parse(null))
                .Should().BeOfType<ArgumentNullException>()
                .Which.ParamName.Should().Be("input");
        }

        [Theory]
        [InlineData("++a")]
        [InlineData("1.1a^b")]
        public void Parse_InvalidInput_ThrowsFormatException(string input)
        {
            Record.Exception(() => Summand.Parse(input))
                .Should().BeOfType<FormatException>();
        }

        [Theory]
        [InlineData("1.0", 1, 0)]
        [InlineData("2.3abc", 2.3, 3)]
        [InlineData("34.5a^1b", 34.5, 2)]
        [InlineData("-4xy^7", -4, 2)]
        [InlineData("-0.1xyz", -0.1, 3)]
        [InlineData("-whoa", -1, 4)]
        [InlineData("+whoa", 1, 4)]
        [InlineData("whoa", 1, 4)]
        public void Summand_ValidInput_ReturnsSummand(
            string input, float factor, int expectedVariablesCount)
        {
            var summand = Summand.Parse(input);

            summand.Factor.Should().Be(factor);
            summand.Variables.Count.Should().Be(expectedVariablesCount);
        }

        [Fact]
        public void ctor_ZeroFactor_CreatesSummandWithoutVariables()
        {
            var var1 = new Variable('x', 2);
            var var2 = new Variable('y', 3);

            var summand = new Summand(0, new[] {var1, var2});

            summand.Factor.Should().Be(0);
            summand.Variables.Should().BeEmpty();
        }

        [Theory]
        [InlineData("1.0", "1")]
        [InlineData("2.0", "2")]
        [InlineData("-1.0", "-1")]
        [InlineData("-2.0", "-2")]
        [InlineData("2.3abc", "2.3abc")]
        [InlineData("34.5a^1b", "34.5ab")]
        [InlineData("-4xy^7", "-4xy^7")]
        [InlineData("-0.1xyz", "-0.1xyz")]
        [InlineData("-whoa", "-whoa")]
        [InlineData("+whoa", "whoa")]
        [InlineData("whoa", "whoa")]
        [InlineData("0", "0")]
        [InlineData("-0a", "0")]
        public void ToString_ReturnsExpectedRepresentation(string input, string expected)
        {
            var summand = Summand.Parse(input);

            summand.ToString().Should().Be(expected);
        }

        [Theory]
        [InlineData("7890", "7890")]
        [InlineData("aaa", "a^3")]
        [InlineData("-1.2xyzx^1y^2z^3", "-1.2x^2y^3z^4")]
        public void Normalize_ReturnsSummandWithDistinctVariableNamesAndCorrectPowers(
            string input, string expected)
        {
            var summand = Summand.Parse(input);

            var normalized = summand.Normalize();

            normalized.ToString().Should().Be(expected);
        }

        [Theory]
        [InlineData("123", "")]
        [InlineData("yxoyxayx", "x^3y^3ao")]
        public void Base_ReturnsVariablesSortedByPowerDescendingAndNameAscending(
            string input, string expected)
        {
            var summand = Summand.Parse(input);

            summand.Base.Should().Be(expected);
        }
    }
}