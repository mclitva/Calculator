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
                if (elements.Count == 1) return elements[0].Value;
                var maxPriority = GetMaxPriority(elements.Where(tok => tok.Type == TokenType.Operator).ToList());
                var res = CalculateWithPriority(maxPriority, elements);
                return res.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while calculate: ({_expression})");
                Console.WriteLine($"Error :{ex.Message}");
            }
            return string.Empty;
        }

        private Token CalculateWithPriority(int priority, List<Token> tokens)
        {
            if (tokens.Count == 1) return tokens[0];
            var newElements = new List<Token>();
            for (int i = 0; i < tokens.Count; i++)
            {
                if (!(tokens[i].Type == TokenType.Operator && Syntax.Priorities[tokens[i].Value] == priority))
                {
                   newElements.Add(tokens[i]);
                   continue;
                }
                newElements[newElements.Count - 1] = GetValue(tokens[i].Value,tokens[i-1].Value,tokens[i+1].Value);
                i++;
            }
            tokens = newElements;
            var res = CalculateWithPriority(++priority, tokens);
            return res;
        }
        
        private Token GetValue(string op, string left, string right)
        {
            var operation = Syntax.Operations[op];
            var value = operation(double.Parse(left), double.Parse(right));
            return new Token(TokenType.Number,value.ToString());
        }

        private int GetMaxPriority(List<Token> operators)
        {
            int res = Syntax.Priorities[operators[0].Value];
            for (int i = 1; i < operators.Count(); i++)
            {
                if (Syntax.Priorities[operators[i].Value] < res) res = Syntax.Priorities[operators[i].Value];
            }
            return res;
        }

    }

   
}