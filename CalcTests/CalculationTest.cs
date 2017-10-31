using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calc;

namespace CalcTests
{
    [TestClass]
    public class CalculationTest
    {
        private Dictionary<string, string> SimpleCases = new Dictionary<string, string>
        {
            {"1", "1"},
            {"2+2", "4"},
            {"2*2", "4"},
            {"3**2", "9"},
            {"2+2*2", "6"},
            {"1   +   2   - 152", "-149"},
        };
        private List<string> ExceptionCases = new List<string>
        {
            "2,0",
            "1/0",
            "*",
            "1-",
            "1+++0",
            "1,,0",
            "  ",
            string.Empty
        }; 

        [TestMethod]
        public void TestSimpleOperations()
        {
            foreach (var testCase in SimpleCases)
            {
                Calculator calc = new Calculator(testCase.Key);
                Assert.AreEqual(calc.Calculate(), testCase.Value);
            }
        }

        [TestMethod]
        public void TestExceptions()
        {
            foreach (var testCase in ExceptionCases)
            {
                Calculator calc = new Calculator(testCase);
                Assert.AreEqual(calc.Calculate(), "Exception");
            }
        }
    }
}
