using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Calc
{
    static class Syntax
    {
        public static readonly Dictionary<string, int> Operators = new Dictionary<string, int>
        {
            {"*",0},
            {"/",0},
            {"+",1},
            {"-",1}
        };
        public static bool IsOperator(string ch) => Operators.ContainsKey(ch);
    }

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
            while (char.IsWhiteSpace(_currentChar))
            {
                NextChar();
            }
            if (_currentChar == '\0')
            {
                _currentToken = new Token(TokenType.Eof, _currentChar.ToString());
                return;
            }
            if(Syntax.IsOperator(_currentChar.ToString()))
            {
                if(_currentToken.Type == TokenType.Operator)
                    throw new Exception($"Invalid operator format: {_currentToken.Value + _currentChar}");
                if(_currentToken.Type != TokenType.Number)
                    throw new Exception($"Еhere is no operand here {_currentToken.Value + _currentChar}");
                _currentToken = new Token(TokenType.Operator,_currentChar.ToString());
                NextChar();
                return;
            }
            var stringBuild = new StringBuilder();
            stringBuild.Append(_currentChar);
            NextChar();
            while (!Syntax.IsOperator(_currentChar.ToString()) && _currentChar != '\0')
            {
                stringBuild.Append(_currentChar);
                NextChar();
            }
            try
            {
                _currentToken = new Token(TokenType.Number, double.Parse(stringBuild.ToString()));
            }
            catch (Exception e)
            {
                throw new Exception($"'{stringBuild}' is invalid format for operand");
            }
           
        }
    }

    public class Token
    {
        private TokenType type;
        private string value;
        private double number;
        public Token(TokenType type, string value)
        {
            this.type = type;
            this.value = value;
            number = 0;
        }
        public Token(TokenType type, double number)
        {
            this.type = type;
            this.number = number;
            value = string.Empty;
        }
        public TokenType Type => type;
        public string Value => value;
        public double Number => number;
    }

    public enum TokenType
    {
        Number,
        Operator,
        Eof
    }
}