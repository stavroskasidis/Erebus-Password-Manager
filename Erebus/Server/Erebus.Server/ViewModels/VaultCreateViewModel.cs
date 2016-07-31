using Erebus.Resources;
using Erebus.Server.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.ViewModels
{
    public class VaultCreateViewModel
    {
        [LocalizedRequired]
        [LocalizedDisplayName(nameof(StringResources.Name))]
        public string VaultName { get; set; }

        [LocalizedRequired]
        [DataType(DataType.Password)]
        [LocalizedDisplayName(nameof(StringResources.MasterPassword))]
        public string MasterPassword { get; set; }

        [LocalizedRequired]
        [DataType(DataType.Password)]
        [Compare(nameof(MasterPassword))]
        [LocalizedDisplayName(nameof(StringResources.MasterPasswordConfirm))]
        public string MasterPasswordConfirm { get; set; }
    }
}
