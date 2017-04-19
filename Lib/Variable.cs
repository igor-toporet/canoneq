using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CanonEq.Lib
{
    public class Variable
    {
        public Variable(char name, int power = 1)
        {
            if (!Char.IsLetter(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name));
            }

            if (power <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(power));
            }

            Name = name;
            Power = power;
        }

        public char Name { get; }

        public int Power { get; }

        public override string ToString()
        {
            return Power == 1 ? Name.ToString() : $"{Name}^{Power}";
        }

        public static Variable Parse(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (!VariablePattern.IsMatch(input))
            {
                throw new FormatException("Invalid format.");
            }

            var parts = input.Split(new[] {'^'}, StringSplitOptions.RemoveEmptyEntries);

            char name = parts[0].Single();

            if (parts.Length == 2)
            {
                int power = int.Parse(parts[1]);
                return new Variable(name, power);
            }

            return new Variable(name);
        }

        private static readonly Regex VariablePattern = new Regex(
            @"^[a-zA-Z](\^\d+)?$", RegexOptions.Singleline | RegexOptions.Compiled);
    }
}