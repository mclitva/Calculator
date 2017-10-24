using System;
using System.Linq;

namespace Calc
{
    class Validator
    {
        private string Expression { get; set; }
        public Validator(string expression)
        {
            Expression = expression;
        }
        public bool IsValid()
        {
            return SyntaxCheck();
        }
        private bool SyntaxCheck()
        {
            if (Syntax.IsOperator(Expression[Expression.Length - 1].ToString()) || Syntax.IsOperator(Expression[0].ToString())) return false;
            if (!Expression.Any(char.IsDigit)) return false;
            bool isValid = false;
            for (int i = 0; i < Expression.Length; i++)
            {
                var ch = Expression[i].ToString();
                isValid = Syntax.IsOperator(ch) || ch[0] == '.' || Char.IsNumber(ch[0]);
                if (!isValid) return false;
            }
            return isValid;
        }
    }
}