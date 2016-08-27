using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Model
{
    public class Vault : PersistableObject, IGroupContainer
    {
        public string Name { get; set; }
        public List<Group> Groups { get; set; }

        public Vault()
        {
            this.Id = Guid.NewGuid();
            this.Groups = new List<Group>();
        }
    }
}
