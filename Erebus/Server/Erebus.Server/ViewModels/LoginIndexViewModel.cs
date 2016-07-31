using Erebus.Resources;
using Erebus.Server.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.ViewModels
{
    public class LoginIndexViewModel
    {
        public IEnumerable<string> VaultNames { get; set; }

        [LocalizedRequired]
        public string SelectedVault { get; set; }

        [LocalizedRequired]
        [DataType(DataType.Password)]
        [LocalizedDisplayName(nameof(StringResources.Vault))]
        public string MasterPassword { get; set; }
    }
}
