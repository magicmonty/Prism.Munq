using Munq;

namespace Prism.Munq
{
    /// <summary>
    /// Wrapper interface for the <see cref="IocContainer"/>
    /// </summary>
    public interface IIocContainer : IDependecyRegistrar, IDependencyResolver { }
}