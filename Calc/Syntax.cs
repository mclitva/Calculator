using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Calc
{
    static class Syntax
    {
        public delegate double OperationDelegate(double num1, double num2);
        public static readonly Dictionary<string, OperationDelegate> Operations = new Dictionary<string, OperationDelegate>
        {
            {"*", Operator.Multiply},
            {"/", Operator.Devide},
            {"+", Operator.Add},
            {"-", Operator.Subtrackt}
        };
        public static readonly Dictionary<string, int> Priorities = new Dictionary<string, int>
        {
            {"*",0},
            {"/",0},
            {"+",1},
            {"-",1}
        };
        public static bool IsOperator(string ch) => Operations.ContainsKey(ch);
    }
 
    public static class Operator
    {
        public static double Add(double a, double b) => a + b;
        public static double Subtrackt(double a, double b) => a - b;
        public static double Multiply(double a, double b) => a * b;
        public static double Devide(double a, double b)
        {
            try
            {
                var res = a / b;
                if (double.IsPositiveInfinity(res)) throw new DivideByZeroException();
                return res;
            }catch(DivideByZeroException e)
            {
                Console.WriteLine($"{e.Message}");
                return 0;
            }
            
        } 
    }

    class Tokenizer
    {
        private readonly TextReader _reader;
        private char _currentChar;
        private Token _currentToken;

        public Tokenizer(TextReader reader)
        {
            _reader = reader;
            _currentToken = new Token(TokenType.Root, "");
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
            if(Syntax.IsOperator(_currentChar.ToString()) && _currentToken.Type != TokenType.Operator && _currentToken.Type == TokenType.Number)
            {
                _currentToken = new Token(TokenType.Operator,_currentChar.ToString());
                NextChar();
                return;
            }
            var stringBuild = new StringBuilder();
            bool haveDelimiter = false;
            stringBuild.Append(_currentChar);
            NextChar();
            while (char.IsDigit(_currentChar) || (!haveDelimiter && _currentChar == '.'))
            {
                stringBuild.Append(_currentChar);
                haveDelimiter = _currentChar == '.';
                NextChar();
            }
            _currentToken = new Token(TokenType.Number, stringBuild.ToString());
            
        }

    }

    public class Token
    {
        private TokenType type;
        private string value;

        public Token(TokenType type, string value)
        {
            this.type = type;
            this.value = value;
        }
        public TokenType Type => type;
        public string Value => value;
    }
    public enum TokenType
    {
        Root,
        Number,
        Operator,
        Eof
    }


}