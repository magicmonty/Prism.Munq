using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Munq;
using Xunit;

namespace Prism.Munq.Wpf.Tests
{
    public class MunqServiceLocatorAdapterFixture
    {
        [Fact]
        public void ShouldForwardResolveToInnerContainer ()
        {
            object myInstance = new object ();

            IIocContainer container = new MockMunqContainer { ResolveMethod = delegate { return myInstance; } };

            IServiceLocator containerAdapter = new MunqServiceLocatorAdapter (container);

            Assert.Same (myInstance, containerAdapter.GetInstance (typeof (object)));

        }

        [Fact]
        public void ShouldForwardResolveAllToInnerContainer ()
        {
            IEnumerable<object> list = new List<object> { new object (), new object () };

            IIocContainer container = new MockMunqContainer {
                ResolveAllMethod = delegate {
                    return list;
                }
            };

            IServiceLocator containerAdapter = new MunqServiceLocatorAdapter (container);

            Assert.Same (list, containerAdapter.GetAllInstances (typeof (object)));
        }

        private class MockMunqContainer : IIocContainer
        {
            public Func<object> ResolveMethod { get; set; }

            public Func<IEnumerable<object>> ResolveAllMethod { get; set; }

            public TType Resolve<TType> () where TType : class
            {
                return (TType)ResolveMethod ();
            }

            public TType Resolve<TType> (string name) where TType : class
            {
                return (TType)ResolveMethod ();
            }

            public object Resolve (Type type)
            {
                return ResolveMethod ();
            }

            public object Resolve (string name, Type type)
            {
                return ResolveMethod ();
            }

            public IEnumerable<TType> ResolveAll<TType> () where TType : class
            {
                return (IEnumerable<TType>)ResolveAllMethod ();
            }

            public IEnumerable<object> ResolveAll (Type type)
            {
                return ResolveAllMethod ();
            }

            public Func<TType> LazyResolve<TType> () where TType : class
            {
                throw new NotImplementedException ();
            }

            public Func<TType> LazyResolve<TType> (string name) where TType : class
            {
                throw new NotImplementedException ();
            }

            public Func<object> LazyResolve (Type type)
            {
                throw new NotImplementedException ();
            }

            public Func<object> LazyResolve (string name, Type type)
            {
                throw new NotImplementedException ();
            }

            public bool CanResolve<TType> () where TType : class
            {
                throw new NotImplementedException ();
            }

            public bool CanResolve<TType> (string name) where TType : class
            {
                throw new NotImplementedException ();
            }

            public bool CanResolve (Type type)
            {
                throw new NotImplementedException ();
            }

            public bool CanResolve (string name, Type type)
            {
                throw new NotImplementedException ();
            }

            public IRegistration Register<TType> (Func<IDependencyResolver, TType> func) where TType : class
            {
                throw new NotImplementedException ();
            }

            public IRegistration Register<TType> (string name, Func<IDependencyResolver, TType> func) where TType : class
            {
                throw new NotImplementedException ();
            }

            public IRegistration Register (Type type, Func<IDependencyResolver, object> func)
            {
                throw new NotImplementedException ();
            }

            public IRegistration Register (string name, Type type, Func<IDependencyResolver, object> func)
            {
                throw new NotImplementedException ();
            }

            public IRegistration RegisterInstance<TType> (TType instance) where TType : class
            {
                throw new NotImplementedException ();
            }

            public IRegistration RegisterInstance<TType> (string name, TType instance) where TType : class
            {
                throw new NotImplementedException ();
            }

            public IRegistration RegisterInstance (Type type, object instance)
            {
                throw new NotImplementedException ();
            }

            public IRegistration RegisterInstance (string name, Type type, object instance)
            {
                throw new NotImplementedException ();
            }

            IRegistration IDependecyRegistrar.Register<TType, TImpl> ()
            {
                throw new NotImplementedException ();
            }

            IRegistration IDependecyRegistrar.Register<TType, TImpl> (string name)
            {
                throw new NotImplementedException ();
            }

            public IRegistration Register (Type tType, Type tImpl)
            {
                throw new NotImplementedException ();
            }

            public IRegistration Register (string name, Type tType, Type tImpl)
            {
                throw new NotImplementedException ();
            }

            public void Remove (IRegistration ireg)
            {
                throw new NotImplementedException ();
            }

            public IRegistration GetRegistration<TType> () where TType : class
            {
                throw new NotImplementedException ();
            }

            public IRegistration GetRegistration<TType> (string name) where TType : class
            {
                throw new NotImplementedException ();
            }

            public IRegistration GetRegistration (Type type)
            {
                throw new NotImplementedException ();
            }

            public IRegistration GetRegistration (string name, Type type)
            {
                throw new NotImplementedException ();
            }

            public IEnumerable<IRegistration> GetRegistrations<TType> () where TType : class
            {
                throw new NotImplementedException ();
            }

            public IEnumerable<IRegistration> GetRegistrations (Type type)
            {
                throw new NotImplementedException ();
            }

            public ILifetimeManager DefaultLifetimeManager {
                get {
                    throw new NotImplementedException ();
                }

                set {
                    throw new NotImplementedException ();
                }
            }
        }
    }
}