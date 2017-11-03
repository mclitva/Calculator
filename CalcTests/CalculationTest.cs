using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calc;

namespace CalcTests
{
    [TestClass]
    public class CalculationTest
    {
        private Dictionary<string, int> SimpleCases = new Dictionary<string, int>
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
        private List<string> ExceptionCases = new List<string>
        {
            "1+((1+5)*0",
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
                Tokenizer tok = new Tokenizer(testCase.Key);
                Assert.AreEqual(tok.Parse().Value, testCase.Value);
            }
        }

        [TestMethod]
        public void TestExceptions()
        {
            foreach (var testCase in ExceptionCases)
            {
                Tokenizer tok = new Tokenizer(testCase);
                Assert.AreEqual(tok.Parse().Type, TokenType.Error);
            }
        }
    }
}
