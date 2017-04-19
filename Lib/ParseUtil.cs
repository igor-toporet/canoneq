using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanonEq.Lib
{
    public static class ParseUtil
    {
        public static IEnumerable<string> GetParts(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            ValidateBracketsOrder(input);

            string compact = IgnoreSpaces(input);
            compact = OpenBrackets(compact);

            return GetPartsInternal(compact);
        }

        private static void ValidateBracketsOrder(string input)
        {
            IterateBracketLevelAware(input, (i, c, level) =>
            {
                if (level < 0)
                {
                    throw new FormatException(
                        $"Bracket ')' at zero-based position {i} is missing its counterpart '('.");
                }
            });
        }

        private delegate void BracketLevelAwareIterationCallback(
            int index, char characterAtIndex, int bracketLevel);

        private static void IterateBracketLevelAware(
            string input, BracketLevelAwareIterationCallback callback)
        {
            int n = 0;
            for (var i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (c == '(')
                {
                    n++;
                }
                else if (c == ')')
                {
                    n--;
                }

                callback(i, c, n);
            }
        }

        private static string OpenBrackets(string compact)
        {
            string current = compact;

            do
            {
                int start = current.IndexOf("(", StringComparison.Ordinal);

                if (start < 0)
                {
                    return current;
                }

                int end = FindClosingBracketOrThrow(current, start);

                current = OpenOuterBrackets(current, start, end);
                current = FixConsecutiveSigns(current);

            } while (true);
        }

        private static string FixConsecutiveSigns(string current)
        {
            return current.Replace("+-", "-").Replace("-+", "-").Replace("++", "+");
        }

        private static string OpenOuterBrackets(string compact, int start, int end)
        {
            string left = compact.Substring(0, start);
            string middle = compact.Substring(start + 1, end - start - 1);
            string right = compact.Substring(end + 1);

            if (middle.Length > 0)
            {
                bool shouldNegateTopLevelSigns = start > 0 && compact[start - 1] == '-';

                if (shouldNegateTopLevelSigns)
                {
                    middle = NegateTopLevelSigns(middle);

                    if (StartsWithSign(middle))
                    {
                        left = RemoveTrailingSign(left);
                    }
                }
            }

            return left + middle + right;
        }

        private static string NegateTopLevelSigns(string compactFragment)
        {
            var result = new StringBuilder();

            Func<char, char> negateSign = c => c == '-' ? '+' : c == '+' ? '-' : c;

            IterateBracketLevelAware(compactFragment, (i, c, level) =>
            {
                result.Append(level == 0 ? negateSign(c) : c);
            });

            return result.ToString();
        }

        private static bool StartsWithSign(string compact)
        {
            return compact[0] == '-' || compact[0] == '+';
        }

        private static string RemoveTrailingSign(string compact)
        {
            return compact.TrimEnd('-', '+');
        }

        private static int FindClosingBracketOrThrow(string compact, int start)
        {
            var openIndicesStack = new Stack<int>();
            int i = start;

            do
            {
                if (compact[i] == '(')
                {
                    openIndicesStack.Push(i);
                }
                else if (compact[i] == ')')
                {
                    openIndicesStack.Pop();
                }

                i++;
            } while (openIndicesStack.Count > 0 && i < compact.Length);

            if (openIndicesStack.Count == 0)
            {
                return i - 1;
            }

            // TODO: position in original input is different than in compact, oops
            throw new FormatException($"Bracket '(' at zero-based position {start} is missing its counterpart ')'.");
        }

        private static IEnumerable<string> GetPartsInternal(string compact)
        {
            char[] operators = {'+', '-'};

            do
            {
                string part;
                int operatorIndex = compact.IndexOfAny(operators);

                if (operatorIndex < 0)
                {
                    part = compact;
                }
                else if (operatorIndex == 0)
                {
                    const int start = 1;
                    int nextOperatorIndex = compact.IndexOfAny(operators, start);

                    part = nextOperatorIndex > start
                        ? compact.Substring(0, nextOperatorIndex)
                        : compact;
                }
                else
                {
                    part = compact.Substring(0, operatorIndex);
                }

                yield return part;
                compact = compact.Remove(0, part.Length);
            } while (compact.Length > 0);
        }

        private static string IgnoreSpaces(string input)
        {
            return new string(input.Where(c => c != ' ').ToArray());
        }
    }
}