using System;
using System.Windows;
using Prism.IocContainer.Wpf.Tests.Support;
using Prism.Modularity;
using Xunit;

namespace Prism.Munq.Wpf.Tests
{
    public class MunqBootstrapperNullModuleCatalogFixture : BootstrapperFixtureBase
    {
        [Fact]
        public void NullModuleCatalogThrowsOnDefaultModuleInitialization ()
        {
            var bootstrapper = new NullModuleCatalogBootstrapper ();

            AssertExceptionThrownOnRun (bootstrapper, typeof (InvalidOperationException), "IModuleCatalog");
        }

        private class NullModuleCatalogBootstrapper : MunqBootstrapper
        {
            protected override IModuleCatalog CreateModuleCatalog ()
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