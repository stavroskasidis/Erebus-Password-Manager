using Erebus.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.ViewModels
{
    public class GeneratePasswordViewModel
    {
        public string Id { get; set; }

        public string ModalTitle { get; set; }

        [Display(Name = nameof(StringResources.PasswordLength), ResourceType = typeof(StringResources))]
        [Required(ErrorMessageResourceName = nameof(StringResources.RequiredErrorMessage), ErrorMessageResourceType = typeof(StringResources))]
        [Range(4, int.MaxValue, ErrorMessageResourceName = nameof(StringResources.MinimumPasswordLengthWarning), ErrorMessageResourceType = typeof(StringResources))]
        public int PasswordLength { get; set; }

        [Display(Name = nameof(StringResources.IncludeUpperCase), ResourceType = typeof(StringResources))]
        public bool IncludeUpperCase { get; set; }

        [Display(Name = nameof(StringResources.IncludeLowerCase), ResourceType = typeof(StringResources))]
        public bool IncludeLowerCase { get; set; }

        [Display(Name = nameof(StringResources.IncludeDigits), ResourceType = typeof(StringResources))]
        public bool IncludeDigits { get; set; }

        [Display(Name = nameof(StringResources.IncludeSymbols), ResourceType = typeof(StringResources))]
        public bool IncludeSymbols { get; set; }
        

    }
}
