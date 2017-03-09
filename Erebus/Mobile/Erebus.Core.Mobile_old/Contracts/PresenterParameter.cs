using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Mobile.Contracts
{
    public class PresenterParameter
    {
        public string ParameterName { get; private set; }
        public object Value { get; private set; }

        public PresenterParameter(string parameterName, object value)
        {
            this.ParameterName = parameterName;
            this.Value = value;
        }

    }
}
