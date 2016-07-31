using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.TagHelpers
{
    [HtmlTargetElement("script", Attributes = MinifiedAutoSwitchAttributeName)]
    public class MinifiedAutoSwitchScriptTagHelper : MinifiedAutoSwitchTagHelper
    {
        public MinifiedAutoSwitchScriptTagHelper(IHostingEnvironment hostingEnviroment) : base(hostingEnviroment)
        {
        }

        protected override string AffectedAttribute
        {
            get
            {
                return "src";
            }
        }
    }
}
