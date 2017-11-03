using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace Calc
{
    public class Calculator
    {
        private List<Token> _elements;
        private int ElementsCount => _elements.Count;
        public Token Result => _elements[0] ?? new Token(TokenType.Error, "Exception");
        public Calculator(List<Token> elements)
        {
            _elements = elements;
        }
        public void Calculate(int startIndex = 0, int endIndex = 0)
        {
            endIndex = endIndex == 0 ? ElementsCount : endIndex;
            int tempIndex = -1;
            for (int i = startIndex; i < endIndex; i++)
            {
                if (_elements[i].Type == TokenType.Operator)
                {
                    if (tempIndex == -1)
                    {
                        tempIndex = i;
                        continue;
                    }
                    if (Syntax.GetPriority((string) _elements[tempIndex].Value) <=
                        Syntax.GetPriority((string) _elements[i].Value))
                    {
                        int start, end;
                        start = end = tempIndex;
                        while (end < endIndex && Syntax.GetPriority((string)_elements[end].Value) ==
                               Syntax.GetPriority((string)_elements[start].Value))
                        {
                            end += 2;
                        }
                        if(IsRightOriented(start))
                            CalculateRight(start,end-2);
                        else
                            CalculateLeft(start,end - 1);
                        i = start - 1;
                        tempIndex = -1;
                    }
                    else
                    {
                        int start, end;
                        start = end = i;
                        while (end < endIndex && Syntax.GetPriority((string) _elements[end].Value) <
                               Syntax.GetPriority((string) _elements[tempIndex].Value))
                        {
                            end += 2;
                        }
                        if(IsRightOriented(end-2))
                            CalculateRight(start - 1, end - 1);
                        else
                            CalculateLeft(start - 1, end - 1);
                        i = tempIndex - 1;
                        tempIndex = -1;
                    }
                    endIndex = ElementsCount;
                }
            }
            if(tempIndex != -1) GetConvolution(tempIndex);
        }


        private void CalculateRight(int startIndex = 0, int endIndex = 0)
        {
            for (int i = endIndex; i >= startIndex; i--)
            {
                if (_elements[i].Type == TokenType.Operator)
                    GetConvolution(i);
                
            }
        }

        private void CalculateLeft(int startIndex = 0, int endIndex = 0)
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                if (_elements[i].Type == TokenType.Operator)
                {
                    GetConvolution(i);
                    endIndex -= 2;
                    i--;
                }
                    
            }
        }

        private void GetConvolution(int iterator)
        {
            _elements[iterator - 1] =
                GetValue((string)_elements[iterator].Value,
                    (int)_elements[iterator - 1].Value,
                    (int)_elements[iterator + 1].Value);
            _elements.RemoveAt(iterator);
            _elements.RemoveAt(iterator);
        }

        private bool IsRightOriented(int index) =>
            Syntax.Operators[(string) _elements[index].Value].Orientation == OperatorOrientation.Right;


        private Token GetValue(string op, int left, int right)
        {
            int res;
            switch (op)
            {
                case "**":
                    res = Power(left, right);
                    break;
                case "+":
                    res = left + right;
                    break;
                case "-":
                    res = left - right;
                    break;
                case "*":
                    res = left * right;
                    break;
                default:
                    res = left / right;
                    break;
            }
            return new Token(TokenType.Number,res);
        }

        private int Power(int value, int power)
        {
            int res = 1;
            while (power != 0)
            {
                if (power % 2 == 0)
                {
                    power /= 2;
                    value *= value;
                }
                else
                {
                    power--;
                    res *= value;
                }
            }
            return res;
        }
    }
}