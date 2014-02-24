using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlexAddin.NUnit.Examples
{
    public class Calculator
    {
        public string Name { get; set; }

        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Subtract(int a, int b)
        {
            return a - b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }

        public int Divide(int a, int b)
        {
            return a / b;
        }

        public override string ToString()
        {
            return "Calculator - " + Name;
        }
    }

    public enum CalcAction
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }
}
