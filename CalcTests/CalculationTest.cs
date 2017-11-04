using System.Collections.Generic;
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

        [TestMethod]
        public void TestSimpleOperations()
        {
            foreach (var testCase in SimpleCases)
            {
                Parser parser = new Parser(testCase.Key);
                Assert.AreEqual(testCase.Value, parser.Result);
            }
        }        
    }
}
