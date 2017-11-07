using System;

namespace Calc
{
    public class Calculator
    {
        private Tokenizer _tokenizer;

        public Calculator(string expr)
        {
            _tokenizer = new Tokenizer(expr);
        }

        public int Calculate()
        {
            int result = 0;
            _tokenizer.NextToken();
            if (_tokenizer.Token.Value == "")
            {
                throw new InvalidSyntaxException("Empty expression");
            }
            result = GetAddition(result);
            if (_tokenizer.Token.Value != "")
            {
                throw new InvalidSyntaxException("Syntax error");
            }
            return result;
        }

        private int GetAddition( int result)
        {
            int rightArg = 0;
            result = GetMultiplication(result);
            var op = _tokenizer.Token.Value;
            while (op == "+" || op == "-")
            {
                _tokenizer.NextToken();
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
                op = _tokenizer.Token.Value;
            }
            return result;
        }

        private int GetMultiplication(int result)
        {
            int rightArg = 0;
            result = GetPower(result);
            string op = _tokenizer.Token.Value;
            while (op == "*" || op == "/")
            {
                _tokenizer.NextToken();
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
                op = _tokenizer.Token.Value;
            }
            return result;
        }

        private int GetPower(int result)
        {
            int rightArg = 0;
            result = GetInBraces(result);
            if (_tokenizer.Token.Value == "**")
            {
                _tokenizer.NextToken();
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
            if (_tokenizer.Token.Value == "(")
            {
                _tokenizer.NextToken();
                result = GetAddition(result);
                if (_tokenizer.Token.Value != ")")
                    throw new InvalidSyntaxException("Invalid count of braces");
                _tokenizer.NextToken();
            }
            else
                result = GetNumber(result);
            return result;
        }

        private int GetNumber(int result)
        {
            if(string.IsNullOrEmpty(_tokenizer.Token.Value))
                throw new InvalidSyntaxException("There is no argument");
            if (!Int32.TryParse(_tokenizer.Token.Value, out result))
            {
                throw new FormatException();
            }
            _tokenizer.NextToken();
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
