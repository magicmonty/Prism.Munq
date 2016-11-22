using System;
using System.Windows;
using Prism.IocContainer.Wpf.Tests.Support.WPFHelpers;
using Shouldly;

namespace Prism.Munq.Wpf.Tests
{
    public class MunqBootstrapperNullContainerFixture
    {
        [WpfFact]
        public void RunThrowsWhenNullContainerCreated ()
        {
            var bootstrapper = new NullContainerBootstrapper();

            Should
                .Throw<InvalidOperationException>(() => bootstrapper.Run())
                .Message.ShouldContain("IocContainer");
        }

        private class NullContainerBootstrapper : MunqBootstrapper
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            protected override IIocContainer CreateContainer () => null;

            protected override DependencyObject CreateShell ()
            {
                throw new NotImplementedException ();
            }

            protected override void InitializeShell ()
            {
                throw new NotImplementedException ();
            }
        }
    }
}