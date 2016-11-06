using Prism.IocContainer.Wpf.Tests.Support.Mocks;
using Prism.IocContainer.Wpf.Tests.Support.Mocks.ViewModels;
using Prism.IocContainer.Wpf.Tests.Support.Mocks.Views;
using Prism.Mvvm;
using Xunit;

namespace Prism.Munq.Wpf.Tests
{
    public class MunqViewModelLocatorFixture
    {
        [Fact]
        public void ShouldLocateViewModelAndResolveWithContainer ()
        {
            var bootstrapper = new DefaultMunqBootstrapper ();
            bootstrapper.Run ();

            bootstrapper.BaseContainer.Register<IService, MockService> ();

            MockView view = new MockView ();
            Assert.Null (view.DataContext);

            ViewModelLocator.SetAutoWireViewModel (view, true);
            Assert.NotNull (view.DataContext);
            Assert.IsType (typeof (MockViewModel), view.DataContext);

            Assert.NotNull (((MockViewModel)view.DataContext).MockService);
        }
    }
}
