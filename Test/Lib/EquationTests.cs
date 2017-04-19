using System;
using CanonEq.Lib;
using FluentAssertions;
using Xunit;

namespace CanonEq.Test.Lib
{
    public class EquationTests
    {
        [Fact]
        public void Parse_null_ThrowsArgumentNullException()
        {
            Record.Exception(() => Equation.Parse(null))
                .Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void ctor_WithNullLeft_ThrowsArgumentNullException()
        {
            Polynomial right = new Polynomial();

            Record.Exception(() => new Equation(null, right))
                .Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void ctor_WithNullRight_ThrowsArgumentNullException()
        {
            Polynomial left = new Polynomial();

            Record.Exception(() => new Equation(left, null))
                .Should().BeOfType<ArgumentNullException>();
        }

        [Theory]
        [InlineData("abc", false)]
        [InlineData("xyz=42", true)]
        public void TryParse_VariousInputs_SucceedsOrFailsAsExpected(
            string input, bool expected)
        {
            Equation.TryParse(input, out Equation equation)
                .Should().Be(expected);
        }

        //
        // Integration tests (sort of)
        //

        [Theory]
        [InlineData("x^2 + 3.5xy + y = y^2 - xy + y", "x^2+3.5xy+y", "y^2-xy+y")]
        [InlineData("-1 + 2.3ab^2c^3=-(-(5-6))", "-1+2.3ab^2c^3", "5-6")]
        public void Parse_ValidInput_CorrectLeftAndRightPolynomials(
            string input, string left, string right)
        {
            var equation = Equation.Parse(input);

            equation.Left.ToString().Should().Be(left);
            equation.Right.ToString().Should().Be(right);
        }

        [Theory]
        [InlineData("x^2 + 3.5xy + y = y^2 - xy + y", "x^2 - y^2 + 4.5xy = 0")]
        [InlineData("x = 1", "x - 1 = 0")]
        [InlineData("x - (y^2 - x) = 0", "2x - y^2 = 0",
            Skip = "The expected order of summands is debatable")]
        [InlineData("x - (y^2 - x) = 0", "- y^2 + 2x = 0")]
        [InlineData("x - (0 - (0 - x)) = 0", "0 = 0")]
        public void ToCanonicalForm_SubtractsRightFromLeftAndLeavesZeroOnTheRight(
            string input, string expected)
        {
            var equation = Equation.Parse(input);

            var canonicalForm = equation.ToCanonicalForm();
            canonicalForm.ToString().Should().Be(expected);
        }
    }
}