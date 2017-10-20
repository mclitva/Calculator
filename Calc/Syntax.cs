using System.Collections.Generic;

namespace Calc
{
    static class Syntax
    {
        private static readonly List<string> Operators = new List<string>
        {
            "*",
            "/",
            "+",
            "-"
        };
        private static readonly List<string> Separators = new List<string> { ",", "." };
        public static readonly List<string> Priorities = new List<string>{"*/", "+-" };
        public static int SignPriority() => Priorities.IndexOf("+-");
        public static bool IsOperator(string ch) => Operators.Contains(ch);
        public static bool IsSeparator(string ch) => Separators.Contains(ch);
    }

}