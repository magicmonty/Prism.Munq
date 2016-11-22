using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Munq;
using Prism.IocContainer.Wpf.Tests.Support.Mocks.Views;
using Prism.IocContainer.Wpf.Tests.Support.WPFHelpers;
using Prism.Regions;
using Prism.Munq.Wpf.Tests.Mocks;
using Shouldly;

namespace Prism.Munq.Wpf.Tests
{
    public class MunqRegionNavigationContentLoaderFixture
    {
        [WpfFact]
        public void ShouldFindCandidateViewInRegion()
        {
            IIocContainer container = new MunqContainerWrapper();
            container.RegisterTypeForNavigation<MockView>();

            ConfigureMockServiceLocator(container);

            // We cannot access the UnityRegionNavigationContentLoader directly so we need to call its
            // GetCandidatesFromRegion method through a navigation request.
            IRegion testRegion = new Region();

            var view = new MockView();
            testRegion.Add(view);
            testRegion.Deactivate(view);

            testRegion.RequestNavigate("MockView");

            testRegion.Views.ShouldContain(view);
            testRegion.Views.Count().ShouldBe(1);
            testRegion.ActiveViews.Count().ShouldBe(1);
            testRegion.ActiveViews.ShouldContain(view);
        }

        [WpfFact]
        public void ShouldFindCandidateViewWithFriendlyNameInRegion()
        {
            IIocContainer container = new MunqContainerWrapper();
            container.RegisterTypeForNavigation<MockView>("SomeView");

            ConfigureMockServiceLocator(container);

            // We cannot access the MunqRegionNavigationContentLoader directly so we need to call its
            // GetCandidatesFromRegion method through a navigation request.
            IRegion testRegion = new Region();

            var view = new MockView();
            testRegion.Add(view);
            testRegion.Deactivate(view);

            testRegion.RequestNavigate("SomeView");

            testRegion.Views.ShouldContain(view);
            testRegion.ActiveViews.Count().ShouldBe(1);
            testRegion.ActiveViews.ShouldContain(view);
        }

        private static void ConfigureMockServiceLocator(IDependecyRegistrar container)
        {
            var serviceLocator = new MockServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }
    }
}