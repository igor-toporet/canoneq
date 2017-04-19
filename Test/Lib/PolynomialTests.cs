using System;
using CanonEq.Lib;
using FluentAssertions;
using Xunit;

namespace CanonEq.Test.Lib
{
    public class PolynomialTests
    {
        [Fact]
        public void Parse_null_ThrowsArgumentNullException()
        {
            Record.Exception(() => Polynomial.Parse(null))
                .Should().BeOfType<ArgumentNullException>()
                .Which.ParamName.Should().Be("input");
        }

        [Theory]
        [InlineData("\t")]
        [InlineData("#")]
        [InlineData("!")]
        [InlineData(",")]
        [InlineData("=")]
        public void Parse_InvalidChars_ThrowsArgumentException(string input)
        {
            var exception = Record.Exception(() => Polynomial.Parse(input));

            var argumentException = Assert.IsType<ArgumentException>(exception);

            argumentException.Message.Should().Contain("invalid characters");
            argumentException.ParamName.Should().Be("input");
        }

        [Fact]
        public void ctor_NullSummands_ThrowsArgumentNullException()
        {
            Record.Exception(() => new Polynomial(null))
                .Should().BeOfType<ArgumentNullException>()
                .Which.ParamName.Should().Be("summands");
        }

        [Theory]
        [InlineData("1+2", "3")]
        [InlineData("x-x", "0")]
        [InlineData("x+y-y", "x")]
        [InlineData("7+x+2xy+y^3+x^2-yx-y-2x^2+y^3", "2y^3-x^2+xy+x-y+7")]
        public void Combine_ProducesCorrectCombinedPolynomial(
            string input, string expected)
        {
            var polynomial = Polynomial.Parse(input);

            var combined = polynomial.Combine();

            combined.ToString().Should().Be(expected);
        }

        [Theory]
        [InlineData("1", "2", "-1")]
        [InlineData("x^2+ 3.5xy + y", "y^2 - xy + y", "x^2-y^2+4.5xy")]
        public void Subtract_ProducesCorrectCombinedPolynomial(
            string minuend, string subtrahend, string difference)
        {
            var minuendPolynomial = Polynomial.Parse(minuend);
            var subtrahendPolynomial = Polynomial.Parse(subtrahend);

            var differencePolynomial = minuendPolynomial - subtrahendPolynomial;

            differencePolynomial.ToString().Should().Be(difference);
        }
    }
}