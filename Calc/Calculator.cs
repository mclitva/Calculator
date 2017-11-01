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
                CalculatePowers(ref elements);
                for(int pr = 1; pr <= Syntax.GetMaxPriority(); pr++)
                {
                    CalculateWithPriority(pr, ref elements);
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
                            (int)tokens[i - 1].Value,
                            (int)tokens[i + 1].Value);
                    tokens.RemoveAt(i);
                    tokens.RemoveAt(i);
                    i--;
                }
            }
        }

        private void CalculatePowers(ref List<Token> tokens)
        {
            for (int i = tokens.Count - 1; i >= 0 ; i--)
            {
                if (tokens[i].Type == TokenType.Operator && (string)tokens[i].Value == "**")
                {
                    tokens[i - 1] =
                        GetValue((string)tokens[i].Value,
                            (int)tokens[i - 1].Value,
                            (int)tokens[i + 1].Value);
                    tokens.RemoveAt(i);
                    tokens.RemoveAt(i);
                    i++;
                }
            }
        }
        
        private Token GetValue(string op, int left, int right)
        {
            int res;
            switch (op)
            {
                case "**":
                    res = (int)Math.Pow(left, right);
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