using System;

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
            return BracesCheck() && SyntaxCheck();            
        }

        private bool BracesCheck()
        {
            bool isValid = true;
            Stack stack = new Stack(Expression.Length);
            for (int i = 0; i < Expression.Length; i++)
            {
                char ch = Expression[i];
                switch (ch)
                {
                    case '(':
                        {
                            stack.Push(ch);
                            break;
                        }
                    case ')':
                        {
                            isValid = stack.Pop() == '(' ? true : false;
                            break;
                        }
                }
            }
            return isValid;
        }
        private bool SyntaxCheck()
        {            
            bool isValid = false;
            char ch = Expression[Expression.Length - 1];                       
            for(int i = 0; i < Expression.Length; i++)
            {              
                ch = Expression[i];
                isValid = Syntax.IsBrace(ch) || Syntax.IsOperator(ch) || Syntax.IsSeparator(ch) || Char.IsNumber(ch);
                if (!isValid) return false;
            }            
            
            return isValid;
        }         
    }
        class Stack
        {
            public char[] StackArray { get; set; }
            private int index;            
            public Stack(int expressionLength)
            {
                StackArray = new char[expressionLength];
                index = -1;
            }
            public void Push(char ch)
            {
                StackArray[++index] = ch;
            }
            public char Pop()
            {
                if (index != -1)
                {
                    return StackArray[index--];
                }
                else return '0';
            }
        }

}
