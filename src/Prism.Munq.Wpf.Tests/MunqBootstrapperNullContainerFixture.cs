using System;
using System.Windows;
using NUnit.Framework;
using Shouldly;

namespace Prism.Munq.Wpf.Tests
{
    [TestFixture]
    public class MunqBootstrapperNullContainerFixture
    {
        [Test]
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
