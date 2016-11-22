using System;
using System.Collections.Generic;
using Munq;

namespace Prism.Munq
{
    public class MunqContainerWrapper : IIocContainer
    {
        private readonly IocContainer _baseContainer;

        public MunqContainerWrapper()
        {
            _baseContainer = new IocContainer();
            _baseContainer.RegisterInstance<IIocContainer>(this);
            _baseContainer.RegisterInstance<IDependecyRegistrar>(this);
            _baseContainer.RegisterInstance<IDependencyResolver>(this);
        }

        public IRegistration Register<TType>(Func<IDependencyResolver, TType> func) where TType : class
        {
            return _baseContainer.Register(func);
        }

        public IRegistration Register<TType>(string name, Func<IDependencyResolver, TType> func) where TType : class
        {
            return _baseContainer.Register(name, func);
        }

        public IRegistration Register(Type type, Func<IDependencyResolver, object> func)
        {
            return _baseContainer.Register(type, func);
        }

        public IRegistration Register(string name, Type type, Func<IDependencyResolver, object> func)
        {
            return _baseContainer.Register(name, type, func);
        }

        public IRegistration RegisterInstance<TType>(TType instance) where TType : class
        {
            return _baseContainer.RegisterInstance(instance);
        }

        public IRegistration RegisterInstance<TType>(string name, TType instance) where TType : class
        {
            return _baseContainer.RegisterInstance(name, instance);
        }

        public IRegistration RegisterInstance(Type type, object instance)
        {
            return _baseContainer.RegisterInstance(type, instance);
        }

        public IRegistration RegisterInstance(string name, Type type, object instance)
        {
            return _baseContainer.RegisterInstance(name, type, instance);
        }

        public IRegistration Register<TType, TImpl>() where TType : class where TImpl : class, TType
        {
            return _baseContainer.Register<TType, TImpl>();
        }

        public IRegistration Register<TType, TImpl>(string name) where TType : class where TImpl : class, TType
        {
            return _baseContainer.Register<TType, TImpl>(name);
        }

        public IRegistration Register(Type tType, Type tImpl)
        {
            return _baseContainer.Register(tType, tImpl);
        }

        public IRegistration Register(string name, Type tType, Type tImpl)
        {
            return _baseContainer.Register(name, tType, tImpl);
        }

        public void Remove(IRegistration ireg)
        {
            _baseContainer.Remove(ireg);
        }

        public IRegistration GetRegistration<TType>() where TType : class
        {
            return _baseContainer.GetRegistration<TType>();
        }

        public IRegistration GetRegistration<TType>(string name) where TType : class
        {
            return _baseContainer.GetRegistration<TType>(name);
        }

        public IRegistration GetRegistration(Type type)
        {
            return _baseContainer.GetRegistration(type);
        }

        public IRegistration GetRegistration(string name, Type type)
        {
            return _baseContainer.GetRegistration(name, type);
        }

        public IEnumerable<IRegistration> GetRegistrations<TType>() where TType : class
        {
            return _baseContainer.GetRegistrations<TType>();
        }

        public IEnumerable<IRegistration> GetRegistrations(Type type)
        {
            return _baseContainer.GetRegistrations(type);
        }

        public ILifetimeManager DefaultLifetimeManager { get; set; }

        public TType Resolve<TType>() where TType : class
        {
            return _baseContainer.Resolve<TType>();
        }

        public TType Resolve<TType>(string name) where TType : class
        {
            return _baseContainer.Resolve<TType>(name);
        }

        public object Resolve(Type type)
        {
            return _baseContainer.Resolve(type);
        }

        public object Resolve(string name, Type type)
        {
            return _baseContainer.Resolve(name, type);
        }

        public IEnumerable<TType> ResolveAll<TType>() where TType : class
        {
            return _baseContainer.ResolveAll<TType>();
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            return _baseContainer.ResolveAll(type);
        }

        public Func<TType> LazyResolve<TType>() where TType : class
        {
            return _baseContainer.LazyResolve<TType>();
        }

        public Func<TType> LazyResolve<TType>(string name) where TType : class
        {
            return _baseContainer.LazyResolve<TType>(name);
        }

        public Func<object> LazyResolve(Type type)
        {
            return _baseContainer.LazyResolve(type);
        }

        public Func<object> LazyResolve(string name, Type type)
        {
            return _baseContainer.LazyResolve(name, type);
        }

        public bool CanResolve<TType>() where TType : class
        {
            return _baseContainer.CanResolve<TType>();
        }

        public bool CanResolve<TType>(string name) where TType : class
        {
            return _baseContainer.CanResolve<TType>(name);
        }

        public bool CanResolve(Type type)
        {
            return _baseContainer.CanResolve(type);
        }

        public bool CanResolve(string name, Type type)
        {
            return _baseContainer.CanResolve(name, type);
        }
    }
}