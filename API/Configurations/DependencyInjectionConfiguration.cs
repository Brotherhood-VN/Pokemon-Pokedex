using System.Reflection;
using API.Helpers.Attributes;

namespace API.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        /// <summary>
        /// Add the services which has been registered with annotation to the DI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="targetProject"></param>
        public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services, Type targetProject)
        {
            var implementedTypes = GetTypesWith<DependencyInjectionAttribute>(true);

            foreach (var implementedType in implementedTypes)
            {
                var types = targetProject.Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(implementedType));

                foreach (var type in types)
                {
                    var attribute = implementedType.GetCustomAttribute<DependencyInjectionAttribute>();
                    if (attribute != null)
                        services.Add(new ServiceDescriptor(implementedType, type, attribute.ServiceLifetime));
                }
            }
            return services;
        }

        private static IEnumerable<Type> GetTypesWith<TAttribute>(bool inherit) where TAttribute : Attribute
        {
            return from a in AppDomain.CurrentDomain.GetAssemblies()
                   from t in a.GetTypes()
                   where t.IsDefined(typeof(TAttribute), inherit)
                   select t;
        }
    }
}