using System;
using System.Collections.Generic;
using System.Text;

namespace Calc
{
    public class Tokenizer
    {
        private readonly string _expression;
        private int _position;
        private char CurChar => _position >= _expression.Length ? '\0' : _expression[_position];
        public Token Token;

        public Tokenizer(string expr)
        {
            _position = 0;
            _expression = expr;
            Token = new Token(TokenType.Empty, string.Empty);
        }
        public void NextToken()
        {
            Token.Type = TokenType.Empty;
            Token.Value = "";
            if (_position == _expression.Length)
                return;
            while (char.IsWhiteSpace(CurChar) &&
                   _position < _expression.Length)
                _position++;
            if (_position == _expression.Length)
                return;
            if (CurChar == '(' || CurChar == ')')
            {
                Token.Value += CurChar;
                _position++;
            }
            else if (Syntax.IsOperator(CurChar))
            {
                if (CurChar == '*' && GetCharAt(_position + 1) == CurChar)
                {
                    Token.Value = "**";
                    _position += 2;
                }
                else
                {
                    Token.Value = CurChar.ToString();
                    _position++;
                }
                Token.Type = TokenType.Operator;
            }
            else if (char.IsDigit(CurChar))
            {
                StringBuilder builder = new StringBuilder();
                while (char.IsDigit(CurChar))
                {
                    if (_position >= _expression.Length)
                        break;
                    builder.Append(CurChar);
                    _position++;
                }
                Token.Value = builder.ToString();
                Token.Type = TokenType.Number;
            }
            else
            {
                throw new InvalidSyntaxException("Unexpected symbol");
            }
        }

        private char GetCharAt(int index)
        {
            if (index >= _expression.Length)
                return '\0';
            return _expression[index];
        }
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

    internal static class Syntax
    {
        private static readonly List<char> Operators = new List<char> { '*', '/', '+', '-' }; 
        public static bool IsOperator(char op) => Operators.IndexOf(op) != -1;
    }
    public enum TokenType
    {
        Number,
        Operator,
        Empty
    }

}