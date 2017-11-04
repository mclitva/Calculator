using System;
using System.Collections.Generic;

namespace Calc
{
    public class Parser
    {
        private string _expression;
        private int _currentIndex;
        private char CurChar => _expression[_currentIndex];
        private Token token;
        public int Result { get; set; }

        public Parser(string expr)
        {
            _currentIndex = 0;
            _expression = expr;
            token = new Token(TokenType.Empty, "");
            Result = Calculate();
        }

        public int Calculate()
        {
            int result;
            try
            {
                NextToken();
                if(token.Value == "")
                {
                    throw new Exception("Empty expression");
                }
                GetArithmeticFirst(out result);
                if (token.Value != "")
                    throw new Exception("Syntax error");
                return result;
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return int.MaxValue;
            }
            
        }
        private void GetArithmeticFirst(out int result)
        {
            string op;
            int rightArg;
            GetArithmeticSecond(out result);
            op = token.Value;
            while (op == "+" || op == "-")
            {
                NextToken();
                GetArithmeticSecond(out rightArg);
                switch (op)
                {
                    case "-":
                        result -= rightArg;
                        break;
                    case "+":
                        result += rightArg;
                        break;
                }
                op = token.Value;
            }
        }
        private void GetArithmeticSecond(out int result)
        {
            string op;
            int rightArg = 0;
            GetPower(out result);
            op = token.Value;
            while(op == "*" || op == "/")
            {
                NextToken();
                GetPower(out rightArg);
                switch (op)
                {
                    case "*":
                        result *= rightArg;
                        break;
                    case "/":
                        result /= rightArg;
                        break;
                }
                op = token.Value;
            }
        }
        private void GetPower(out int result)
        {
            int rightArg, power;
            GetInBraces(out result);
            if(token.Value == "**")
            {
                NextToken();
                GetPower(out rightArg);
                power = rightArg;
                if(rightArg == 0)
                {
                    result = 1;
                    return;
                }
                result = Power(result, power);
            }

        }
        private void GetInBraces(out int result)
        {
            if (token.Value == "(")
            {
                NextToken();
                GetArithmeticFirst(out result);
                if (token.Value != ")")
                    throw new Exception("Invalid count of braces");
                NextToken();
            }
            else
                GetNumber(out result);
        }
        private void GetNumber(out int result)
        {
            try
            {
                result = Int32.Parse(token.Value);
            }
            catch (FormatException f)
            {
                result = 0;
                throw new Exception(f.Message);
            }
            NextToken();            
        }
        private int Power(int value,int power)
        {
            int res = 1;
            while (power != 0)
            {
                if (power % 2 == 0)
                {
                    power /= 2;
                    value *= value;
                }
                else
                {
                    power--;
                    res *= value;
                }
            }
            return res;
        }
        private void NextToken()
        {
            token.Type = TokenType.Empty;
            token.Value = "";
            if (_currentIndex == _expression.Length) return;
            while (char.IsWhiteSpace(CurChar) &&
                _currentIndex < _expression.Length)
                _currentIndex++;
            if (_currentIndex == _expression.Length) return;
            if (CurChar == '(' || CurChar == ')')
            {
                token.Type = TokenType.Delimiter;
                token.Value += CurChar;
                _currentIndex++;
            }
            else if (Syntax.IsOperator(CurChar.ToString()))
            {
                token.Value = CurChar.ToString();
                while (Syntax.IsOperator(token.Value))
                {
                    _currentIndex++;
                    token.Value += CurChar;
                }
                token.Type = TokenType.Operator;
                token.Value = token.Value.Substring(0, token.Value.Length - 1);
            }
            else if (Char.IsDigit(CurChar))
            {
                while (Char.IsDigit(CurChar))
                {
                    token.Value += CurChar;
                    _currentIndex++;
                    if (_currentIndex >= _expression.Length) break;
                }
                token.Type = TokenType.Number;
            }
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
        public static List<string> Operators = new List<string> { "**", "*", "/", "+", "-" };
        public static List<char> Delimiters = new List<char> { ',', '.', '(', ')' };
        public static bool IsOperator(string op) => Operators.IndexOf(op) != -1;
        public static bool IsDelimiter(char op) => Delimiters.IndexOf(op) != -1;
        public static bool IsSpecialSymbol(string op) => Operators.IndexOf(op) != -1 || Delimiters.IndexOf(op[0]) != -1;
    }
    public enum TokenType
    {
        Number,
        Operator,
        Delimiter,
        Empty,
        Error
    }    
}