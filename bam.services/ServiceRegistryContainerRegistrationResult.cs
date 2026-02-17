using System.Reflection;
using Bam.DependencyInjection;
using Bam.Services;

namespace Bam.CoreServices.ServiceRegistration
{
    /// <summary>
    /// A class providing meta data about a service registry container type.
    /// </summary>
    public class ServiceRegistryContainerRegistrationResult
    {
        public ServiceRegistryContainerRegistrationResult(string name, ServiceRegistry registry, Type type, MethodInfo method, ServiceRegistryLoaderAttribute attr)
        {
            Success = true;
            Type = type;
            MethodInfo = method;
            Attribute = attr;
            ServiceRegistry = registry;
            Name = name;
        }

        public ServiceRegistryContainerRegistrationResult(Exception ex)
        {
            Exception = ex;
            Success = false;
        }

        public string Name { get; set; } = null!;
        public bool Success { get; set; }
        public Exception Exception { get; set; } = null!;
        public ServiceRegistryLoaderAttribute Attribute { get; set; } = null!;
        public MethodInfo MethodInfo { get; set; } = null!;
        public Type Type { get; set; } = null!;
        public ServiceRegistry ServiceRegistry { get; set; } = null!;
    }
}
