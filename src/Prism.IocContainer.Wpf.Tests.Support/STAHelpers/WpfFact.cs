﻿using System;
using Xunit.Sdk;

namespace Xunit.Wpf
{
    [AttributeUsage (AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer ("Xunit.Wpf.WpfFactDiscoverer", "Prism.IocContainer.Wpf.Tests.Support")]
    public class WpfFactAttribute : FactAttribute { }
}
