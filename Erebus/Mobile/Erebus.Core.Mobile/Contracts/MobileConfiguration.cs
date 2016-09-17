using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Core.Mobile.Contracts
{
    public class MobileConfiguration
    {
        public ApplicationMode ApplicationMode { get; set; }
        public string ServerUrl { get; set; }
        public string Language { get; set; }
    }
}
