using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.ServiceLocation;
using Moq;
using Prism.Events;
using Prism.Logging;
using Prism.Modularity;
using Prism.Regions;
using Munq;
using Prism.IocContainer.Wpf.Tests.Support.WPFHelpers;
using Shouldly;

namespace Prism.Munq.Wpf.Tests
{
    public class MunqBootstrapperRunMethodFixture
    {
        [WpfFact]
        public void CanRunBootstrapper()
        {
            var bootstrapper = new DefaultMunqBootstrapper();
            Should.NotThrow(() => bootstrapper.Run());
        }

        [WpfFact]
        public void RunShouldNotFailIfReturnedNullShell()
        {
            var bootstrapper = new DefaultMunqBootstrapper {ShellObject = null};
            Should.NotThrow(() => bootstrapper.Run());
        }

        [WpfFact]
        public void RunConfiguresServiceLocatorProvider()
        {
            var bootstrapper = new DefaultMunqBootstrapper();
            bootstrapper.Run();

            ServiceLocator.Current.ShouldBeOfType<MunqServiceLocatorAdapter>();
        }

        [WpfFact]
        public void RunShouldInitializeContainer()
        {
            var bootstrapper = new DefaultMunqBootstrapper();
            var container = bootstrapper.BaseContainer;

            container.ShouldBeNull();

            bootstrapper.Run();

            container = bootstrapper.BaseContainer;

            container.ShouldNotBeNull();
            container.ShouldBeOfType<MunqContainerWrapper>();
        }

        [WpfFact]
        public void RunAddsCompositionContainerToContainer()
        {
            var bootstrapper = new DefaultMunqBootstrapper();

            var createdContainer = bootstrapper.CallCreateContainer();
            var returnedContainer = createdContainer.Resolve<IIocContainer>();

            returnedContainer.ShouldNotBeNull();
            returnedContainer.ShouldBeOfType<MunqContainerWrapper>();
        }

        [WpfFact]
        public void RunAddsDependencyResolverToContainer()
        {
            var bootstrapper = new DefaultMunqBootstrapper();

            var createdContainer = bootstrapper.CallCreateContainer();
            var returnedContainer = createdContainer.Resolve<IDependencyResolver>();

            returnedContainer.ShouldNotBeNull();
            returnedContainer.ShouldBeOfType<MunqContainerWrapper>();
        }

        [WpfFact]
        public void RunAddsDependencyRegistrarToContainer()
        {
            var bootstrapper = new DefaultMunqBootstrapper();

            var createdContainer = bootstrapper.CallCreateContainer();
            var returnedContainer = createdContainer.Resolve<IDependecyRegistrar>();

            returnedContainer.ShouldNotBeNull();
            returnedContainer.ShouldBeOfType<MunqContainerWrapper>();
        }

        [WpfFact]
        public void RunShouldCallInitializeModules()
        {
            var bootstrapper = new DefaultMunqBootstrapper();
            bootstrapper.Run();

            bootstrapper.InitializeModulesCalled.ShouldBeTrue();
        }

        [WpfFact]
        public void RunShouldCallConfigureDefaultRegionBehaviors()
        {
            var bootstrapper = new DefaultMunqBootstrapper();

            bootstrapper.Run();

            bootstrapper.ConfigureDefaultRegionBehaviorsCalled.ShouldBeTrue();
        }

        [WpfFact]
        public void RunShouldCallConfigureRegionAdapterMappings()
        {
            var bootstrapper = new DefaultMunqBootstrapper();

            bootstrapper.Run();

            bootstrapper.ConfigureRegionAdapterMappingsCalled.ShouldBeTrue();
        }

        [WpfFact]
        public void RunShouldAssignRegionManagerToReturnedShell()
        {
            var bootstrapper = new DefaultMunqBootstrapper();

            bootstrapper.Run();

            RegionManager.GetRegionManager(bootstrapper.BaseShell).ShouldNotBeNull();
        }

        [WpfFact]
        public void RunShouldCallCreateLogger()
        {
            var bootstrapper = new DefaultMunqBootstrapper();

            bootstrapper.Run();

            bootstrapper.CreateLoggerCalled.ShouldBeTrue();
        }

        [WpfFact]
        public void RunShouldCallCreateModuleCatalog()
        {
            var bootstrapper = new DefaultMunqBootstrapper();

            bootstrapper.Run();

            bootstrapper.CreateModuleCatalogCalled.ShouldBeTrue();
        }

        [WpfFact]
        public void RunShouldCallConfigureModuleCatalog()
        {
            var bootstrapper = new DefaultMunqBootstrapper();

            bootstrapper.Run();

            bootstrapper.ConfigureModuleCatalogCalled.ShouldBeTrue();
        }

        [WpfFact]
        public void RunShouldCallCreateContainer()
        {
            var bootstrapper = new DefaultMunqBootstrapper();

            bootstrapper.Run();

            bootstrapper.CreateContainerCalled.ShouldBeTrue();
        }

        [WpfFact]
        public void RunShouldCallCreateShell()
        {
            var bootstrapper = new DefaultMunqBootstrapper();

            bootstrapper.Run();

            bootstrapper.CreateShellCalled.ShouldBeTrue();
        }

        [WpfFact]
        public void RunShouldCallConfigureContainer()
        {
            var bootstrapper = new DefaultMunqBootstrapper();

            bootstrapper.Run();

            bootstrapper.ConfigureContainerCalled.ShouldBeTrue();
        }

        [WpfFact]
        public void RunShouldCallConfigureServiceLocator()
        {
            var bootstrapper = new DefaultMunqBootstrapper();

            bootstrapper.Run();

            bootstrapper.ConfigureServiceLocatorCalled.ShouldBeTrue();
        }

        [WpfFact]
        public void RunShouldCallConfigureViewModelLocator()
        {
            var bootstrapper = new DefaultMunqBootstrapper();

            bootstrapper.Run();

            bootstrapper.ConfigureViewModelLocatorCalled.ShouldBeTrue();
        }

        [WpfFact]
        public void RunRegistersInstanceOfILoggerFacade()
        {
            var mockedContainer = new Mock<IIocContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.RegisterInstance(bootstrapper.BaseLogger), Times.Once());
        }

        [WpfFact]
        public void RunRegistersInstanceOfIModuleCatalog()
        {
            var mockedContainer = new Mock<IIocContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.RegisterInstance(It.IsAny<IModuleCatalog>()), Times.Once());
        }

        [WpfFact]
        public void RunRegistersTypeForIServiceLocator()
        {
            var mockedContainer = new Mock<IIocContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.Register(It.IsAny<Func<IDependencyResolver, IServiceLocator>>()), Times.Once());
        }

        [WpfFact]
        public void RunRegistersTypeForIModuleInitializer()
        {
            var mockedContainer = new Mock<IIocContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.Register(It.IsAny<Func<IDependencyResolver, IModuleInitializer>>()),
                Times.Once());
        }

        [WpfFact]
        public void RunRegistersTypeForIRegionManager()
        {
            var mockedContainer = new Mock<IIocContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.Register(It.IsAny<Func<IDependencyResolver, IRegionManager>>()), Times.Once());
        }

        [WpfFact]
        public void RunRegistersTypeForRegionAdapterMappings()
        {
            var mockedContainer = new Mock<IIocContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.Register(It.IsAny<Func<IDependencyResolver, RegionAdapterMappings>>()),
                Times.Once());
        }

        [WpfFact]
        public void RunRegistersTypeForIRegionViewRegistry()
        {
            var mockedContainer = new Mock<IIocContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.Register(It.IsAny<Func<IDependencyResolver, IRegionViewRegistry>>()),
                Times.Once());
        }

        [WpfFact]
        public void RunRegistersTypeForIRegionBehaviorFactory()
        {
            var mockedContainer = new Mock<IIocContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.Register(It.IsAny<Func<IDependencyResolver, IRegionBehaviorFactory>>()),
                Times.Once());
        }

        [WpfFact]
        public void RunRegistersTypeForIEventAggregator()
        {
            var mockedContainer = new Mock<IIocContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.Register(It.IsAny<Func<IDependencyResolver, IEventAggregator>>()),
                Times.Once());
        }

        [WpfFact]
        public void RunFalseShouldNotRegisterDefaultServicesAndTypes()
        {
            var mockedContainer = new Mock<IIocContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);
            bootstrapper.Run(false);

            mockedContainer.Verify(c => c.Register(It.IsAny<Func<IDependencyResolver, IEventAggregator>>()),
                Times.Never());
            mockedContainer.Verify(c => c.Register(It.IsAny<Func<IDependencyResolver, IRegionManager>>()), Times.Never());
            mockedContainer.Verify(c => c.Register(It.IsAny<Func<IDependencyResolver, RegionAdapterMappings>>()),
                Times.Never());
            mockedContainer.Verify(c => c.Register(It.IsAny<Func<IDependencyResolver, IServiceLocator>>()),
                Times.Never());
            mockedContainer.Verify(c => c.Register(It.IsAny<Func<IDependencyResolver, IModuleInitializer>>()),
                Times.Never());
        }

        [WpfFact]
        public void ModuleManagerRunCalled()
        {
            // Have to use a non-mocked container because of IsRegistered<> extension method, Registrations property,and ContainerRegistration
            var container = new MunqContainerWrapper();

            var mockedModuleInitializer = new Mock<IModuleInitializer>();
            var mockedModuleManager = new Mock<IModuleManager>();
            var regionAdapterMappings = new RegionAdapterMappings();
            var serviceLocatorAdapter = new MunqServiceLocatorAdapter(container);
            var regionBehaviorFactory = new RegionBehaviorFactory(serviceLocatorAdapter);


            container.RegisterInstance<IServiceLocator>(serviceLocatorAdapter);
            container.RegisterInstance<IModuleCatalog>(new ModuleCatalog());
            container.RegisterInstance(mockedModuleInitializer.Object);
            container.RegisterInstance(mockedModuleManager.Object);
            container.RegisterInstance(regionAdapterMappings);

            container.RegisterInstance(new SelectorRegionAdapter(regionBehaviorFactory));
            container.RegisterInstance(new ItemsControlRegionAdapter(regionBehaviorFactory));
            container.RegisterInstance(new ContentControlRegionAdapter(regionBehaviorFactory));

            var bootstrapper = new MockedContainerBootstrapper(container);

            bootstrapper.Run();

            mockedModuleManager.Verify(mm => mm.Run(), Times.Once());
        }

        [WpfFact]
        public void RunShouldCallTheMethodsInOrder()
        {
            var bootstrapper = new DefaultMunqBootstrapper();
            bootstrapper.Run();

            bootstrapper.MethodCalls.ShouldBe(
                            new[]
                            {
                                "CreateLogger",
                                "CreateModuleCatalog",
                                "ConfigureModuleCatalog",
                                "CreateContainer",
                                "ConfigureContainer",
                                "ConfigureServiceLocator",
                                "ConfigureViewModelLocator",
                                "ConfigureRegionAdapterMappings",
                                "ConfigureDefaultRegionBehaviors",
                                "RegisterFrameworkExceptionTypes",
                                "CreateShell",
                                "InitializeShell",
                                "InitializeModules"
                            });
        }

        [WpfFact]
        public void RunShouldLogBootstrapperSteps()
        {
            var bootstrapper = new DefaultMunqBootstrapper();
            bootstrapper.Run();
            IList<string> messages = bootstrapper.BaseLogger.Messages;

            messages[0].ShouldContain("Logger was created successfully.");
            messages[1].ShouldContain("Creating module catalog.");
            messages[2].ShouldContain("Configuring module catalog.");
            messages[3].ShouldContain("Creating Munq container.");
            messages[4].ShouldContain("Configuring the Munq container.");
            messages[5].ShouldContain("Configuring ServiceLocator singleton.");
            messages[6].ShouldContain("Configuring the ViewModelLocator to use Munq.");
            messages[7].ShouldContain("Configuring region adapters.");
            messages[8].ShouldContain("Configuring default region behaviors.");
            messages[9].ShouldContain("Registering Framework Exception Types.");
            messages[10].ShouldContain("Creating the shell.");
            messages[11].ShouldContain("Setting the RegionManager.");
            messages[12].ShouldContain("Updating Regions.");
            messages[13].ShouldContain("Initializing the shell.");
            messages[14].ShouldContain("Initializing modules.");
            messages[15].ShouldContain("Bootstrapper sequence completed.");
        }

        [WpfFact]
        public void RunShouldLogLoggerCreationSuccess()
        {
            const string expectedMessageText = "Logger was created successfully.";
            var bootstrapper = new DefaultMunqBootstrapper();
            bootstrapper.Run();

            IList<string> messages = bootstrapper.BaseLogger.Messages;

            messages.ShouldContain(expectedMessageText);
        }

        [WpfFact]
        public void RunShouldLogAboutModuleCatalogCreation()
        {
            const string expectedMessageText = "Creating module catalog.";
            var bootstrapper = new DefaultMunqBootstrapper();
            bootstrapper.Run();
            IList<string> messages = bootstrapper.BaseLogger.Messages;

            messages.ShouldContain(expectedMessageText);
        }

        [WpfFact]
        public void RunShouldLogAboutConfiguringModuleCatalog()
        {
            const string expectedMessageText = "Configuring module catalog.";
            var bootstrapper = new DefaultMunqBootstrapper();
            bootstrapper.Run();
            IList<string> messages = bootstrapper.BaseLogger.Messages;

            messages.ShouldContain(expectedMessageText);
        }

        [WpfFact]
        public void RunShouldLogAboutCreatingTheContainer()
        {
            const string expectedMessageText = "Creating Munq container.";
            var bootstrapper = new DefaultMunqBootstrapper();
            bootstrapper.Run();
            IList<string> messages = bootstrapper.BaseLogger.Messages;

            messages.ShouldContain(expectedMessageText);
        }

        [WpfFact]
        public void RunShouldLogAboutConfiguringContainer()
        {
            const string expectedMessageText = "Configuring the Munq container.";
            var bootstrapper = new DefaultMunqBootstrapper();
            bootstrapper.Run();
            IList<string> messages = bootstrapper.BaseLogger.Messages;

            messages.ShouldContain(expectedMessageText);
        }

        [WpfFact]
        public void RunShouldLogAboutConfiguringViewModelLocator()
        {
            const string expectedMessageText = "Configuring the ViewModelLocator to use Munq.";
            var bootstrapper = new DefaultMunqBootstrapper();
            bootstrapper.Run();
            IList<string> messages = bootstrapper.BaseLogger.Messages;

            messages.ShouldContain(expectedMessageText);
        }

        [WpfFact]
        public void RunShouldLogAboutConfiguringRegionAdapters()
        {
            const string expectedMessageText = "Configuring region adapters.";
            var bootstrapper = new DefaultMunqBootstrapper();
            bootstrapper.Run();
            IList<string> messages = bootstrapper.BaseLogger.Messages;

            messages.ShouldContain(expectedMessageText);
        }

        [WpfFact]
        public void RunShouldLogAboutConfiguringRegionBehaviors()
        {
            const string expectedMessageText = "Configuring default region behaviors.";
            var bootstrapper = new DefaultMunqBootstrapper();
            bootstrapper.Run();
            IList<string> messages = bootstrapper.BaseLogger.Messages;

            messages.ShouldContain(expectedMessageText);
        }

        [WpfFact]
        public void RunShouldLogAboutRegisteringFrameworkExceptionTypes()
        {
            const string expectedMessageText = "Registering Framework Exception Types.";
            var bootstrapper = new DefaultMunqBootstrapper();
            bootstrapper.Run();
            IList<string> messages = bootstrapper.BaseLogger.Messages;

            messages.ShouldContain(expectedMessageText);
        }

        [WpfFact]
        public void RunShouldLogAboutCreatingTheShell()
        {
            const string expectedMessageText = "Creating the shell.";
            var bootstrapper = new DefaultMunqBootstrapper();
            bootstrapper.Run();
            IList<string> messages = bootstrapper.BaseLogger.Messages;

            messages.ShouldContain(expectedMessageText);
        }

        [WpfFact]
        public void RunShouldLogAboutInitializingTheShellIfShellCreated()
        {
            const string expectedMessageText = "Initializing the shell.";
            var bootstrapper = new DefaultMunqBootstrapper();

            bootstrapper.Run();
            IList<string> messages = bootstrapper.BaseLogger.Messages;

            messages.ShouldContain(expectedMessageText);
        }

        [WpfFact]
        public void RunShouldNotLogAboutInitializingTheShellIfShellIsNotCreated()
        {
            const string expectedMessageText = "Initializing shell";
            var bootstrapper = new DefaultMunqBootstrapper {ShellObject = null};

            bootstrapper.Run();
            IList<string> messages = bootstrapper.BaseLogger.Messages;

            messages.ShouldNotContain(expectedMessageText);
        }

        [WpfFact]
        public void RunShouldLogAboutInitializingModules()
        {
            const string expectedMessageText = "Initializing modules.";
            var bootstrapper = new DefaultMunqBootstrapper();
            bootstrapper.Run();
            IList<string> messages = bootstrapper.BaseLogger.Messages;

            messages.ShouldContain(expectedMessageText);
        }

        [WpfFact]
        public void RunShouldLogAboutRunCompleting()
        {
            const string expectedMessageText = "Bootstrapper sequence completed.";
            var bootstrapper = new DefaultMunqBootstrapper();
            bootstrapper.Run();
            IList<string> messages = bootstrapper.BaseLogger.Messages;

            messages.ShouldContain(expectedMessageText);
        }

        private static void SetupMockedContainerForVerificationTests(Mock<IIocContainer> mockedContainer)
        {
            var mockedModuleInitializer = new Mock<IModuleInitializer>();
            var mockedModuleManager = new Mock<IModuleManager>();
            var regionAdapterMappings = new RegionAdapterMappings();
            var serviceLocatorAdapter = new MunqServiceLocatorAdapter(mockedContainer.Object);
            var regionBehaviorFactory = new RegionBehaviorFactory(serviceLocatorAdapter);

            mockedContainer
                .Setup(c => c.Resolve<IServiceLocator>())
                .Returns(serviceLocatorAdapter);

            mockedContainer.Setup(c => c.RegisterInstance(It.IsAny<string>(), It.IsAny<Type>(), It.IsAny<object>()));

            mockedContainer
                .Setup(c => c.Resolve<IModuleCatalog>())
                .Returns(new ModuleCatalog());

            mockedContainer
                .Setup(c => c.Resolve<IModuleInitializer>())
                .Returns(mockedModuleInitializer.Object);

            mockedContainer
                .Setup(c => c.Resolve<IModuleManager>())
                .Returns(mockedModuleManager.Object);

            mockedContainer
                .Setup(c => c.Resolve<RegionAdapterMappings>())
                .Returns(regionAdapterMappings);

            mockedContainer
                .Setup(c => c.Resolve<SelectorRegionAdapter>())
                .Returns(new SelectorRegionAdapter(regionBehaviorFactory));

            mockedContainer.Setup(c => c.Resolve<ItemsControlRegionAdapter>())
                           .Returns(new ItemsControlRegionAdapter(regionBehaviorFactory));

            mockedContainer.Setup(c => c.Resolve<ContentControlRegionAdapter>())
                           .Returns(new ContentControlRegionAdapter(regionBehaviorFactory));
        }

        private class MockedContainerBootstrapper : MunqBootstrapper
        {
            private readonly IIocContainer _container;

            public ILoggerFacade BaseLogger => Logger;

            public MockedContainerBootstrapper(IIocContainer container)
            {
                _container = container;
            }

            protected override IIocContainer CreateContainer()
            {
                return _container;
            }

            protected override DependencyObject CreateShell()
            {
                return new UserControl();
            }

            protected override void InitializeShell()
            {
                // no op
            }
        }
    }
}