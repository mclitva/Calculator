using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calc;

namespace CalcTests
{
    [TestClass]
    public class CalculationTests
    {
        private readonly Dictionary<string, int> _simpleCases = new Dictionary<string, int>
        {

            {"2*(1+1)+0", 4 },
            {"1   +   2   - 152", -149},
            {"1", 1},
            {"2+2", 4},
            {"2*2", 4},
            {"3**2", 9},
            {"2+2*2", 6},
            {"2**2**3", 256},
        };

        [TestMethod]
        public void TestSimpleOperations()
        {
            foreach (var testCase in _simpleCases)
            {
                Calculator calculator = new Calculator(testCase.Key);
                Assert.AreEqual(testCase.Value, calculator.Calculate());
            }
        }
    }

    [TestClass]
    public class ExceptionsTests
    {
        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void DivideByZeroException()
        {
            Calculator calculator = new Calculator("2/0");
            int res = calculator.Calculate();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSyntaxException))]
        public void EmptyExpressionException()
        {
            Calculator calculator = new Calculator("");
            int res = calculator.Calculate();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSyntaxException))]
        public void MissingArgumentException()
        {
            Calculator calculator = new Calculator("1+");
            int res = calculator.Calculate();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSyntaxException))]
        public void UnexceptedPointException()
        {
            Calculator calculator = new Calculator("1.5");
            int res = calculator.Calculate();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSyntaxException))]
        public void UnexceptedSymbolException()
        {
            Calculator calculator = new Calculator("1%");
            int res = calculator.Calculate();
        }
    }
}
