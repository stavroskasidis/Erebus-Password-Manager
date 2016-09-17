using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Core.Mobile.Contracts
{
    public interface IMobileConfigurationReader
    {
        MobileConfiguration GetConfiguration();
    }
}
