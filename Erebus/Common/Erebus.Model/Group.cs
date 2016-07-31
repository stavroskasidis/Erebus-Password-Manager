using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Model
{
    public class Group : PersistableObject
    {
        public string Name { get; set; }

        public List<Group> Groups { get; set; }
        public List<Entry> Entries { get; set; }

        public Group()
        {
            this.Id = Guid.NewGuid();
            this.Groups = new List<Group>();
            this.Entries = new List<Entry>();
        }
    }
}
