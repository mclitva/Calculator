using System.Collections.Generic;
using System.Linq;

namespace Calc
{
    public class Calculator
    {
        private List<Token> _tokens;
        private int _currentIndex;
        private Token currentToken => _tokens.ElementAtOrDefault(_currentIndex) ?? new Token(TokenType.EOF, string.Empty);

        public Calculator(string input)
        {
            _tokens = new Tokenizer(input).Tokenize();
            _currentIndex = 0;
        }

        public int Calculate()
        {
            int result = 0;
            if (currentToken.Value == "")
            {
                throw new InvalidSyntaxException("Empty expression");
            }
            result = CalculateAddition(result);
            if (currentToken.Value != "")
            {
                throw new InvalidSyntaxException("Syntax error");
            }
            return result;
        }

        private int CalculateAddition( int result)
        {
            int rightArg = 0;
            result = CalculateMultiplication(result);
            var op = currentToken.Value;
            while (op == "+" || op == "-")
            {
                _currentIndex++;
                rightArg = CalculateMultiplication(rightArg);
                switch (op)
                {
                    case "-":
                        result -= rightArg;
                        break;
                    case "+":
                        result += rightArg;
                        break;
                }
                op = currentToken.Value;
            }
            return result;
        }

        private int CalculateMultiplication(int result)
        {
            int rightArg = 0;
            result = GetPower(result);
            string op = currentToken.Value;
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
                op = currentToken.Value;
            }
            return result;
        }

        private int GetPower(int result)
        {
            int rightArg = 0;
            result = GetInBraces(result);
            if (currentToken.Value == "^^")
            {
                _currentIndex++;
                rightArg = GetPower(rightArg);
                var power = rightArg;                
                result = CalculatePower(result, power);
            }
            return result;
        }

        private int GetInBraces(int result)
        {
            if (currentToken.Value == "(")
            {
                _currentIndex++;
                result = CalculateAddition(result);
                if (currentToken.Value != ")")
                    throw new InvalidSyntaxException("Invalid count of braces");
                _currentIndex++;
            }
            else
                result = GetNumber(result);
            return result;
        }

        private int GetNumber(int result)
        {
            if(string.IsNullOrEmpty(currentToken.Value))
                throw new InvalidSyntaxException("There is no argument");
            result = int.Parse(currentToken.Value);            
            _currentIndex++;
            return result;
        }

        private int CalculatePower(int value, int power)
        {            
            int res = 1;
            if (power == 0)
                {
                    return res;
                }
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
