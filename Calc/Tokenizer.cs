using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Calc
{
    class Tokenizer
    {
        private readonly TextReader _reader;
        private char _currentChar;
        private Token _currentToken;
        public Tokenizer(TextReader reader)
        {
            _reader = reader;
            _currentToken = new Token(TokenType.Operator, "");
            NextChar();
            NextToken();
        }
        public Token Token => _currentToken;
        public void NextChar()
        {
            int ch = _reader.Read();
            _currentChar = ch < 0 ? '\0' : (char)ch;
        }
        public void NextToken()
        {
            if (_currentChar == '\0')
            {
                _currentToken = new Token(TokenType.Eof, _currentChar.ToString());
                return;
            }
            if(Syntax.IsOperator(_currentChar.ToString()))
            {
                var temp = _currentChar;
                NextChar();
                if (_currentChar == temp && temp == '*')
                {
                    _currentToken = new Token(TokenType.Operator, "**");
                    NextChar();
                    return;
                }
                if(_currentToken.Type == TokenType.Operator)
                    throw new Exception
                        ($"Invalid operator format: {(string)_currentToken.Value + temp}");
                if(_currentToken.Type != TokenType.Number)
                    throw new Exception
                        ($"There is no operand here {(string)_currentToken.Value + temp}");
                _currentToken = new Token(TokenType.Operator,temp.ToString());
                return;
            }
            var stringBuild = new StringBuilder();
            stringBuild.Append(_currentChar);
            NextChar();
            while (char.IsDigit(_currentChar))
            {
                stringBuild.Append(_currentChar);
                NextChar();
            }
            try
            {
                _currentToken = new Token(TokenType.Number, Int32.Parse(stringBuild.ToString()));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
           
        }
    }

    public class Token
    {
        private TokenType type;
        private object value;
        public Token(TokenType type, object value)
        {
            this.type = type;
            this.value = value;
        }
        public TokenType Type => type;
        public object Value => value;
        public string ToString()
        {
            return value.ToString();
        }
        public Token Clone()
        {
            return new Token(type, value);
        }
    }

    static class Syntax
    {
        public static readonly Dictionary<string, int> Operators = new Dictionary<string, int>
        {
            {"**", 0},
            {"*", 1},
            {"/", 1},
            {"+", 2},
            {"-", 2}
        };
        public static int GetMinPriority() => Operators.Values.Max();
        public static bool IsOperator(string ch) => Operators.ContainsKey(ch);
        public static List<char> Separators = new List<char> { ',', '.' };
    }

    public enum TokenType
    {
        Number,
        Operator,
        Eof
    }
}