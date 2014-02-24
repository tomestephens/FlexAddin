using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlexAddin.NUnit
{
    /// <summary>
    /// Exception thrown by the Flex NUnit Addin
    /// </summary>
    public class FlexAddinException : Exception
    {
        /// <summary>
        /// Create exception with message
        /// </summary>
        /// <param name="message"></param>
        public FlexAddinException(string message) { }
    }
}
