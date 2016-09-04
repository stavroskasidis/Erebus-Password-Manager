using Erebus.Resources;
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

        [Required(ErrorMessageResourceName = nameof(StringResources.RequiredErrorMessage), ErrorMessageResourceType = typeof(StringResources))]
        [Display(Name = nameof(StringResources.Vault), ResourceType = typeof(StringResources))]
        public string SelectedVault { get; set; }

        [Required(ErrorMessageResourceName = nameof(StringResources.RequiredErrorMessage), ErrorMessageResourceType = typeof(StringResources))]
        [DataType(DataType.Password)]
        [Display(Name = nameof(StringResources.MasterPassword), ResourceType = typeof(StringResources))]
        public string MasterPassword { get; set; }

        public bool Expired { get; set; }
    }
}
