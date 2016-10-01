using Erebus.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.ViewModels
{
    public class VaultChangePasswordViewModel
    {
        [Required(ErrorMessageResourceName = nameof(StringResources.RequiredErrorMessage), ErrorMessageResourceType = typeof(StringResources))]
        [Display(Name = nameof(StringResources.Name), ResourceType = typeof(StringResources))]
        public string VaultName { get; set; }

        [Required(ErrorMessageResourceName = nameof(StringResources.RequiredErrorMessage), ErrorMessageResourceType = typeof(StringResources))]
        [DataType(DataType.Password)]
        [Display(Name = nameof(StringResources.MasterPassword), ResourceType = typeof(StringResources))]
        public string MasterPassword { get; set; }

        [Required(ErrorMessageResourceName = nameof(StringResources.RequiredErrorMessage), ErrorMessageResourceType = typeof(StringResources))]
        [DataType(DataType.Password)]
        [Display(Name = nameof(StringResources.NewMasterPassword), ResourceType = typeof(StringResources))]
        public string NewMasterPassword { get; set; }

        [Required(ErrorMessageResourceName = nameof(StringResources.RequiredErrorMessage), ErrorMessageResourceType = typeof(StringResources))]
        [DataType(DataType.Password)]
        [Compare(nameof(NewMasterPassword))]
        [Display(Name = nameof(StringResources.NewMasterPasswordConfirm), ResourceType = typeof(StringResources))]
        public string NewMasterPasswordConfirm { get; set; }
    }
}
