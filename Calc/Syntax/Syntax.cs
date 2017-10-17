using System.Collections.Generic;

namespace Calc.Syntax
{
    class Syntax
    {
        public readonly List<char> Operators = new List<char>
        {
            '*',
            '/',
            '+',
            '-'
        };
        public bool IsOperator(char ch)
        {
            return Operators.Contains(ch);
        }
    }
}
