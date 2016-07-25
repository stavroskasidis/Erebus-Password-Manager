using Erebus.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Model
{
    public abstract class PersistableObject : IPersistableObject
    {
        public Guid ID { get; set; }

        public PersistableObject()
        {
            ID = Guid.NewGuid();
        }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
