using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calc
{
    public class Tokenizer
    {
        private string _input;
        private int _position;
        private char CurrentChar => _input.ElementAtOrDefault(_position);
        private static readonly List<char> OperatorChars = new List<char> { '*', '/', '+', '-', '^' };

        public Tokenizer(string input)
        {
            _input = input;
            _position = 0;
        }

        public List<Token> Tokenize()
        {
            List<Token> tokens = new List<Token>();
            Token token;
            while(_position < _input.Length)
            {
                token = NextToken();
                if (token.Type != TokenType.Error)
                    tokens.Add(token);
            }
            return tokens;
        }

        public Token NextToken()
        {
            if (_position == _input.Length)
                return new Token(TokenType.Empty, CurrentChar.ToString());
            while (char.IsWhiteSpace(CurrentChar) && _position < _input.Length)
                _position++;
            if (_position == _input.Length)
                return new Token(TokenType.Empty, CurrentChar.ToString());
            if (CurrentChar == '(' || CurrentChar == ')')
            {
                char brace = CurrentChar;
                _position++;
                return new Token(TokenType.Brace, brace.ToString());
            }
            else if (IsOperator(CurrentChar))
            {
                return TokenizeOperator();
            }
            else if (char.IsDigit(CurrentChar))
            {
                return TokenizeNumber();
            }
            else
            {
                _position++;
                return new Token(TokenType.Error, string.Empty);
            }
        }
        
        public Token TokenizeNumber()
        {
            StringBuilder builder = new StringBuilder();
            while (char.IsDigit(CurrentChar))
            {
                builder.Append(CurrentChar);
                _position++;
            }
            return new Token(TokenType.Number, builder.ToString());
        }

        public Token TokenizeOperator()
        {
            StringBuilder builder = new StringBuilder();
            while (IsOperator(CurrentChar))
            {
                builder.Append(CurrentChar);
                _position++;
            }
            return new Token(TokenType.Operator, builder.ToString());
        }

        public static bool IsOperator(char op) => OperatorChars.IndexOf(op) != -1;
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