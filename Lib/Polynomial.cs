using System;
using System.Collections.Generic;
using System.Linq;

namespace CanonEq.Lib
{
    public class Polynomial
    {
        public Polynomial()
        {
        }

        public Polynomial(IEnumerable<Summand> summands)
        {
            if (summands == null)
            {
                throw new ArgumentNullException(nameof(summands));
            }
            foreach (var summand in summands)
            {
                Summands.Add(summand);
            }
        }

        public IList<Summand> Summands { get; } = new List<Summand>();

        public Polynomial Combine()
        {
            var combinedSummands = Summands
                .Select(s => s.Normalize())
                .OrderByDescending(s => s.MaxPower)
                .ThenByDescending(s => s.TotalPower)
                .GroupBy(s => s.Base)
                .Select(g => new Summand(
                    g.Sum(s => s.Factor),
                    g.First().Variables))
                .Where(s => s.Factor != 0);

            return new Polynomial(combinedSummands);
        }

        public Polynomial Negate()
        {
            return new Polynomial(Summands.Select(s => s.Negate()));
        }

        public Polynomial Subtract(Polynomial subtrahend)
        {
            var differenceSummands = Summands.Concat((-subtrahend).Summands);

            return new Polynomial(differenceSummands).Combine();
        }

        public static Polynomial operator -(Polynomial minuend, Polynomial subtrahend)
        {
            return minuend.Subtract(subtrahend);
        }

        public static Polynomial operator -(Polynomial polynomial)
        {
            return polynomial.Negate();
        }

        public static Polynomial Parse(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            EnsureValidCharacters(input);

            var parts = ParseUtil.GetParts(input);
            var summands = parts.Select(Summand.Parse);

            return new Polynomial(summands);
        }

        public override string ToString()
        {
            var nonZeroSummands = Summands.Where(s => s.Factor != 0).ToList();

            if (nonZeroSummands.Count == 0) return "0";

            var summands = nonZeroSummands.Select(
                (summand, index) =>
                    index == 0
                        ? summand.ToString() /* as is, plus won't show, minus will */
                        : Math.Sign(summand.Factor) > 0
                            ? "+" + summand.ToString() /* append plus */
                            : summand.ToString()) /* the minus will appear anyways */;

            return string.Join(string.Empty, summands);
        }

        private static void EnsureValidCharacters(string input)
        {
            if (ContainsOnlyAllowedCharacters(input)) return;

            const string message =
                "String contains invalid characters (other than digits, letters, space, '(', ')', '.', '+', '-' and '^')";

            throw new ArgumentException(message, nameof(input));
        }

        private static bool ContainsOnlyAllowedCharacters(string input)
        {
            var allowedCharacters = " .+-()^".ToCharArray();

            Func<char, bool> isValidChar = c => char.IsLetterOrDigit(c) || allowedCharacters.Contains(c);

            return input.All(isValidChar);
        }
    }
}