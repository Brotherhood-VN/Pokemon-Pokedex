namespace API.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class DependencyInjectionAttribute : Attribute
    {
        public readonly ServiceLifetime ServiceLifetime;

        public DependencyInjectionAttribute(ServiceLifetime serviceLifetime)
        {
            ServiceLifetime = serviceLifetime;
        }
    }
}