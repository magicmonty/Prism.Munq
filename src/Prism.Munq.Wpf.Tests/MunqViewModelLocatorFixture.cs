using Prism.IocContainer.Wpf.Tests.Support.Mocks;
using Prism.IocContainer.Wpf.Tests.Support.Mocks.ViewModels;
using Prism.IocContainer.Wpf.Tests.Support.Mocks.Views;
using Prism.IocContainer.Wpf.Tests.Support.WPFHelpers;
using Prism.Mvvm;
using Shouldly;

namespace Prism.Munq.Wpf.Tests
{
    public class MunqViewModelLocatorFixture
    {
        [WpfFact]
        public void ShouldLocateViewModelAndResolveWithContainer()
        {
            var bootstrapper = new DefaultMunqBootstrapper();
            bootstrapper.Run();

            bootstrapper.BaseContainer.Register<IService, MockService>();

            var view = new MockView();
            view.DataContext.ShouldBeNull();

            ViewModelLocator.SetAutoWireViewModel(view, value: true);

            view.DataContext.ShouldNotBeNull();
            view.DataContext.ShouldBeOfType<MockViewModel>();
            ((MockViewModel) view.DataContext).MockService.ShouldNotBeNull();
        }
    }
}