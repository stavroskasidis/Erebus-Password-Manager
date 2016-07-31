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
        [Display(Name = nameof(StringResources.Vault), ResourceType = typeof(StringResources))]
        public string SelectedVault { get; set; }

        [LocalizedRequired]
        [DataType(DataType.Password)]
        [Display(Name = nameof(StringResources.MasterPassword), ResourceType = typeof(StringResources))]
        public string MasterPassword { get; set; }
    }
}
