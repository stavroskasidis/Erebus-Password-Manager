using Erebus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Implementations
{
    internal class GetGroupResult
    {
        public Group Group { get; set; }
        public IGroupContainer Parent { get; set; }

        public GetGroupResult(Group group, IGroupContainer parent)
        {
            this.Group = group;
            this.Parent = parent;
        }
    }
}
