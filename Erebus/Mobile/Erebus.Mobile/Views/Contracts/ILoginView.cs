using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Mobile.Views.Contracts
{
    public interface ILoginView
    {
        string SelectedVaultName { get; set;}
        IEnumerable<string> VaultNames { set; }
        string MasterPasswordInputText { get; }
        string ActivityIndicatorText { set; }
        void ShowLoadingIndicator();
        void DisableUI();
        void HideLoadingIndicator();
        void EnableUI();
        bool SyncButtonVisible { set; }
        event Action Login;
        event Action NavigateToConfiguration;
        event Action Sync;
    }
}
