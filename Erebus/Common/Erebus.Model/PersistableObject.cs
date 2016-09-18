using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Model
{
    public abstract class PersistableObject
    {
        public Guid Id { get; set; }

        public PersistableObject()
        {
            Id = Guid.NewGuid();
        }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public long Version { get; set; }
    }
}
