using System;
using System.Windows;
using Prism.Logging;
using Shouldly;
using Xunit;

namespace Prism.Munq.Wpf.Tests
{
    public class MunqBootstrapperNullLoggerFixture
    {
        [Fact]
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