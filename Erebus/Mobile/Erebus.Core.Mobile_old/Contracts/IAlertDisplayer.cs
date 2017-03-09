using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Mobile.Contracts
{
    public interface IAlertDisplayer
    {
        void DisplayAlert(string title, string message);
    }
}
