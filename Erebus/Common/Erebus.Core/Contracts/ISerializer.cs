using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Contracts
{
    public interface ISerializer
    {
        T Deserialize<T>(string serializedData);
        string Serialize(object obj);
    }
}
