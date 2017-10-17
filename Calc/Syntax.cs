using System.Collections.Generic;

namespace Calc
{
     static class Syntax
    {
        public static readonly List<char> Operators = new List<char>
        {
            '^',
            '*',
            '/',
            '+',
            '-'
        };
        public static readonly List<char> Braces = new List<char> { '(', ')' };
        public static readonly List<char> Separators = new List<char> { ',', '.' };
        public static string[] Priorities = { "^", "*/", "+-" };
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
