using System;
using System.Collections.Generic;
using System.Text;

namespace Calc
{
    public class Tokenizer
    {
        private string _expression;
        private int _position;
        private char CurChar => _position >= _expression.Length ? '\0' : _expression[_position];
        private static readonly List<char> Operators = new List<char> { '*', '/', '+', '-', '^' };

        public Tokenizer(string expression)
        {
            _expression = expression;
            _position = 0;
        }

        public List<Token> Parse()
        {
            List<Token> tokens = new List<Token>();
            Token token = new Token(TokenType.Number, "");
            while(_position < _expression.Length)
            {
                token = NextToken();
                if (token.Type == TokenType.Error)
                    continue;
                tokens.Add(token);
            }
            return tokens;
        }

        public Token NextToken()
        {
            string tokenValue = string.Empty;
            if (_position == _expression.Length)
                return new Token(TokenType.Empty, tokenValue);
            while (char.IsWhiteSpace(CurChar) && _position < _expression.Length)
                _position++;
            if (_position == _expression.Length)
                return new Token(TokenType.Empty, tokenValue);
            if (CurChar == '(' || CurChar == ')')
            {
                tokenValue += CurChar;
                _position++;
                return new Token(TokenType.Brace, tokenValue);
            }
            else if (IsOperator(CurChar))
            {
                Token op = ParseOperator();
                return op;
            }
            else if (char.IsDigit(CurChar))
            {
                Token number = ParseNumber();
                return number;
            }
            else
            {
                _position++;
                return new Token(TokenType.Error, tokenValue);
            }
        }
        
        public Token ParseNumber()
        {
            StringBuilder builder = new StringBuilder();
            while (char.IsDigit(CurChar))
            {
                if (_position >= _expression.Length)
                    break;
                builder.Append(CurChar);
                _position++;
            }
            return new Token(TokenType.Number, builder.ToString());
        }

        public Token ParseOperator()
        {
            string operatorValue = string.Empty;
            while (IsOperator(CurChar))
            {
                operatorValue += CurChar;
                _position++;
            }
            return new Token(TokenType.Operator, operatorValue);
        }

        private char GetCharAt(int index)
        {
            if (index >= _expression.Length)
                return '\0';
            return _expression[index];
        }

        public static bool IsOperator(char op) => Operators.IndexOf(op) != -1;
    }

    public class Token
    {
        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public override string ToString() => Value.ToString();
    }  

    public enum TokenType
    {
        Number,
        Operator,
        Brace,
        Empty,
        Error
    }

}