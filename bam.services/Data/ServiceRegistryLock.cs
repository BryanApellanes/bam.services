using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Data.Repositories;

namespace Bam.CoreServices.ServiceRegistration.Data
{
    [Serializable]
    public class ServiceRegistryLock: AuditRepoData
    {
        public string Name { get; set; }
    }
}
