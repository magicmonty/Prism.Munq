using System;
using Xunit;
using Xunit.Sdk;

namespace Xunit.Wpf
{
    [AttributeUsage (AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer ("Xunit.Wpf.WpfFactDiscoverer", "Xunit.Wpf")]
    public class WpfFactAttribute : FactAttribute { }
}
