using Erebus.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.ViewModels
{
    public class EntryGridViewModel
    {
        public string Id { get; set; }

        //[Display(Name = nameof(StringResources.Title), ResourceType = typeof(StringResources))]
        public string Title { get; set; }

        //[Display(Name = nameof(StringResources.UserName), ResourceType = typeof(StringResources))]
        public string UserName { get; set; }

        //[Display(Name = nameof(StringResources.Password), ResourceType = typeof(StringResources))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Display(Name = nameof(StringResources.Url), ResourceType = typeof(StringResources))]
        [DataType(DataType.Url)]
        public string Url { get; set; }

        //[Display(Name = nameof(StringResources.Description), ResourceType = typeof(StringResources))]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
