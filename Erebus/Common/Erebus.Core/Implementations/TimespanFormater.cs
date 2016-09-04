using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Implementations
{
    public class TimespanFormater : ITimespanFormater
    {
        public string FormatTimespan(TimeSpan timespan)
        {
            if (timespan.Hours != 0)
            {
                return String.Format("{0:d2}:{1:d2}:{2:d2}", timespan.Hours, timespan.Minutes, timespan.Seconds);
            }
            if (timespan.Minutes != 0)
            {
                return String.Format("{0:d2}:{1:d2}", timespan.Minutes, timespan.Seconds);
            }
            return String.Format("00:{0:d2}", timespan.Seconds);
        }
    }
}
