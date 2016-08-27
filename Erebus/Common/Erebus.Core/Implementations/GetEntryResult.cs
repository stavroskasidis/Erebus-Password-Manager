using Erebus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Implementations
{
    internal class GetEntryResult
    {
        public Entry Entry { get; set; }
        public Group Parent { get; set; }

        public GetEntryResult(Entry entry, Group parent)
        {
            this.Entry = entry;
            this.Parent = parent;
        }

    }
}
