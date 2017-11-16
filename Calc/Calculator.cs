using System.Collections.Generic;
using System.Linq;

namespace Calc
{
    public class Calculator
    {
        private List<Token> _tokens;
        private int _currentIndex;
        private Token CurrentToken => _tokens.ElementAtOrDefault(_currentIndex) ?? new Token(TokenType.Empty, string.Empty);

        public Calculator(string input)
        {
            _tokens = new Tokenizer(input).Tokenize();
            _currentIndex = 0;
        }

        public int Calculate()
        {
            int result = 0;
            if (CurrentToken.Value == "")
            {
                throw new InvalidSyntaxException("Empty expression");
            }
            result = CalcAddition(result);
            if (CurrentToken.Value != "")
            {
                throw new InvalidSyntaxException("Syntax error");
            }
            return result;
        }

        private int CalcAddition( int result)
        {
            int rightArg = 0;
            result = CalcMultiplication(result);
            var op = CurrentToken.Value;
            while (op == "+" || op == "-")
            {
                _currentIndex++;
                rightArg = CalcMultiplication(rightArg);
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

        private int CalcMultiplication(int result)
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
                result = CalcAddition(result);
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
