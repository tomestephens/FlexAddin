using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using FlexAddin.NUnit;

namespace FlexAddin.NUnit.Examples
{
    /// <summary>
    /// Just an example of how to use the Flex NUnit addin
    /// Several of the test cases fail, but they aren't intended to pass - just intended to be an example of how it works.
    /// </summary>
    [TestFixture]
    public class FlexTestCaseExample
    {
        IEnumerable<TestCaseData> ExampleTestData()
        {
            yield return new TestCaseData(new Calculator() { Name = "Test1" }).SetName("TestCalculator 1");
            yield return new TestCaseData(new Calculator() { Name = "Test2" }).SetName("TestCalculator 2");
            yield return new TestCaseData(new Calculator() { Name = "Test3" }).SetName("TestCalculator 3");
        }

        IEnumerable<TestCaseData> ExampleTestData2()
        {
            yield return new TestCaseData(new Calculator() { Name = "Test1" });
            yield return new TestCaseData(new Calculator() { Name = "Test2" });
            yield return new TestCaseData(new Calculator() { Name = "Test3" });
        }


        [FlexTestCase("ExampleTestData", 1, 1, 2)]
        public void ExampleTestA(Calculator calc, int valA, int valB, int expected)
        {
            Assert.AreEqual(calc.Add(valA, valB), expected);
        }

        [FlexTestCase("ExampleTestData2", 1, 1, 2)]
        public void ExampleTestA2(Calculator calc, int valA, int valB, int expected)
        {
            Assert.AreEqual(calc.Add(valA, valB), expected);
        }

        [FlexTestCase("ExampleTestData", new[] { 1, 2 }, new[] { 1, 2 }, typeof(CalcAction))]
        public void ExampleTestB(Calculator calc, int valA, int valB, CalcAction action)
        {
            switch (action)
            {
                case CalcAction.Add:
                    Assert.AreEqual(calc.Add(valA, valB), valA + valB);
                    break;
                case CalcAction.Divide:
                    Assert.AreEqual(calc.Add(valA, valB), valA / valB);
                    break;
                case CalcAction.Multiply:
                    Assert.AreEqual(calc.Add(valA, valB), valA * valB);
                    break;
                case CalcAction.Subtract:
                    Assert.AreEqual(calc.Add(valA, valB), valA - valB);
                    break;
            }
        }

        [FlexTestCase(1, 1, typeof(CalcAction))]
        public void ExampleTestC(int valA, int valB, CalcAction action)
        {
            var calc = new Calculator();

            switch (action)
            {
                case CalcAction.Add:
                    Assert.AreEqual(calc.Add(valA, valB), valA + valB);
                    break;
                case CalcAction.Divide:
                    Assert.AreEqual(calc.Add(valA, valB), valA / valB);
                    break;
                case CalcAction.Multiply:
                    Assert.AreEqual(calc.Add(valA, valB), valA * valB);
                    break;
                case CalcAction.Subtract:
                    Assert.AreEqual(calc.Add(valA, valB), valA - valB);
                    break;
            }
        }

        [FlexTestCase(new[] { 1, 2 }, new[] { 1, 2 }, typeof(CalcAction))]
        public void ExampleTestD(int valA, int valB, CalcAction action)
        {
            var calc = new Calculator();

            switch (action)
            {
                case CalcAction.Add:
                    Assert.AreEqual(calc.Add(valA, valB), valA + valB);
                    break;
                case CalcAction.Divide:
                    Assert.AreEqual(calc.Add(valA, valB), valA / valB);
                    break;
                case CalcAction.Multiply:
                    Assert.AreEqual(calc.Add(valA, valB), valA * valB);
                    break;
                case CalcAction.Subtract:
                    Assert.AreEqual(calc.Add(valA, valB), valA - valB);
                    break;
            }
        }

        [FlexTestCase(typeof(CalcAction), NoPermutation = true)]
        public void ExampleTestE(Type typeToTest)
        {
            Assert.True(typeToTest.IsEnum);
        }
    }
}
