using System;
using System.Collections.Generic;
using Calc;
using NUnit.Framework;

namespace CalcTests
{
    [TestFixture]
    public class CalculationTests
    {        
        [TestCase(4,"2+2")]
        [TestCase(4, "2*2")]
        [TestCase(9, "3^^2")]
        [TestCase(0, "0/1")]
        public void TestSimpleOperation(int excepted, string expression)
        {
            Assert.AreEqual(excepted, new Calculator(expression).Calculate());
        }

        [Test]
        public void TestOperationsPriority()
        {
            Assert.AreEqual(6, new Calculator("2+2*2").Calculate());
        }

        [Test]
        public void TestRightOrientedOperator()
        {
            Assert.AreEqual(256, new Calculator("2^^2^^3").Calculate());
        }
    }

    [TestFixture]
    public class CalculatingExceptionsTests
    {
        [Test]
        public void DivideByZeroException()
        {
            Calculator calculator = new Calculator("2/0");
        }

        [Test]
        public void EmptyExpressionException()
        {            
            Calculator calculator = new Calculator("");
            Assert.Throws<InvalidSyntaxException>(delegate { calculator.Calculate(); });
        }

        [Test]
        public void MissingArgumentException()
        {
            Calculator calculator = new Calculator("1+");
            Assert.Throws<InvalidSyntaxException>(delegate { calculator.Calculate(); });
        }

        [Test]
        public void UnexceptedPointException()
        {
            Assert.Throws<InvalidSyntaxException>(delegate { new Calculator("1.5").Calculate(); });
        }
    }    
}
