using Microsoft.AspNetCore.Mvc.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.IO;

namespace Erebus.Server.TagHelpers
{
    [HtmlTargetElement("link", Attributes = MinifiedAutoSwitchAttributeName)]
    public class MinifiedAutoSwitchLinkTagHelper : MinifiedAutoSwitchTagHelper
    {
        public MinifiedAutoSwitchLinkTagHelper(IHostingEnvironment hostingEnviroment) : base(hostingEnviroment)
        {
        }

        protected override string AffectedAttribute
        {
            get
            {
                return "href";
            }
        }
    }
}
