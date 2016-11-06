using System;
using System.Windows;
using Prism.IocContainer.Wpf.Tests.Support;
using Prism.Logging;
using Xunit;

namespace Prism.Munq.Wpf.Tests
{
    public class MunqBootstrapperNullLoggerFixture : BootstrapperFixtureBase
    {
        [Fact]
        public void NullLoggerThrows ()
        {
            var bootstrapper = new NullLoggerBootstrapper ();

            AssertExceptionThrownOnRun (bootstrapper, typeof (InvalidOperationException), "ILoggerFacade");
        }

        internal class NullLoggerBootstrapper : MunqBootstrapper
        {
            protected override ILoggerFacade CreateLogger ()
            {
                return null;
            }

            protected override DependencyObject CreateShell ()
            {
                throw new NotImplementedException ();
            }

            protected override void InitializeShell ()
            {
                throw new NotImplementedException ();
            }
        }
    }
}