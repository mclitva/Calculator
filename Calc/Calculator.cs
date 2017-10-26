using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Calc
{
    class Calculator
    {
        private readonly string _expression;
        public Calculator(string expression)
        {
            _expression = expression;
        }
        public string Calculate()
        {
            try
            {
                var t = new Tokenizer(new StringReader(_expression));
                List<Token> elements = new List<Token>();
                while (t.Token.Type != TokenType.Eof)
                {
                    elements.Add(t.Token);
                    t.NextToken();
                }
                if (elements.Count == 1)
                    return elements[0].Number.ToString();
               var maxPriority = GetMaxPriority(elements.Where(tok => tok.Type == TokenType.Operator).ToList());
               var res = CalculateWithPriority(maxPriority, elements);
               return res.Number.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while calculate: '{_expression}'");
                Console.WriteLine($"Error : {ex.Message}");
                return "Exception";
            }
        }

        private Token CalculateWithPriority(int priority, List<Token> tokens)
        {
            if (tokens.Count == 1) return tokens[0];
            var newElements = new List<Token>();
            for (int i = 0; i < tokens.Count; i++)
            {
                if (!(tokens[i].Type == TokenType.Operator && Syntax.Operators[tokens[i].Value] == priority))
                {
                   newElements.Add(tokens[i]);
                   continue;
                }
                newElements[newElements.Count - 1] = GetValue(tokens[i].Value, newElements[newElements.Count - 1].Number, tokens[i + 1].Number);
                i++;
            }
            tokens = newElements;
            var res = CalculateWithPriority(++priority, tokens);
            return res;
        }
        
        private Token GetValue(string op, double left, double right)
        {
            double res;
            switch (op)
            {
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
                    if (right == 0)
                    {
                        throw new DivideByZeroException();
                    }
                    res = left / right;
                    break;
            }
            return new Token(TokenType.Number,res);
        }

        private int GetMaxPriority(List<Token> operators)
        {
            int res = Syntax.Operators[operators[0].Value];
            for (int i = 1; i < operators.Count; i++)
            {
                if (Syntax.Operators[operators[i].Value] < res) res = Syntax.Operators[operators[i].Value];
            }
            return res;
        }
    }
}