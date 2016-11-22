using System.Windows;
using Microsoft.Practices.ServiceLocation;
using Prism.Regions;
using Shouldly;
using Xunit;

namespace Prism.Munq.Wpf.Tests
{
    public class MunqBootstrapperNullModuleManagerFixture
    {
        [Fact]
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