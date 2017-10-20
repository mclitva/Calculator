using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Calc
{
    class Calculator
    {
        private string _expression;
        public Calculator(string expression)
        {
            _expression = expression;
        }

        public string Run()
        {
            try
            {
                var res = Calculate(new List<string>());
                return string.IsNullOrEmpty(res) ? "Exception" : res ;
            }catch(Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return "Exception";
            }
        }

        private List<string> GetElements(string expr)
        {
            List<string> res = new List<string>();
            string element = string.Empty;
            for(int i = 0; i < expr.Length; i++)
            {
                string ch = expr[i].ToString();
                if (!Syntax.IsOperator(ch))
                {
                    element += ch;                    
                }
                else
                {
                    if (i == 0 || (Syntax.IsOperator(expr[i - 1].ToString()) && GetPriority(ch) == Syntax.SignPriority()))
                    {
                        element += ch;
                        continue;
                    }
                    res.Add(element);
                    res.Add(ch);
                    element = string.Empty;
                }
            }
            res.Add(element);
            return res;            
        }
        
        private string Calculate(List<string> elements)
        {
            try
            {
                elements = elements.Count == 0 ? GetElements(_expression) : elements;
                int priority = 0;
                while(priority < Syntax.Priorities.Count())
                {
                    for(int i = 0; i < elements.Count; i++)
                    {
                        if (Syntax.IsOperator(elements[i])) 
                        {
                            string ch = elements[i];
                            if (GetPriority(ch) == priority)
                            {
                                try
                                {
                                    List<string> newElements = new List<string>();
                                    var element = GetOperationResult(ch, elements[i - 1], elements[i + 1]);
                                    newElements.AddRange(elements.GetRange(0,i - 1));
                                    newElements.Add(element);
                                    newElements.AddRange(elements.GetRange(newElements.Count + 2, elements.Count - newElements.Count - 2));
                                    elements = newElements;
                                    i--;
                                }
                                catch(DivideByZeroException e)
                                {
                                    Console.WriteLine($"An error occured, the reason is: {e.Message}");
                                    return string.Empty;
                                }
                            }
                        }
                    }
                    priority++;
                }
                return elements.Count > 1 ? Calculate(elements): elements.First();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while calculate: ({_expression})");
                Console.WriteLine($"Error :{ex.Message}");
            }
            return string.Empty;

        }
        
        private string GetOperationResult(string oper, string leftOperand, string rightOperand)
        {
            float left = float.Parse(leftOperand);
            float right = float.Parse(rightOperand);
            float res;
            switch (oper)
            {
                case "^":
                    res = (float)Math.Pow(left, right);
                    break;
                case "*":
                    res = left * right;
                    break;
                case "/":
                    if (right == 0.0)
                        throw new DivideByZeroException();
                    res = left / right;
                    break;
                case "-":                
                    res = left - right;
                    break;
                default:                 
                    res = left + right;
                    break;
            }
            return res.ToString();
        }

        private int GetPriority(string ch)
        {
            for (int i = 0; i < Syntax.Priorities.Count; i++)
            {
                if (Syntax.Priorities[i].Contains(ch)) return i;
            }
            return int.MaxValue;
        }

    }
}