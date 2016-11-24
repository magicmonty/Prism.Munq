using System.Threading;
using NUnit.Framework;
using Prism.IocContainer.Wpf.Tests.Support.Mocks;
using Prism.IocContainer.Wpf.Tests.Support.Mocks.ViewModels;
using Prism.IocContainer.Wpf.Tests.Support.Mocks.Views;
using Prism.Mvvm;
using Shouldly;

namespace Prism.Munq.Wpf.Tests
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class MunqViewModelLocatorFixture
    {
        [Test]
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
