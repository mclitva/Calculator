using System;
using System.Collections.Generic;
using System.Linq;

namespace Calc.Managers
{
    class Calculator
    {
        private readonly char[] _expression;
        private readonly Syntax.Syntax syntax;
        public Calculator(string expression)
        {
            syntax = new Syntax.Syntax();
            _expression = expression.ToCharArray();
        }

        public string Run()
        {
            List<char[]> elements = GetElements();
            var res = Calculate(elements).First();
            return new string(res);
        }

        private List<char[]> GetElements()
        {
            var res = new List<char[]>();
            var sntx = new Syntax.Syntax();
            var element = string.Empty;
            foreach (char ch in _expression)
            {
                if (!sntx.IsOperator(ch)) element += ch;
                else
                {
                    res.Add(element.ToCharArray());
                    res.Add(new []{ ch });
                    element=string.Empty;
                }
            }
            res.Add(element.ToCharArray());
            return res;
        }

        private List<char[]> Calculate(List<char[]> elements)
        {
            if (elements.Any(elem => syntax.IsOperator(elem.First()) && GetPriority(elem.First()) <= 1))
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    var opr = elements[i].First();
                    if (syntax.IsOperator(opr) && GetPriority(opr) <= 1)
                    {
                        elements[i + 1] =
                            GetOperationResult(opr, new string(elements[i - 1]), new string(elements[i + 1]))
                                .ToCharArray();
                        elements[i] = null;
                        elements[i - 1] = null;
                    }
                }
                elements.RemoveAll(arr => arr == null);
                Calculate(elements);
            }
            else
            {
                elements.RemoveAll(arr => arr == null);
                for (int i = 0; i < elements.Count; i++)
                {
                    var opr = elements[i].First();
                    if (syntax.IsOperator(opr))
                    {
                        
                        elements[i + 1] =
                            GetOperationResult(opr, new string(elements[i - 1]), new string(elements[i + 1]))
                                .ToCharArray();
                        elements[i] = null;
                        elements[i - 1] = null;
                    }
                }
                elements.RemoveAll(arr => arr == null);
            }
            return elements;
        }

        private string GetOperationResult(char oper, string leftOperand, string rightOperand)
        {
            float.TryParse(leftOperand, out float left);
            float.TryParse(rightOperand, out float right);
            float res;
            switch (oper)
            {
                case '*':
                    res = (left * right); break;
                case '/':
                    if (right == 0)
                    {
                        Console.WriteLine("Делить на ноль нельзя");
                    }
                    res = (left / right); break;
                case '-':                
                    res = (left - right); break;
                default:                 
                    res = (left + right); break;
            }
            return res.ToString();
        }

        private int GetPriority(char ch)
        {
            return syntax.Operators.IndexOf(ch);
        }

    }

  
}
