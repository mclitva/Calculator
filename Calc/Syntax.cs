using System.Collections.Generic;

namespace Calc
{
    static class Syntax
    {
        private static readonly List<char> Operators = new List<char>
        {
            '^',
            '*',
            '/',
            '+',
            '-'
        };
        private static readonly List<char> Braces = new List<char> { '(', ')' };
        private static readonly List<char> Separators = new List<char> { ',', '.' };
        public static readonly List<string> Priorities = new List<string>{ "^", "*/", "+-" };

        public static int SignPriority() => Priorities.IndexOf("+-");

        public static bool IsOperator(char ch)
        {
            return Operators.Contains(ch);
        }
        public static bool IsBrace(char ch)
        {
            return Braces.Contains(ch);
        }
        public static bool IsSeparator(char ch)
        {
            return Separators.Contains(ch);
        }
    }

}