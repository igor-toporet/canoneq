using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CanonEq.Lib
{
    public class Summand
    {
        public Summand(float factor)
        {
            Factor = factor;
        }

        public Summand(float factor, IEnumerable<Variable> variables)
            : this(factor)
        {
            if (factor == 0) return;

            foreach (var variable in variables)
            {
                Variables.Add(variable);
            }
        }

        public float Factor { get; set; }

        public IList<Variable> Variables { get; } = new List<Variable>();

        /// <summary>
        /// String representation of variables
        /// sorted by power descending and by name ascending.
        /// </summary>
        public string Base
        {
            get
            {
                var variables = Normalize().Variables
                    .OrderByDescending(v => v.Power)
                    .ThenBy(v => v.Name)
                    .Select(v => v.ToString());

                return string.Join(string.Empty, variables);
            }
        }

        public int MaxPower
        {
            get
            {
                return Variables.Any()
                    ? Variables.Max(v => v.Power)
                    : 0;
            }
        }

        public int TotalPower
        {
            get { return Variables.Sum(v => v.Power); }
        }

        public Summand Negate()
        {
            return new Summand(-Factor, Variables);
        }

        /// <summary>
        /// Returns and equivalent summand with distinct variables
        /// by summing up the powers of variables with same names.
        /// </summary>
        public Summand Normalize()
        {
            var distinctVariables = Variables
                .GroupBy(v => v.Name)
                .Select(g => new Variable(g.Key, g.Sum(v => v.Power)));

            return new Summand(Factor, distinctVariables);
        }

        public override string ToString()
        {
            string sign = Math.Sign(Factor) < 0 ? "-" : "";

            float absFactor = Math.Abs(Factor);

            string factor = Variables.Any()
                ? absFactor == 1 ? "" : $"{absFactor}"
                : $"{absFactor}";

            string variables = string.Join(
                string.Empty, Variables.Select(v => v.ToString()));

            return $"{sign}{factor}{variables}";
        }

        public static Summand Parse(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (!SummandPattern.IsMatch(input))
            {
                throw new FormatException("Invalid format.");
            }


            string sFactor = FactorPattern.Match(input).Value;
            sFactor = AddImplicitOneAsNeeded(sFactor);

            var factor = float.Parse(sFactor);
            var variables = VariablePattern.Matches(input)
                .Cast<Match>().Select(m => Variable.Parse(m.Value));

            return new Summand(factor, variables);
        }

        private static string AddImplicitOneAsNeeded(string factor)
        {
            if (factor == string.Empty || factor == "-" || factor == "+")
            {
                return factor + "1";
            }
            return factor;
        }

        private static readonly Regex SummandPattern = new Regex(
            @"^[+-]?\d*\.?\d*([a-zA-Z](\^\d+)*)*$",
            RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex FactorPattern = new Regex(
            @"^[+-]?\d*\.?\d*", RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex VariablePattern = new Regex(
            @"[a-zA-Z](\^\d+)?", RegexOptions.Singleline | RegexOptions.Compiled);
    }
}