using System;
using System.Windows;
using Prism.IocContainer.Wpf.Tests.Support;
using Xunit;
using Xunit.Wpf;

namespace Prism.Munq.Wpf.Tests
{
    public class MunqBootstrapperNullContainerFixture : BootstrapperFixtureBase
    {
        [WpfFact]
        public void RunThrowsWhenNullContainerCreated ()
        {
            var bootstrapper = new NullContainerBootstrapper ();

            AssertExceptionThrownOnRun (bootstrapper, typeof (InvalidOperationException), "IocContainer");
        }

        private class NullContainerBootstrapper : MunqBootstrapper
        {
            protected override IIocContainer CreateContainer ()
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