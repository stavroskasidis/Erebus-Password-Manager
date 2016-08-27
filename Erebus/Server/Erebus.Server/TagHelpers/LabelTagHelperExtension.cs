using Microsoft.AspNetCore.Mvc.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Erebus.Server.TagHelpers
{
    [HtmlTargetElement("label", Attributes = AddMetadataClassesAttribute)]
    public class LabelTagHelperExtension : TagHelper
    {
        public const string AddMetadataClassesAttribute = "add-metadata-classes";

        [HtmlAttributeName(AddMetadataClassesAttribute)]
        public bool? AddMetadataClasses { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override int Order
        {
            get
            {

                return int.MinValue;
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if (AddMetadataClasses == true)
            {
                var modelExpression = context.AllAttributes["asp-for"];
                if (modelExpression != null)
                {
                    if ((modelExpression.Value as ModelExpression).ModelExplorer.Metadata.IsRequired)
                    {
                        var classAttribute = output.Attributes["class"];
                        output.Attributes.SetAttribute("class", classAttribute == null ? "required" : classAttribute.Value + " required");
                    }
                }
            }
        }
    }
}
