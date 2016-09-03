using Erebus.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.ViewModels
{
    public class VaultRenameViewModel
    {
        [Display(Name = nameof(StringResources.Name), ResourceType = typeof(StringResources))]
        public string VaultName { get; set; }

        [Required(ErrorMessageResourceName = nameof(StringResources.RequiredErrorMessage), ErrorMessageResourceType = typeof(StringResources))]
        [Display(Name = nameof(StringResources.NewName), ResourceType = typeof(StringResources))]
        public string NewVaultName { get; set; }
    }
}
