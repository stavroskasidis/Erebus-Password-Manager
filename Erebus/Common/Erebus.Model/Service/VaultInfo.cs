using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Model.Service
{
    public class VaultInfo
    {
        public string VaultName { get; set; }
        public VaultCreateLocation CreateLocation { get; set; }
        public long Version { get; set; }
    }
}
