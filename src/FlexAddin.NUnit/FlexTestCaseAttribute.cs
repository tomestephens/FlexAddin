using System;
using NUnit.Framework;

namespace FlexAddin.NUnit
{
    /// <summary>
    /// Flex Test Case
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=true)]
    public class FlexTestCaseAttribute : TestAttribute
    {
        /// <summary>
        /// Gets or Sets if Permutations are allowed
        /// </summary>
        public bool NoPermutation { get; set; }
        /// <summary>
        /// Gets the Test Case Source Function
        /// </summary>
        public string SourceFunction { get; private set; }
        /// <summary>
        /// Gets the Addition Arguments
        /// </summary>
        public object[] Arguments { get; private set; }
        /// <summary>
        /// Create a Flex Test Case
        /// </summary>
        /// <param name="sourceFunction"></param>
        /// <param name="arguments"></param>
        public FlexTestCaseAttribute(string sourceFunction, params object[] arguments)
        {
            SourceFunction = sourceFunction;
            Arguments = arguments;
        }
        /// <summary>
        /// Create a Flex Test Case without a source function
        /// </summary>
        /// <param name="arguments"></param>
        public FlexTestCaseAttribute(params object[] arguments)
        {
            Arguments = arguments;
        }

        public bool AllowPermutations()
        {
            return NoPermutation == false;
        }
    }
}
