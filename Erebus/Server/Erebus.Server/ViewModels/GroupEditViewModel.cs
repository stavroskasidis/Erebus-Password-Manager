using Erebus.Resources;
using Erebus.Server.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.ViewModels
{
    public class GroupEditViewModel
    {
        public string Id { get; set; }

        public string ParentId { get; set; }

        [LocalizedRequired]
        [Display(Name = nameof(StringResources.Name), ResourceType = typeof(StringResources))]
        public string Name { get; set; }
    }
}
