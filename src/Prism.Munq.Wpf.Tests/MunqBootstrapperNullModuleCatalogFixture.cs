using System;
using System.Windows;
using NUnit.Framework;
using Prism.Modularity;
using Shouldly;

namespace Prism.Munq.Wpf.Tests
{
    [TestFixture]
    public class MunqBootstrapperNullModuleCatalogFixture
    {
        [Test]
        public void NullModuleCatalogThrowsOnDefaultModuleInitialization()
        {
            var bootstrapper = new NullModuleCatalogBootstrapper();

            Should
                .Throw<InvalidOperationException>(() => bootstrapper.Run())
                .Message.ShouldContain("IModuleCatalog");
        }

        private class NullModuleCatalogBootstrapper : MunqBootstrapper
        {
            protected override IModuleCatalog CreateModuleCatalog()
            {
                return null;
            }

            protected override DependencyObject CreateShell()
            {
                throw new NotImplementedException();
            }

            protected override void InitializeShell()
            {
                throw new NotImplementedException();
            }
        }
    }
}
