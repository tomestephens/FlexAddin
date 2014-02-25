using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Core;
using NUnit.Framework;

namespace FlexAddin.NUnit
{
    internal class TestFixtureActivator
    {
        private static readonly object sync = new object();
        private static readonly Dictionary<string, object[]> parametersCache = new Dictionary<string, object[]>();

        public static object GetTestFixture(Test parent)
        {
            return Activator.CreateInstance(parent.FixtureType, FindParameters(parent));
        }

        private static object[] FindParameters(Test parent)
        {
            string key = BuildCacheKey(parent);

            if (!parametersCache.ContainsKey(key))
            {
                lock (sync)
                {
                    if (!parametersCache.ContainsKey(key))
                    {
                        int start = parent.TestName.Name.IndexOf('(') + 1;
                        int end = parent.TestName.Name.LastIndexOf(')');

                        string[] parameters = parent.TestName.Name.Remove(end).Substring(start).Split(',');

                        TestFixtureAttribute[] attributes = (TestFixtureAttribute[])parent.FixtureType.GetCustomAttributes(typeof(TestFixtureAttribute), true);
                        foreach (var attribute in attributes)
                        {
                            if (MatchesAttribute(attribute, parameters))
                            {
                                parametersCache.Add(key, attribute.Arguments);
                                break;
                            }
                        }
                    }
                }
            }

            return parametersCache[key];
        }

        private static bool MatchesAttribute(TestFixtureAttribute attribute, string[] parameterNames)
        {
            if (attribute.Arguments.Length != parameterNames.Length)
                return false;

            for (int i = 0; i < parameterNames.Length; i++)
            {
                if (parameterNames[i] != attribute.Arguments[i].ToString())
                {
                    return false;
                }
            }

            return true;
        }

        private static string BuildCacheKey(Test test)
        {
            int end = test.TestName.Name.LastIndexOf(')');
            return test.TestName.Name.Remove(end);
        }
    }
}
