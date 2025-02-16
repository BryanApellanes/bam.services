using System.Reflection;

namespace Bam.CoreServices.ServiceRegistration
{
    public class ServiceDefinition
    {
        public Type ForType { get; set; }
        public Assembly ForAssembly { get; set; }
        public Type UseType { get; set; }
        public Assembly UseAssembly { get; set; }
    }
}
