using Bam.Data.Repositories;
using Bam.DependencyInjection;
using Bam.Services;

namespace Bam.CoreServices.ServiceRegistration.Data
{
    /// <summary>
    /// A serializable descriptor for a ServiceRegistry
    /// </summary>
    [Serializable]
    public class ServiceRegistryDescriptor: AuditRepoData
    {
        public ServiceRegistryDescriptor() { }
        public ServiceRegistryDescriptor(string name, string description) { }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<ServiceDescriptor> Services { get; set; }

        public ServiceDescriptor AddService(Type forType, Type useType)
        {
            if(Services == null)
            {
                Services = new List<ServiceDescriptor>();
            }
            ServiceDescriptor add = new ServiceDescriptor(forType, useType);
            Services.Add(add);
            SetSequenceValues();
            return add;
        }

        public static ServiceRegistryDescriptor FromIncubator(string name, DependencyProvider incubator, string desciption = null)
        {
            ServiceRegistryDescriptor result = new ServiceRegistryDescriptor(name, desciption);
            foreach(Type type in incubator.MappedTypes)
            {
                result.AddService(type, incubator[type].GetType());
            }
            return result;
        }

        public override IRepoData Save(IRepository repo)
        {
            foreach(ServiceDescriptor svcDesc in Services)
            {
                svcDesc.Save(repo);
            }
            return base.Save(repo);
        }

        private void SetSequenceValues()
        {
            if(Services != null)
            {
                for(int i = 0; i < Services.Count; i++)
                {
                    Services[i].SequenceNumber = i + 1;
                }
            }
        }
    }
}
