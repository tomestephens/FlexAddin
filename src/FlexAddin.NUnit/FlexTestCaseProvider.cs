using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using NUnit.Core;
using NUnit.Core.Builders;
using NUnit.Core.Extensibility;
using NUnit.Framework;

namespace FlexAddin.NUnit
{
    /// <summary>
    /// Provider for building and permuting Flex Test Cases
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    public class FlexTestCaseProvider<TAttribute> : ITestCaseProvider2
        where TAttribute : FlexTestCaseAttribute
    {
        #region ITestCaseProvider Members
        /// <summary>
        /// Get test cases for the method
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IEnumerable GetTestCasesFor(MethodInfo method)
        {
            return GetTestCasesFor(method, null);
        }
        /// <summary>
        /// Check if there are test cases for the method
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public bool HasTestCasesFor(MethodInfo method)
        {
            return method.GetCustomAttributes(false).Any(a => a.GetType() == typeof(TAttribute));
        }

        #endregion

        #region ITestCaseProvider2 Members
        /// <summary>
        /// Check if there are test cases for the method
        /// </summary>
        /// <param name="method"></param>
        /// <param name="suite"></param>
        /// <returns></returns>
        public bool HasTestCasesFor(MethodInfo method, Test suite)
        {
            return HasTestCasesFor(method);
        }
        /// <summary>
        /// Get Test Cases for the Method
        /// </summary>
        /// <param name="method"></param>
        /// <param name="suite"></param>
        /// <returns></returns>
        public IEnumerable GetTestCasesFor(MethodInfo method, Test suite)
        {   
            ArrayList parameterList = new ArrayList();

            foreach (var row in GetRows(method))
            {
                // need an instance of the fixture for the Source Function
                var fixture = TestFixtureActivator.GetTestFixture(suite);
                IEnumerable<TestCaseData> baseTestCases = GetBaseTestCases(row, fixture);

                foreach (var testCase in baseTestCases)
                {
                    // if we're allowing permutations and one of the arguments is permutable 
                    // then we build out the permutations
                    if (row.Arguments.Any(a => a.IsPermutable()) && row.AllowPermutations())
                        parameterList.AddRange(Permute(testCase, row, method));
                    // otherwise just parameterize
                    else
                        parameterList.Add(Parameterize(testCase, row.Arguments, method));
                }
            }

            return parameterList;
        }


        #endregion
        
        #region Helpers

        private IEnumerable<TestCaseData> GetBaseTestCases(TAttribute row, object fixture)
        {
            // no source function situation -- not sure this will work as is?
            if (string.IsNullOrEmpty(row.SourceFunction))
            {
                return new List<TestCaseData>() { new TestCaseData() };
            }

            MemberInfo[] members = fixture.GetType().GetMember(row.SourceFunction, 
                                                               MemberTypes.Field | MemberTypes.Method | MemberTypes.Property,
                                                               BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (members.Length > 0)
            {
                MethodInfo source = members[0] as MethodInfo;
                if (source != null)
                    return (IEnumerable<TestCaseData>)source.Invoke(fixture, null);
            }

            throw new FlexAddinException(string.Format("Unable to locate {0}.{1}", fixture.GetType().FullName, row.SourceFunction));
        }

        private ParameterSet Parameterize(TestCaseData baseTestCase, object[] arguments, MethodInfo method)
        {
            ParameterSet parms = new ParameterSet();

            List<object> args = new List<object>(baseTestCase.Arguments);
            args.AddRange(arguments);

            if (args.Count != method.GetParameters().Length)
                throw new FlexAddinException(string.Format("Invalid number of arguments: The test accepts {0} paramters but the test case has {1} arguments.",
                                                            method.GetParameters().Length, args.Count));
            
            parms.Arguments = args.ToArray();
            parms.TestName = TestCaseNamer.Create(baseTestCase, method, arguments);
            
            foreach (var category in baseTestCase.Categories)
                parms.Categories.Add(category);

            return parms;
        }

        private ICollection Permute(TestCaseData baseTestCase, TAttribute row, MethodInfo method)
        {
            List<ParameterSet> permutations = new List<ParameterSet>();

            foreach (var perm in PermutationProvider.CreatePermutations(row.Arguments))
                permutations.Add(Parameterize(baseTestCase, perm.ToArray(), method));

            return permutations;
        }

        private IEnumerable<TAttribute> GetRows(MethodInfo method)
        {
            var attributes = method.GetCustomAttributes(typeof(TAttribute), false);
            return attributes.Cast<TAttribute>();
        }

        #endregion
    }
}
