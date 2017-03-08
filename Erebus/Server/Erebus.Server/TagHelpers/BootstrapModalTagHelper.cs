using Erebus.Localization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.TagHelpers
{

    public class ModalContext
    {
        public IHtmlContent Body { get; set; }
        public IHtmlContent Footer { get; set; }
    }

    /// <summary>
    /// A Bootstrap modal dialog
    /// </summary>
    [RestrictChildren("modal-body", "modal-footer")]
    public class BootstrapModalTagHelper : TagHelper
    {
        /// <summary>
        /// The title of the modal
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Id of the modal
        /// </summary>
        public string Id { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var modalContext = new ModalContext();
            context.Items.Add(typeof(BootstrapModalTagHelper), modalContext);

            await output.GetChildContentAsync();

            var template =
$@"<div class='modal-dialog' role='document'>
    <div class='modal-content'>
      <div class='modal-header'>
        <button type = 'button' class='close' data-dismiss='modal' aria-label='Close'><span aria-hidden='true'>&times;</span></button>
        <h4 class='modal-title' id='{context.UniqueId}Label'>{Title}</h4>
      </div>
        <div class='modal-body'>";

            output.TagName = "div";
            output.Attributes.SetAttribute("role", "dialog");
            output.Attributes.SetAttribute("id", Id);
            output.Attributes.SetAttribute("aria-labelledby", $"{context.UniqueId}Label");
            output.Attributes.SetAttribute("tabindex", "-1");
            var classNames = "modal fade";
            if (output.Attributes.ContainsName("class"))
            {
                classNames = string.Format("{0} {1}", output.Attributes["class"].Value, classNames);
            }
            output.Attributes.SetAttribute("class", classNames);
            output.Content.AppendHtml(template);
            if (modalContext.Body != null)
            {
                output.Content.AppendHtml(modalContext.Body);
            }
            output.Content.AppendHtml("</div>");
            if (modalContext.Footer != null)
            {
                output.Content.AppendHtml("<div class='modal-footer'>");
                output.Content.AppendHtml(modalContext.Footer);
                output.Content.AppendHtml("</div>");
            }

            output.Content.AppendHtml("</div></div>");
        }
    }



    /// <summary>
    /// The modal-body portion of a Bootstrap modal dialog
    /// </summary>
    [HtmlTargetElement("modal-body", ParentTag = "bootstrap-modal")]
    public class ModalBodyTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            var modalContext = (ModalContext)context.Items[typeof(BootstrapModalTagHelper)];
            modalContext.Body = childContent;
            output.SuppressOutput();
        }
    }

    /// <summary>
    /// The modal-footer portion of Bootstrap modal dialog
    /// </summary>
    [HtmlTargetElement("modal-footer", ParentTag = "bootstrap-modal")]
    public class ModalFooterTagHelper : TagHelper
    {
        /// <summary>
        /// Whether or not to show a button to dismiss the dialog. 
        /// Default: <c>true</c>
        /// </summary>
        public bool ShowDismiss { get; set; } = true;

        /// <summary>
        /// The text to show on the Dismiss button
        /// Default: Cancel
        /// </summary>
        public string DismissText { get; set; } = StringResources.Cancel;


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (ShowDismiss)
            {
                output.PreContent.AppendFormat(@"<button type='button' class='btn btn-default' data-dismiss='modal'>{0}</button>", DismissText);
            }
            var childContent = await output.GetChildContentAsync();
            var footerContent = new DefaultTagHelperContent();
            if (ShowDismiss)
            {
                footerContent.AppendFormat(@"<button type='button' class='btn btn-default' data-dismiss='modal'>{0}</button>", DismissText);
            }
            footerContent.AppendHtml(childContent);
            var modalContext = (ModalContext)context.Items[typeof(BootstrapModalTagHelper)];
            modalContext.Footer = footerContent;
            output.SuppressOutput();
        }
    }


}
