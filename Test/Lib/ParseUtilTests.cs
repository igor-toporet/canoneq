using System;
using System.Linq;
using CanonEq.Lib;
using FluentAssertions;
using Xunit;

namespace CanonEq.Test.Lib
{
    public class ParseUtilTests
    {
        [Fact]
        public void GetParts_null_ThrowsArgumentNullException()
        {
            Record.Exception(() => ParseUtil.GetParts(null))
                .Should().BeOfType<ArgumentNullException>()
                .Which.ParamName.Should().Be("input");
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" 2.3 x y z ", "2.3xyz")]
        [InlineData(" + 1.23 x ^ 4 ", "+1.23x^4")]
        [InlineData(" - 1 . 2 a b c ", "-1.2abc")]
        public void GetParts_SingleSummand_SinglePartWithoutSpaces(string input, string expected)
        {
            ParseUtil.GetParts(input)
                .Single()
                .Should().Be(expected);
        }

        [Theory]
        [InlineData("1.2x + 3.4yz", "1.2x", "+3.4yz")]
        [InlineData("-2x - 3y - 4z ", "-2x", "-3y", "-4z")]
        public void GetParts_MultipleSummands_AllWithoutSpaces(string input, params string[] expected)
        {
            ParseUtil.GetParts(input)
                .ShouldBeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("1)+2", 1)]
        [InlineData("1-(2-3)-4)", 9)]
        public void GetParts_SummandsWithMalformedBrackets_ThrowsFormatException(
            string input, int posOfExtraClosingBracket)
        {
            string expectedMessage =
                $"Bracket ')' at zero-based position {posOfExtraClosingBracket} is missing its counterpart '('.";

            Record.Exception(() =>
                    ParseUtil.GetParts(input))
                .Should().BeOfType<FormatException>()
                .Which.Message.Should().Be(expectedMessage);
        }

        [Theory]
        [InlineData("(1+2", 0)]
        [InlineData("1-(2-(3-4)+5", 2)]
        public void GetParts_SummandsMissingClosingBracket_ThrowsFormatException(
            string input, int posOfUnclosedBracket)
        {
            string expectedMessage =
                $"Bracket '(' at zero-based position {posOfUnclosedBracket} is missing its counterpart ')'.";

            Record.Exception(() =>
                    ParseUtil.GetParts(input))
                .Should().BeOfType<FormatException>()
                .Which.Message.Should().Be(expectedMessage);
        }

        [Theory]
        [InlineData("(1+2)", "1", "+2")]
        [InlineData("1+(2-3)", "1", "+2", "-3")]
        [InlineData("a-(bc-de)", "a", "-bc", "+de")]
        [InlineData("2-(1-(-2+3)-4)-5", "2", "-1", "-2", "+3", "+4", "-5")]
        [InlineData("3+(+4-(+5))+6", "3", "+4", "-5", "+6")]
        [InlineData("4-(-5-(6))+(-(-7)-8)", "4", "+5", "+6", "+7", "-8")]
        [InlineData("5ab-(-6cd+7ef)-(+8gh-(9ij))", "5ab", "+6cd", "-7ef", "-8gh", "+9ij")]
        public void GetParts_SummandsWithBrackets_OpensBrackets(string input, params string[] expected)
        {
            var parts = ParseUtil.GetParts(input).ToArray();

            parts.ShouldBeEquivalentTo(expected);
        }
    }
}