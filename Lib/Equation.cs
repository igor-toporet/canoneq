using System;

namespace CanonEq.Lib
{
    public class Equation
    {
        public Equation(Polynomial left, Polynomial right)
        {
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));
        }

        public Polynomial Right { get; }

        public Polynomial Left { get; }

        public static Equation Parse(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var parts = input.Split('=');

            var leftPart = parts[0];
            var rightPart = parts[1];

            var left = Polynomial.Parse(leftPart);
            var right = Polynomial.Parse(rightPart);

            return new Equation(left, right);
        }

        public static bool TryParse(string input, out Equation equation)
        {
            try
            {
                equation = Parse(input);
                return true;
            }
            catch (Exception)
            {
                equation = null;
                return false;
            }
        }

        public Equation ToCanonicalForm()
        {
            var leftMinusRight = Left - Right;
            var zero = new Polynomial();

            return new Equation(leftMinusRight, zero);
        }

        public override string ToString()
        {
            string sparseLeft = SparseSigns(Left.ToString());
            string sparseRight = SparseSigns(Right.ToString());

            return $"{sparseLeft} = {sparseRight}";
        }

        private static string SparseSigns(string polynomial)
        {
            return polynomial
                .Replace("-", " - ")
                .Replace("+", " + ")
                .TrimStart();
        }
    }
}