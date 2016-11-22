using System;
using Xunit;
using Xunit.Sdk;

namespace Prism.IocContainer.Wpf.Tests.Support.WPFHelpers
{
    [AttributeUsage (AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("Prism.IocContainer.Wpf.Tests.Support.WPFHelpers.WpfFactDiscoverer", "Prism.IocContainer.Wpf.Tests.Support")]
    public class WpfFactAttribute : FactAttribute { }
}
