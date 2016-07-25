using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Model
{
    public class VaultFolder : PersistableObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<VaultFolder> SubFolders { get; set; }
        public virtual Vault Vault { get; set; }
        public virtual VaultFolder ParentFolder { get; set; }
    }
}
