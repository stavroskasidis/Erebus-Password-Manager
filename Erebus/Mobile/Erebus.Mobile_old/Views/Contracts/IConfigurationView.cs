using Erebus.Core.Mobile;
using Erebus.Mobile.Presenters.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Mobile.Views.Contracts
{
    public interface IConfigurationView
    {
        IEnumerable<ApplicationMode> ApplicationModes { set; }
        ApplicationMode SelectedApplicationMode { get; set; }
        string ServerUrlInputText { get; set; }
        bool ServerUrlInputEnabled { get; set; }
        event Action Save;
        event Action ApplicationModeChange;
    }
}
