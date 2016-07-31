using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.TagHelpers
{
    public abstract class MinifiedAutoSwitchTagHelper : TagHelper
    {
        public const string MinifiedAutoSwitchAttributeName = "minified-auto-switch";

        private IHostingEnvironment HostingEnviroment;

        protected abstract string AffectedAttribute { get; }

        public MinifiedAutoSwitchTagHelper(IHostingEnvironment hostingEnviroment)
        {
            HostingEnviroment = hostingEnviroment;
        }

        public override int Order
        {
            get
            {
                return int.MinValue;
            }
        }

        /// <summary>
        /// Value indicating if the file path should automatically switch to its' normal or its' minified version, depending on the enviroment (Development: normal, 
        /// Staging,Production: minified)
        /// </summary>
        [HtmlAttributeName(MinifiedAutoSwitchAttributeName)]
        public bool? MinifiedAutoSwitch { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagHelperAttribute originalAttribute;
            if (MinifiedAutoSwitch.HasValue && MinifiedAutoSwitch.Value && context.AllAttributes.TryGetAttribute(AffectedAttribute, out originalAttribute) && originalAttribute.Value != null)
            {
                string attributeValue = originalAttribute.Value.ToString();
                var extension = Path.GetExtension(attributeValue);
                if (HostingEnviroment.IsDevelopment())
                {
                    if (attributeValue.EndsWith(".min" + extension))
                    {
                        attributeValue = attributeValue.Replace(".min" + extension, extension);
                    }
                }
                else
                {
                    if (attributeValue.EndsWith(".min" + extension) == false)
                    {
                        attributeValue = attributeValue.Replace(extension, ".min" + extension);
                    }
                }

                output.Attributes.SetAttribute(AffectedAttribute, attributeValue);
            }
        }
    }
}
