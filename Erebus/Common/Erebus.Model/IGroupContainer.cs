using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Model
{
    public interface IGroupContainer
    {
        List<Group> Groups { get; set; }
    }
}
