using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Calc
{
    public class Tokenizer
    {
        private List<Token> _tokens;
        private readonly Expression _expression;
        private char _currentChar;
        private Token _prevToken;
        private Token _currentToken;

        public Tokenizer(string expression)
        {
            _expression = new Expression(expression);
            NextChar();
            _prevToken = new Token(TokenType.Operator, "");
        }
        
        public Token Parse()
        {
            try
            {
                NextToken();
                _tokens = new List<Token>();
                while (_currentToken.Type != TokenType.Eof)
                {
                    _tokens.Add(_currentToken);
                    NextToken();
                }
                if(_tokens.Any(t => t.Type == TokenType.Brace))
                    throw new Exception("Invalid count of braces.");
                var calculator = new Calculator(_tokens);
                calculator.Calculate();
                return calculator.Result;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured while calculating.");
                return new Token(TokenType.Error, e.Message);
            }
        }

        private void NextToken()
        {
            while (char.IsWhiteSpace(_currentChar))
                NextChar();
            var stringBuild = new StringBuilder();
            if (_currentChar == '\0')
            {
                _currentToken = new Token(TokenType.Eof, _currentChar.ToString());
                return;
            }
            if (_currentChar == ')')
            {
                int prevBraceIndex = _tokens.FindLastIndex(t => t.Type == TokenType.Brace);
                var child = _tokens.GetRange(prevBraceIndex+1, _tokens.Count - prevBraceIndex - 1);
                _tokens.RemoveRange(prevBraceIndex, _tokens.Count - prevBraceIndex);
                var tempCalc = new Calculator(child);
                tempCalc.Calculate();
                _currentToken = tempCalc.Result;
                NextChar();
                return;
            }
            if (_currentChar == '(')
            {
                _currentToken = new Token(TokenType.Brace, _currentChar.ToString());
                NextChar();
                return;
            }

            if (Syntax.IsOperator(_currentChar.ToString()))
            {
                while (Syntax.IsOperator(_currentChar.ToString()))
                {
                    stringBuild.Append(_currentChar);
                    NextChar();
                }
                if (!Syntax.Operators.Keys.Contains(stringBuild.ToString()))
                    throw new Exception
                        ($"Unexcepted operator: {stringBuild}");
                if (_currentToken.Type != TokenType.Number)
                    throw new Exception
                        ($"There is no operand here {(string)_currentToken.Value + stringBuild}");
                _currentToken = new Token(TokenType.Operator, stringBuild.ToString());
                return;
            }
            stringBuild = new StringBuilder();
            while (char.IsDigit(_currentChar))
            {
                stringBuild.Append(_currentChar);
                NextChar();
            }
            _currentToken = new Token(TokenType.Number, Int32.Parse(stringBuild.ToString()));
        }

        private void NextChar()
        {
            _currentChar = _expression.CurrentIndex ==_expression.Value.Length?
                '\0' : _expression.Value[_expression.CurrentIndex] ;
            _expression.CurrentIndex++;
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
        public override string ToString() => value.ToString();
    }

    public class Expression
    {
        public string Value { get; set; }
        public int CurrentIndex { get; set; }

        public Expression(string value)
        {
            Value = value;
            CurrentIndex = 0;
        }
    }
    static class Syntax
    {
        public static readonly Dictionary<string, OperatorParams> Operators = new Dictionary<string, OperatorParams>
        {
            {"**", new OperatorParams(0, OperatorOrientation.Right)},
            {"*",  new OperatorParams(1, OperatorOrientation.Left)},
            {"/",  new OperatorParams(1, OperatorOrientation.Left)},
            {"+",  new OperatorParams(2, OperatorOrientation.Left)},
            {"-",  new OperatorParams(2, OperatorOrientation.Left)}
        };
        public static int GetPriority(string op) => Operators[op].Priority;
        public static bool IsOperator(string ch) => Operators.ContainsKey(ch);
        public static List<char> Separators = new List<char> { ',', '.' };
    }

    class OperatorParams
    {
        public int Priority { get; set; }
        public OperatorOrientation Orientation { get; set; }

        public OperatorParams(int priority, OperatorOrientation orient)
        {
            Priority = priority;
            Orientation = orient;
        }
    }
    public enum TokenType
    {
        Number,
        Operator,
        Brace,
        Eof,
        Error
    }

    public enum OperatorOrientation
    {
        Left,
        Right
    }
}