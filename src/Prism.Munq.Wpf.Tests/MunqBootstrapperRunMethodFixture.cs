using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.ServiceLocation;
using Xunit;
using Moq;
using Prism.Events;
using Prism.Logging;
using Prism.Modularity;
using Prism.Regions;
using Munq;

namespace Prism.Munq.Wpf.Tests
{
    public class MunqBootstrapperRunMethodFixture
    {
        [Fact]
        public void CanRunBootstrapper ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();
        }

        [Fact]
        public void RunShouldNotFailIfReturnedNullShell ()
        {
            var bootstrapper = new DefaultMunqBootstrapper { ShellObject = null };
            bootstrapper.Run ();
        }

        [Fact]
        public void RunConfiguresServiceLocatorProvider ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();

            Assert.True (ServiceLocator.Current is MunqServiceLocatorAdapter);
        }

        [Fact]
        public void RunShouldInitializeContainer ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();
            var container = bootstrapper.BaseContainer;

            Assert.Null (container);

            bootstrapper.Run ();

            container = bootstrapper.BaseContainer;

            Assert.NotNull (container);
            Assert.IsType (typeof (MunqContainerWrapper), container);
        }

        [Fact]
        public void RunAddsCompositionContainerToContainer ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();

            var createdContainer = bootstrapper.CallCreateContainer ();
            var returnedContainer = createdContainer.Resolve<IIocContainer> ();
            Assert.NotNull (returnedContainer);
            Assert.Equal (typeof (MunqContainerWrapper), returnedContainer.GetType ());
        }

        [Fact]
        public void RunAddsDependencyResolverToContainer ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();

            var createdContainer = bootstrapper.CallCreateContainer ();
            var returnedContainer = createdContainer.Resolve<IDependencyResolver> ();
            Assert.NotNull (returnedContainer);
            Assert.Equal (typeof (MunqContainerWrapper), returnedContainer.GetType ());
        }

        [Fact]
        public void RunAddsDependencyRegistrarToContainer ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();

            var createdContainer = bootstrapper.CallCreateContainer ();
            var returnedContainer = createdContainer.Resolve<IDependecyRegistrar> ();
            Assert.NotNull (returnedContainer);
            Assert.Equal (typeof (MunqContainerWrapper), returnedContainer.GetType ());
        }

        [Fact]
        public void RunShouldCallInitializeModules ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();

            Assert.True (bootstrapper.InitializeModulesCalled);
        }

        [Fact]
        public void RunShouldCallConfigureDefaultRegionBehaviors ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();

            bootstrapper.Run ();

            Assert.True (bootstrapper.ConfigureDefaultRegionBehaviorsCalled);
        }

        [Fact]
        public void RunShouldCallConfigureRegionAdapterMappings ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();

            bootstrapper.Run ();

            Assert.True (bootstrapper.ConfigureRegionAdapterMappingsCalled);
        }

        [Fact]
        public void RunShouldAssignRegionManagerToReturnedShell ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();

            bootstrapper.Run ();

            Assert.NotNull (RegionManager.GetRegionManager (bootstrapper.BaseShell));
        }

        [Fact]
        public void RunShouldCallCreateLogger ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();

            bootstrapper.Run ();

            Assert.True (bootstrapper.CreateLoggerCalled);
        }

        [Fact]
        public void RunShouldCallCreateModuleCatalog ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();

            bootstrapper.Run ();

            Assert.True (bootstrapper.CreateModuleCatalogCalled);
        }

        [Fact]
        public void RunShouldCallConfigureModuleCatalog ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();

            bootstrapper.Run ();

            Assert.True (bootstrapper.ConfigureModuleCatalogCalled);
        }

        [Fact]
        public void RunShouldCallCreateContainer ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();

            bootstrapper.Run ();

            Assert.True (bootstrapper.CreateContainerCalled);
        }

        [Fact]
        public void RunShouldCallCreateShell ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();

            bootstrapper.Run ();

            Assert.True (bootstrapper.CreateShellCalled);
        }

        [Fact]
        public void RunShouldCallConfigureContainer ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();

            bootstrapper.Run ();

            Assert.True (bootstrapper.ConfigureContainerCalled);
        }

        [Fact]
        public void RunShouldCallConfigureServiceLocator ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();

            bootstrapper.Run ();

            Assert.True (bootstrapper.ConfigureServiceLocatorCalled);
        }

        [Fact]
        public void RunShouldCallConfigureViewModelLocator ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();

            bootstrapper.Run ();

            Assert.True (bootstrapper.ConfigureViewModelLocatorCalled);
        }

        [Fact]
        public void RunRegistersInstanceOfILoggerFacade ()
        {
            var mockedContainer = new Mock<IIocContainer> ();
            SetupMockedContainerForVerificationTests (mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper (mockedContainer.Object);

            bootstrapper.Run ();

            mockedContainer.Verify (c => c.RegisterInstance (bootstrapper.BaseLogger), Times.Once ());
        }

        [Fact]
        public void RunRegistersInstanceOfIModuleCatalog ()
        {
            var mockedContainer = new Mock<IIocContainer> ();
            SetupMockedContainerForVerificationTests (mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper (mockedContainer.Object);

            bootstrapper.Run ();

            mockedContainer.Verify (c => c.RegisterInstance (It.IsAny<IModuleCatalog> ()), Times.Once ());
        }

        [Fact]
        public void RunRegistersTypeForIServiceLocator ()
        {
            var mockedContainer = new Mock<IIocContainer> ();
            SetupMockedContainerForVerificationTests (mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper (mockedContainer.Object);

            bootstrapper.Run ();

            mockedContainer.Verify (c => c.Register<IServiceLocator> (It.IsAny<Func<IDependencyResolver, IServiceLocator>> ()), Times.Once ());
        }

        [Fact]
        public void RunRegistersTypeForIModuleInitializer ()
        {
            var mockedContainer = new Mock<IIocContainer> ();
            SetupMockedContainerForVerificationTests (mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper (mockedContainer.Object);

            bootstrapper.Run ();

            mockedContainer.Verify (c => c.Register (It.IsAny<Func<IDependencyResolver, IModuleInitializer>> ()), Times.Once ());
        }

        [Fact]
        public void RunRegistersTypeForIRegionManager ()
        {
            var mockedContainer = new Mock<IIocContainer> ();
            SetupMockedContainerForVerificationTests (mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper (mockedContainer.Object);

            bootstrapper.Run ();

            mockedContainer.Verify (c => c.Register (It.IsAny<Func<IDependencyResolver, IRegionManager>> ()), Times.Once ());
        }

        [Fact]
        public void RunRegistersTypeForRegionAdapterMappings ()
        {
            var mockedContainer = new Mock<IIocContainer> ();
            SetupMockedContainerForVerificationTests (mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper (mockedContainer.Object);

            bootstrapper.Run ();

            mockedContainer.Verify (c => c.Register (It.IsAny<Func<IDependencyResolver, RegionAdapterMappings>> ()), Times.Once ());
        }

        [Fact]
        public void RunRegistersTypeForIRegionViewRegistry ()
        {
            var mockedContainer = new Mock<IIocContainer> ();
            SetupMockedContainerForVerificationTests (mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper (mockedContainer.Object);

            bootstrapper.Run ();

            mockedContainer.Verify (c => c.Register (It.IsAny<Func<IDependencyResolver, IRegionViewRegistry>> ()), Times.Once ());
        }

        [Fact]
        public void RunRegistersTypeForIRegionBehaviorFactory ()
        {
            var mockedContainer = new Mock<IIocContainer> ();
            SetupMockedContainerForVerificationTests (mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper (mockedContainer.Object);

            bootstrapper.Run ();

            mockedContainer.Verify (c => c.Register (It.IsAny<Func<IDependencyResolver, IRegionBehaviorFactory>> ()), Times.Once ());
        }

        [Fact]
        public void RunRegistersTypeForIEventAggregator ()
        {
            var mockedContainer = new Mock<IIocContainer> ();
            SetupMockedContainerForVerificationTests (mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper (mockedContainer.Object);

            bootstrapper.Run ();

            mockedContainer.Verify (c => c.Register (It.IsAny<Func<IDependencyResolver, IEventAggregator>> ()), Times.Once ());
        }

        [Fact]
        public void RunFalseShouldNotRegisterDefaultServicesAndTypes ()
        {
            var mockedContainer = new Mock<IIocContainer> ();
            SetupMockedContainerForVerificationTests (mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper (mockedContainer.Object);
            bootstrapper.Run (false);

            mockedContainer.Verify (c => c.Register (It.IsAny<Func<IDependencyResolver, IEventAggregator>> ()), Times.Never ());
            mockedContainer.Verify (c => c.Register (It.IsAny<Func<IDependencyResolver, IRegionManager>> ()), Times.Never ());
            mockedContainer.Verify (c => c.Register (It.IsAny<Func<IDependencyResolver, RegionAdapterMappings>> ()), Times.Never ());
            mockedContainer.Verify (c => c.Register (It.IsAny<Func<IDependencyResolver, IServiceLocator>> ()), Times.Never ());
            mockedContainer.Verify (c => c.Register (It.IsAny<Func<IDependencyResolver, IModuleInitializer>> ()), Times.Never ());
        }

        [Fact]
        public void ModuleManagerRunCalled ()
        {
            // Have to use a non-mocked container because of IsRegistered<> extension method, Registrations property,and ContainerRegistration
            var container = new MunqContainerWrapper ();

            var mockedModuleInitializer = new Mock<IModuleInitializer> ();
            var mockedModuleManager = new Mock<IModuleManager> ();
            var regionAdapterMappings = new RegionAdapterMappings ();
            var serviceLocatorAdapter = new MunqServiceLocatorAdapter (container);
            var regionBehaviorFactory = new RegionBehaviorFactory (serviceLocatorAdapter);


            container.RegisterInstance<IServiceLocator> (serviceLocatorAdapter);
            container.RegisterInstance<IModuleCatalog> (new ModuleCatalog ());
            container.RegisterInstance<IModuleInitializer> (mockedModuleInitializer.Object);
            container.RegisterInstance<IModuleManager> (mockedModuleManager.Object);
            container.RegisterInstance<RegionAdapterMappings> (regionAdapterMappings);

            container.RegisterInstance<SelectorRegionAdapter> (new SelectorRegionAdapter (regionBehaviorFactory));
            container.RegisterInstance<ItemsControlRegionAdapter> (new ItemsControlRegionAdapter (regionBehaviorFactory));
            container.RegisterInstance<ContentControlRegionAdapter> (new ContentControlRegionAdapter (regionBehaviorFactory));

            var bootstrapper = new MockedContainerBootstrapper (container);

            bootstrapper.Run ();

            mockedModuleManager.Verify (mm => mm.Run (), Times.Once ());
        }

        [Fact]
        public void RunShouldCallTheMethodsInOrder ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();

            Assert.Equal ("CreateLogger", bootstrapper.MethodCalls [0]);
            Assert.Equal ("CreateModuleCatalog", bootstrapper.MethodCalls [1]);
            Assert.Equal ("ConfigureModuleCatalog", bootstrapper.MethodCalls [2]);
            Assert.Equal ("CreateContainer", bootstrapper.MethodCalls [3]);
            Assert.Equal ("ConfigureContainer", bootstrapper.MethodCalls [4]);
            Assert.Equal ("ConfigureServiceLocator", bootstrapper.MethodCalls [5]);
            Assert.Equal ("ConfigureViewModelLocator", bootstrapper.MethodCalls [6]);
            Assert.Equal ("ConfigureRegionAdapterMappings", bootstrapper.MethodCalls [7]);
            Assert.Equal ("ConfigureDefaultRegionBehaviors", bootstrapper.MethodCalls [8]);
            Assert.Equal ("RegisterFrameworkExceptionTypes", bootstrapper.MethodCalls [9]);
            Assert.Equal ("CreateShell", bootstrapper.MethodCalls [10]);
            Assert.Equal ("InitializeShell", bootstrapper.MethodCalls [11]);
            Assert.Equal ("InitializeModules", bootstrapper.MethodCalls [12]);
        }

        [Fact]
        public void RunShouldLogBootstrapperSteps ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.True (messages [0].Contains ("Logger was created successfully."));
            Assert.True (messages [1].Contains ("Creating module catalog."));
            Assert.True (messages [2].Contains ("Configuring module catalog."));
            Assert.True (messages [3].Contains ("Creating Munq container."));
            Assert.True (messages [4].Contains ("Configuring the Munq container."));
            Assert.True (messages [5].Contains ("Configuring ServiceLocator singleton."));
            Assert.True (messages [6].Contains ("Configuring the ViewModelLocator to use Munq."));
            Assert.True (messages [7].Contains ("Configuring region adapters."));
            Assert.True (messages [8].Contains ("Configuring default region behaviors."));
            Assert.True (messages [9].Contains ("Registering Framework Exception Types."));
            Assert.True (messages [10].Contains ("Creating the shell."));
            Assert.True (messages [11].Contains ("Setting the RegionManager."));
            Assert.True (messages [12].Contains ("Updating Regions."));
            Assert.True (messages [13].Contains ("Initializing the shell."));
            Assert.True (messages [14].Contains ("Initializing modules."));
            Assert.True (messages [15].Contains ("Bootstrapper sequence completed."));
        }

        [Fact]
        public void RunShouldLogLoggerCreationSuccess ()
        {
            const string expectedMessageText = "Logger was created successfully.";
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.True (messages.Contains (expectedMessageText));
        }
        [Fact]
        public void RunShouldLogAboutModuleCatalogCreation ()
        {
            const string expectedMessageText = "Creating module catalog.";
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.True (messages.Contains (expectedMessageText));
        }

        [Fact]
        public void RunShouldLogAboutConfiguringModuleCatalog ()
        {
            const string expectedMessageText = "Configuring module catalog.";
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.True (messages.Contains (expectedMessageText));
        }

        [Fact]
        public void RunShouldLogAboutCreatingTheContainer ()
        {
            const string expectedMessageText = "Creating Munq container.";
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.True (messages.Contains (expectedMessageText));
        }

        [Fact]
        public void RunShouldLogAboutConfiguringContainer ()
        {
            const string expectedMessageText = "Configuring the Munq container.";
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.True (messages.Contains (expectedMessageText));
        }

        [Fact]
        public void RunShouldLogAboutConfiguringViewModelLocator ()
        {
            const string expectedMessageText = "Configuring the ViewModelLocator to use Munq.";
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.True (messages.Contains (expectedMessageText));
        }

        [Fact]
        public void RunShouldLogAboutConfiguringRegionAdapters ()
        {
            const string expectedMessageText = "Configuring region adapters.";
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.True (messages.Contains (expectedMessageText));
        }

        [Fact]
        public void RunShouldLogAboutConfiguringRegionBehaviors ()
        {
            const string expectedMessageText = "Configuring default region behaviors.";
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.True (messages.Contains (expectedMessageText));
        }

        [Fact]
        public void RunShouldLogAboutRegisteringFrameworkExceptionTypes ()
        {
            const string expectedMessageText = "Registering Framework Exception Types.";
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.True (messages.Contains (expectedMessageText));
        }

        [Fact]
        public void RunShouldLogAboutCreatingTheShell ()
        {
            const string expectedMessageText = "Creating the shell.";
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.True (messages.Contains (expectedMessageText));
        }

        [Fact]
        public void RunShouldLogAboutInitializingTheShellIfShellCreated ()
        {
            const string expectedMessageText = "Initializing the shell.";
            var bootstrapper = new DefaultMunqBootstrapper ();

            bootstrapper.Run ();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.True (messages.Contains (expectedMessageText));
        }

        [Fact]
        public void RunShouldNotLogAboutInitializingTheShellIfShellIsNotCreated ()
        {
            const string expectedMessageText = "Initializing shell";
            var bootstrapper = new DefaultMunqBootstrapper { ShellObject = null };

            bootstrapper.Run ();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.False (messages.Contains (expectedMessageText));
        }

        [Fact]
        public void RunShouldLogAboutInitializingModules ()
        {
            const string expectedMessageText = "Initializing modules.";
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.True (messages.Contains (expectedMessageText));
        }

        [Fact]
        public void RunShouldLogAboutRunCompleting ()
        {
            const string expectedMessageText = "Bootstrapper sequence completed.";
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.True (messages.Contains (expectedMessageText));
        }

        private static void SetupMockedContainerForVerificationTests (Mock<IIocContainer> mockedContainer)
        {
            var mockedModuleInitializer = new Mock<IModuleInitializer> ();
            var mockedModuleManager = new Mock<IModuleManager> ();
            var regionAdapterMappings = new RegionAdapterMappings ();
            var serviceLocatorAdapter = new MunqServiceLocatorAdapter (mockedContainer.Object);
            var regionBehaviorFactory = new RegionBehaviorFactory (serviceLocatorAdapter);

            mockedContainer
                .Setup (c => c.Resolve<IServiceLocator> ())
                .Returns (serviceLocatorAdapter);

            mockedContainer.Setup (c => c.RegisterInstance (It.IsAny<string> (), It.IsAny<Type> (), It.IsAny<object> ()));

            mockedContainer
                .Setup (c => c.Resolve<IModuleCatalog> ())
                .Returns (new ModuleCatalog ());

            mockedContainer
                .Setup (c => c.Resolve<IModuleInitializer> ())
                .Returns (mockedModuleInitializer.Object);

            mockedContainer
                .Setup (c => c.Resolve<IModuleManager> ())
                .Returns (mockedModuleManager.Object);

            mockedContainer
                .Setup (c => c.Resolve<RegionAdapterMappings> ())
                .Returns (regionAdapterMappings);

            mockedContainer
                .Setup (c => c.Resolve<SelectorRegionAdapter> ())
                .Returns (new SelectorRegionAdapter (regionBehaviorFactory));

            mockedContainer.Setup (c => c.Resolve<ItemsControlRegionAdapter> ())
                .Returns (new ItemsControlRegionAdapter (regionBehaviorFactory));

            mockedContainer.Setup (c => c.Resolve<ContentControlRegionAdapter> ())
                .Returns (new ContentControlRegionAdapter (regionBehaviorFactory));
        }

        private class MockedContainerBootstrapper : MunqBootstrapper
        {
            private readonly IIocContainer container;

            public ILoggerFacade BaseLogger { get { return base.Logger; } }

            public void CallConfigureContainer ()
            {
                base.ConfigureContainer ();
            }

            public MockedContainerBootstrapper (IIocContainer container)
            {
                this.container = container;
            }

            protected override IIocContainer CreateContainer ()
            {
                return container;
            }

            protected override DependencyObject CreateShell ()
            {
                return new UserControl ();
            }

            protected override void InitializeShell ()
            {
                // no op
            }
        }

    }
}
