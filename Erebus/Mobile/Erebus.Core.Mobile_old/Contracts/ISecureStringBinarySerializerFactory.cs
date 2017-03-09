using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Contracts
{
    public interface ISecureStringBinarySerializerFactory
    {
        ISecureStringBinarySerializer CreateInstance();
    }
}
