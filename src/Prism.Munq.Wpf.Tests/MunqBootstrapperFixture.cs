using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Prism.IocContainer.Wpf.Tests.Support;
using Prism.IocContainer.Wpf.Tests.Support.Mocks;
using Prism.Logging;
using Prism.Modularity;
using Prism.Regions;
using Xunit;
using Xunit.Wpf;

namespace Prism.Munq.Wpf.Tests
{
    public class MunqBootstrapperFixture : BootstrapperFixtureBase
    {
        [WpfFact]
        public void ContainerDefaultsToNull ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();
            var container = bootstrapper.Container;

            Assert.Null (container);
        }

        [WpfFact]
        public void CanCreateConcreteBootstrapper ()
        {
            new DefaultMunqBootstrapper ();
        }

        [WpfFact]
        public void CreateContainerShouldInitializeContainer ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();

            IIocContainer container = bootstrapper.CallCreateContainer ();

            Assert.NotNull (container);
            Assert.IsType (typeof (MunqContainerWrapper), container);
        }

        [WpfFact]
        public void ConfigureContainerAddsModuleCatalogToContainer ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();

            var returnedCatalog = bootstrapper.BaseContainer.Resolve<IModuleCatalog> ();
            Assert.NotNull (returnedCatalog);
            Assert.True (returnedCatalog is ModuleCatalog);
        }

        [WpfFact]
        public void ConfigureContainerAddsLoggerFacadeToContainer ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();

            var returnedCatalog = bootstrapper.BaseContainer.Resolve<ILoggerFacade> ();
            Assert.NotNull (returnedCatalog);
        }

        [WpfFact]
        public void ConfigureContainerAddsRegionNavigationJournalEntryToContainer ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();

            var actual1 = bootstrapper.BaseContainer.Resolve<IRegionNavigationJournalEntry> ();
            var actual2 = bootstrapper.BaseContainer.Resolve<IRegionNavigationJournalEntry> ();

            Assert.NotNull (actual1);
            Assert.NotNull (actual2);
            Assert.NotSame (actual1, actual2);
        }

        [WpfFact]
        public void ConfigureContainerAddsRegionNavigationJournalToContainer ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();

            var actual1 = bootstrapper.BaseContainer.Resolve<IRegionNavigationJournal> ();
            var actual2 = bootstrapper.BaseContainer.Resolve<IRegionNavigationJournal> ();

            Assert.NotNull (actual1);
            Assert.NotNull (actual2);
            Assert.NotSame (actual1, actual2);
        }

        [WpfFact]
        public void ConfigureContainerAddsRegionNavigationServiceToContainer ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();

            var actual1 = bootstrapper.BaseContainer.Resolve<IRegionNavigationService> ();
            var actual2 = bootstrapper.BaseContainer.Resolve<IRegionNavigationService> ();

            Assert.NotNull (actual1);
            Assert.NotNull (actual2);
            Assert.NotSame (actual1, actual2);
        }

        [WpfFact]
        public void ConfigureContainerAddsNavigationTargetHandlerToContainer ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();

            var actual1 = bootstrapper.BaseContainer.Resolve<IRegionNavigationContentLoader> ();
            var actual2 = bootstrapper.BaseContainer.Resolve<IRegionNavigationContentLoader> ();

            Assert.NotNull (actual1);
            Assert.NotNull (actual2);
            Assert.Same (actual1, actual2);
        }
    }

    internal class DefaultMunqBootstrapper : MunqBootstrapper
    {
        public List<string> MethodCalls = new List<string> ();
        public bool InitializeModulesCalled;
        public bool ConfigureRegionAdapterMappingsCalled;
        public RegionAdapterMappings DefaultRegionAdapterMappings;
        public bool CreateLoggerCalled;
        public bool CreateModuleCatalogCalled;
        public bool ConfigureContainerCalled;
        public bool CreateShellCalled;
        public bool CreateContainerCalled;
        public bool ConfigureModuleCatalogCalled;
        public bool InitializeShellCalled;
        public bool ConfigureServiceLocatorCalled;
        public bool ConfigureViewModelLocatorCalled;
        public bool ConfigureDefaultRegionBehaviorsCalled;
        public DependencyObject ShellObject = new UserControl ();

        public DependencyObject BaseShell => Shell;

        public IIocContainer BaseContainer {
            get { return Container; }
            set { Container = value; }
        }

        public MockLoggerAdapter BaseLogger => Logger as MockLoggerAdapter;

        public IIocContainer CallCreateContainer ()
        {
            return CreateContainer ();
        }

        protected override IIocContainer CreateContainer ()
        {
            MethodCalls.Add (MethodBase.GetCurrentMethod ().Name);
            CreateContainerCalled = true;
            return base.CreateContainer ();
        }

        protected override void ConfigureContainer ()
        {
            MethodCalls.Add (MethodBase.GetCurrentMethod ().Name);
            ConfigureContainerCalled = true;
            base.ConfigureContainer ();
        }

        protected override ILoggerFacade CreateLogger ()
        {
            MethodCalls.Add (MethodBase.GetCurrentMethod ().Name);
            CreateLoggerCalled = true;
            return new MockLoggerAdapter ();
        }

        protected override DependencyObject CreateShell ()
        {
            MethodCalls.Add (MethodBase.GetCurrentMethod ().Name);
            CreateShellCalled = true;
            return ShellObject;
        }

        protected override void ConfigureServiceLocator ()
        {
            MethodCalls.Add (MethodBase.GetCurrentMethod ().Name);
            ConfigureServiceLocatorCalled = true;
            base.ConfigureServiceLocator ();
        }

        protected override void ConfigureViewModelLocator ()
        {
            MethodCalls.Add (MethodBase.GetCurrentMethod ().Name);
            ConfigureViewModelLocatorCalled = true;
            base.ConfigureViewModelLocator ();
        }

        protected override IModuleCatalog CreateModuleCatalog ()
        {
            MethodCalls.Add (MethodBase.GetCurrentMethod ().Name);
            CreateModuleCatalogCalled = true;
            return base.CreateModuleCatalog ();
        }

        protected override void ConfigureModuleCatalog ()
        {
            MethodCalls.Add (MethodBase.GetCurrentMethod ().Name);
            ConfigureModuleCatalogCalled = true;
            base.ConfigureModuleCatalog ();
        }

        protected override void InitializeShell ()
        {
            MethodCalls.Add (MethodBase.GetCurrentMethod ().Name);
            InitializeShellCalled = true;
            // no op
        }

        protected override void InitializeModules ()
        {
            MethodCalls.Add (MethodBase.GetCurrentMethod ().Name);
            InitializeModulesCalled = true;
            base.InitializeModules ();
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors ()
        {
            MethodCalls.Add (MethodBase.GetCurrentMethod ().Name);
            ConfigureDefaultRegionBehaviorsCalled = true;
            return base.ConfigureDefaultRegionBehaviors ();
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings ()
        {
            MethodCalls.Add (MethodBase.GetCurrentMethod ().Name);
            ConfigureRegionAdapterMappingsCalled = true;
            var regionAdapterMappings = base.ConfigureRegionAdapterMappings ();

            DefaultRegionAdapterMappings = regionAdapterMappings;

            return regionAdapterMappings;
        }

        protected override void RegisterFrameworkExceptionTypes ()
        {
            MethodCalls.Add (MethodBase.GetCurrentMethod ().Name);
            base.RegisterFrameworkExceptionTypes ();
        }

        public void CallRegisterFrameworkExceptionTypes ()
        {
            base.RegisterFrameworkExceptionTypes ();
        }
    }

}
