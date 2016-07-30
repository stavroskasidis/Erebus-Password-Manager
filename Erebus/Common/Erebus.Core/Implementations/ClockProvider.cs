using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Implementations
{
    public class ClockProvider : IClockProvider
    {
        public DateTime GetNow()
        {
            return DateTime.Now;
        }
    }
}
