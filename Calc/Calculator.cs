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
                return res;
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
               
                char ch = expr[i];
                if (Syntax.IsBrace(ch))
                {
                    element = GetElementInBrace(expr.Substring(i));
                    i += element.Length-1;
                    continue;
                }
                if (!Syntax.IsOperator(ch))
                {
                    element += ch;                    
                }
                else
                {
                    if (i == 0 || (Syntax.IsOperator(expr[i - 1]) && GetPriority(ch) == 2))
                    {
                        element += ch;
                        continue;
                    }
                    res.Add(element);
                    res.Add(ch.ToString());
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
                elements = elements.Count == 0 ? GetElements(_expression):elements;
                int priority = 0;
                while (elements.Any(s => s.Contains('(')))
                {
                    string str = elements.First(s => s.Contains('('));
                    int ind = elements.IndexOf(str);
                    try
                    {
                        str = str.Remove(0, 1).Remove(str.Length - 2);
                        elements[ind] = Calculate(GetElements(str));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
                while(priority < Syntax.Priorities.Count())
                {
                    if (elements.All(str => GetPriority(str.First()) != priority))
                    {
                        priority++;
                        continue;
                    }
                    for(int i = 0; i < elements.Count; i++)
                    {
                        if (elements[i].Count() == 1 && Syntax.IsOperator(elements[i].First())) 
                        {
                            char ch = elements[i].First();
                            if (GetPriority(ch) == priority)
                            {
                                try
                                {
                                    elements[i + 1] = GetOperationResult(ch, elements[i - 1], elements[i + 1]);
                                }
                                catch(DivideByZeroException e)
                                {
                                    Console.WriteLine($"Тут вам не JS: {e.Message}");
                                    break;
                                }
                                
                                elements.Remove(elements[i - 1]);
                                elements.Remove(elements[i - 1]);
                                i -= 2;
                            }
                        }
                    }

                    priority++;
                }
                return elements.Count > 1 ? Calculate(elements): elements.First();

            }
            catch (Exception ex)
            {
                Console.Write($"Произошла ошибка при вичислении выражения ({_expression}):\n{ex.Message}");
            }
            return null;
            
        }

        private string GetElementInBrace(string expression)
        {
            int i = 0;
            int countOfBraces = 0;
            var ch = expression[i];
            while (ch != ')')
            {
                i++;
                if(ch=='(')countOfBraces++;
                ch = expression[i];
            }
            while (countOfBraces!=0)
            {
                if (expression[i] == ')') countOfBraces--;
                else if (expression[i] == '(') countOfBraces++;
                i++;
            }

            return expression.Substring(0,i);
        } 

        private string GetOperationResult(char oper, string leftOperand, string rightOperand)
        {
            leftOperand = leftOperand.Replace(',', '.');
            rightOperand = rightOperand.Replace(',', '.');
            float left = float.Parse(leftOperand, CultureInfo.InvariantCulture);
            float right = float.Parse(rightOperand, CultureInfo.InvariantCulture);
            float res;
            switch (oper)
            {
                case '^':
                    res = (float)Math.Pow(left, right);
                    break;
                case '*':
                    res = left * right;
                    break;
                case '/':
                    if (right == 0.0)
                        throw new DivideByZeroException();
                    res = left / right;
                    break;
                case '-':                
                    res = left - right;
                    break;
                default:                 
                    res = left + right;
                    break;
            }
            return res.ToString(CultureInfo.InvariantCulture);
        }

        private int GetPriority(char ch)
        {
            for (int i = 0; i < Syntax.Priorities.Length; i++)
            {
                if (Syntax.Priorities[i].Contains(ch)) return i;
            }
            return 1000;
        }

    }

  
}