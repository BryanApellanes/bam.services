using Bam.Data.Repositories;

namespace Bam.CoreServices.ServiceRegistration.Data
{
    [Serializable]
    public class ServiceRegistryLock: AuditRepoData
    {
        public string Name { get; set; }
    }
}
