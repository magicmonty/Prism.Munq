using System;
using System.Windows;
using NUnit.Framework;
using Prism.Logging;
using Shouldly;

namespace Prism.Munq.Wpf.Tests
{
    [TestFixture]
    public class MunqBootstrapperNullLoggerFixture
    {
        [Test]
        public void NullLoggerThrows()
        {
            var bootstrapper = new NullLoggerBootstrapper();

            Should
                .Throw<InvalidOperationException>(() => bootstrapper.Run())
                .Message.ShouldContain("ILoggerFacade");
        }

        private class NullLoggerBootstrapper : MunqBootstrapper
        {
            protected override ILoggerFacade CreateLogger()
            {
                return null;
            }

            protected override DependencyObject CreateShell()
            {
                throw new NotImplementedException();
            }

            protected override void InitializeShell()
            {
                throw new NotImplementedException();
            }
        }
    }
}
