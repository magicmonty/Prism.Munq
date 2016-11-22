using JetBrains.Annotations;
using Munq;

namespace Prism.Munq
{
    public static class MunqExtensions
    {
        /// <summary>
        /// Registers an object for navigation.
        /// </summary>
        /// <typeparam name="T">The Type of the object to register</typeparam>
        /// <param name="container"><see cref="IDependecyRegistrar"/> used to register type for Navigation.</param>
        /// <param name="name">The unique name to register with the object.</param>
        [NotNull, PublicAPI]
        public static IDependecyRegistrar RegisterTypeForNavigation<T>([NotNull] this IDependecyRegistrar container, string name = null)
        {
            var type = typeof(T);
            var viewName = string.IsNullOrWhiteSpace(name) ? type.Name : name;

            container.Register(viewName, typeof(object), type);

            return container;
        }
    }
}
