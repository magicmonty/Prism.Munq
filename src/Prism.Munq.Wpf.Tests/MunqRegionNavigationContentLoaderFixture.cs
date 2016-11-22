using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Prism.IocContainer.Wpf.Tests.Support.Mocks.Views;
using Prism.IocContainer.Wpf.Tests.Support.WPFHelpers;
using Prism.Regions;
using Prism.Munq.Wpf.Tests.Mocks;
using Xunit;

namespace Prism.Munq.Wpf.Tests
{
    public class MunqRegionNavigationContentLoaderFixture
    {
        [WpfFact]
        public void ShouldFindCandidateViewInRegion ()
        {
            IIocContainer container = new MunqContainerWrapper ();
            container.RegisterTypeForNavigation<MockView> ();

            ConfigureMockServiceLocator (container);

            // We cannot access the UnityRegionNavigationContentLoader directly so we need to call its
            // GetCandidatesFromRegion method through a navigation request.
            IRegion testRegion = new Region ();

            MockView view = new MockView ();
            testRegion.Add (view);
            testRegion.Deactivate (view);

            testRegion.RequestNavigate ("MockView");

            Assert.True (testRegion.Views.Contains (view));
            Assert.True (testRegion.Views.Count () == 1);
            Assert.True (testRegion.ActiveViews.Count () == 1);
            Assert.True (testRegion.ActiveViews.Contains (view));
        }

        [WpfFact]
        public void ShouldFindCandidateViewWithFriendlyNameInRegion ()
        {
            IIocContainer container = new MunqContainerWrapper ();
            container.RegisterTypeForNavigation<MockView> ("SomeView");

            ConfigureMockServiceLocator (container);

            // We cannot access the UnityRegionNavigationContentLoader directly so we need to call its
            // GetCandidatesFromRegion method through a navigation request.
            IRegion testRegion = new Region ();

            MockView view = new MockView ();
            testRegion.Add (view);
            testRegion.Deactivate (view);

            testRegion.RequestNavigate ("SomeView");

            Assert.True (testRegion.Views.Contains (view));
            Assert.True (testRegion.ActiveViews.Count () == 1);
            Assert.True (testRegion.ActiveViews.Contains (view));
        }

        public void ConfigureMockServiceLocator (IIocContainer container)
        {
            MockServiceLocator serviceLocator = new MockServiceLocator (container);
            ServiceLocator.SetLocatorProvider (() => serviceLocator);
        }
    }
}
