using System;
using System.Collections.Generic;
using System.IO;

namespace Calc
{
    public class Calculator
    {
        private readonly string _expression;
        public Calculator(string expression)
        {
            _expression = expression.Replace(" ", "");
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
                    return elements[0].Value.ToString();
                foreach(var op in Syntax.Operators)
                {
                    CalculateWithPriority(op.Value, ref elements);
                }
                return elements[0].Value.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while calculate: '{_expression}'");
                Console.WriteLine($"Error : {ex.Message}");
                return "Exception";
            }
        }

        private void CalculateWithPriority(int priority, ref List<Token> tokens)
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Type == TokenType.Operator &&
                    Syntax.Operators[(string) tokens[i].Value] == priority)
                {
                    tokens[i - 1] =
                        GetValue((string)tokens[i].Value,
                            Convert.ToInt32(tokens[i - 1].Value),
                            Convert.ToInt32(tokens[i + 1].Value));
                    tokens.RemoveAt(i);
                    tokens.RemoveAt(i);
                    i--;
                }
            }
        }
        
        private Token GetValue(string op, int left, int right)
        {
            double res;
            switch (op)
            {
                case "**":
                    res = Math.Pow(left, right);
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
    }
}