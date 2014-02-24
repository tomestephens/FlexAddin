using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using NUnit.Framework;

namespace FlexAddin.NUnit
{
    internal class TestCaseNamer
    {
        public static string Create(TestCaseData testCase, MethodInfo method, object[] arguments)
        {
            string argsName = testCase.TestName;
            var methodParams = method.GetParameters();

            // a little weird but basically we're making an allowance for the idea that the test case source did some naming already
            if(testCase.Arguments.Length < arguments.Length &&
               string.IsNullOrEmpty(argsName))
            {
                for (int i = 0; i < testCase.Arguments.Length; i++)
                {
                    argsName = AddParam(argsName, methodParams[i].Name, testCase.Arguments[i]);
                }
            }

            for (int i = 0; i < arguments.Length; i++)
            {
                argsName = AddParam(argsName, methodParams[i + testCase.Arguments.Length].Name, arguments[i]);
            }

            return string.Format("{0}({1})", method.Name, argsName);
        }

        public static string AddParam(string currentName, string paramName, object value)
        {
            return string.Format("{0}{1}[{2}={3}]", currentName,
                                                    string.IsNullOrEmpty(currentName) ? "" : "-",
                                                    paramName,
                                                    value);
        }
    }
}
