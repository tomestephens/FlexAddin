using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Core.Extensibility;

namespace FlexAddin.NUnit
{
    /// <summary>
    /// Flex Add-In
    /// </summary>
    [NUnitAddin(Description = "Flex Provider Add-in", Name = "Flex Provider", Type = ExtensionType.Core)]
    public class FlexAddin : IAddin
    {
        private static readonly Type providerType = typeof(FlexTestCaseProvider<>);
        private static readonly List<Type> addInAttributes;

        static FlexAddin()
        {   
            addInAttributes = new List<Type>() { typeof(FlexTestCaseAttribute) };
            // Discovery any Custom Flex Attributes in the AppDomain
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
                addInAttributes.AddRange(assembly.GetExportedTypes().Where(t => t.IsSubclassOf(typeof(FlexTestCaseAttribute))));
        }

        #region IAddin Members

        public bool Install(IExtensionHost host)
        {
            foreach (var attribute in addInAttributes)
                host.GetExtensionPoint("TestCaseProviders").Install(Activator.CreateInstance(providerType.MakeGenericType(attribute)));

            return true;
        }

        #endregion
    }
}
