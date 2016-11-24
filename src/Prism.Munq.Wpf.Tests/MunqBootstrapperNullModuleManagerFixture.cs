using System.Windows;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using Prism.Regions;
using Shouldly;

namespace Prism.Munq.Wpf.Tests
{
    [TestFixture]
    public class MunqBootstrapperNullModuleManagerFixture
    {
        [Test]
        public void RunShouldNotCallInitializeModulesWhenModuleManagerNotFound()
        {
            var bootstrapper = new NullModuleManagerBootstrapper();

            bootstrapper.Run();
            bootstrapper.InitializeModulesCalled.ShouldBeFalse();
        }

        private class NullModuleManagerBootstrapper : MunqBootstrapper
        {
            public bool InitializeModulesCalled;

            protected override void ConfigureContainer()
            {
                Container.RegisterInstance(Logger);
                Container.RegisterInstance(ModuleCatalog);

                RegisterTypeIfMissing<IServiceLocator>(_ => new MunqServiceLocatorAdapter(Container), true);
            }

            protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
            {
                return null;
            }

            protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
            {
                return null;
            }

            protected override DependencyObject CreateShell()
            {
                return null;
            }

            protected override void InitializeModules()
            {
                InitializeModulesCalled = true;
            }
        }
    }
}
