using System;
using System.Collections.Generic;

namespace Calc
{
    public class Calculator
    {
        private List<Token> _tokens;
        private int _currentIndex;
        private Token CurrentToken { get
            {
                if (_currentIndex < _tokens.Count)
                    return _tokens[_currentIndex];
                else
                    return new Token(TokenType.Empty, "");
            }}

        public Calculator(string expr)
        {
            _tokens = new Tokenizer(expr).Parse();
            _currentIndex = 0;
        }

        public int Calculate()
        {
            int result = 0;
            if (CurrentToken.Value == "")
            {
                throw new InvalidSyntaxException("Empty expression");
            }
            result = GetAddition(result);
            if (CurrentToken.Value != "")
            {
                throw new InvalidSyntaxException("Syntax error");
            }
            return result;
        }

        private int GetAddition( int result)
        {
            int rightArg = 0;
            result = GetMultiplication(result);
            var op = CurrentToken.Value;
            while (op == "+" || op == "-")
            {
                _currentIndex++;
                rightArg = GetMultiplication(rightArg);
                switch (op)
                {
                    case "-":
                        result -= rightArg;
                        break;
                    case "+":
                        result += rightArg;
                        break;
                }
                op = CurrentToken.Value;
            }
            return result;
        }

        private int GetMultiplication(int result)
        {
            int rightArg = 0;
            result = GetPower(result);
            string op = CurrentToken.Value;
            while (op == "*" || op == "/")
            {
                _currentIndex++;
                rightArg = GetPower(rightArg);
                switch (op)
                {
                    case "*":
                        result *= rightArg;
                        break;
                    case "/":
                        result /= rightArg;
                        break;
                }
                op = CurrentToken.Value;
            }
            return result;
        }

        private int GetPower(int result)
        {
            int rightArg = 0;
            result = GetInBraces(result);
            if (CurrentToken.Value == "^^")
            {
                _currentIndex++;
                rightArg = GetPower(rightArg);
                var power = rightArg;
                if (rightArg == 0)
                {
                    result = 1;
                    return result;
                }
                result = CalculatePower(result, power);
            }
            return result;
        }

        private int GetInBraces(int result)
        {
            if (CurrentToken.Value == "(")
            {
                _currentIndex++;
                result = GetAddition(result);
                if (CurrentToken.Value != ")")
                    throw new InvalidSyntaxException("Invalid count of braces");
                _currentIndex++;
            }
            else
                result = GetNumber(result);
            return result;
        }

        private int GetNumber(int result)
        {
            if(string.IsNullOrEmpty(CurrentToken.Value))
                throw new InvalidSyntaxException("There is no argument");
            result = int.Parse(CurrentToken.Value);            
            _currentIndex++;
            return result;
        }

        private int CalculatePower(int value, int power)
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
    }
}
