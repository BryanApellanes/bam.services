using System.Reflection;

namespace Bam.CoreServices.ServiceRegistration
{
    public class ServiceDefinition
    {
        public Type ForType { get; set; } = null!;
        public Assembly ForAssembly { get; set; } = null!;
        public Type UseType { get; set; } = null!;
        public Assembly UseAssembly { get; set; } = null!;
    }
}
