using Erebus.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.Localization
{
    public class LocalizedRequiredAttribute : RequiredAttribute
    {
        public LocalizedRequiredAttribute()
        {
            this.ErrorMessageResourceType = typeof(StringResources);
            this.ErrorMessageResourceName = nameof(StringResources.RequiredErrorMessage);
        }
    }
}
