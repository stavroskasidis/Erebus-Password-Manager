using Erebus.Core.Mobile.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Mobile.Implementations
{
    public class UrlValidator : IUrlValidator
    {
        public bool IsUrlValid(string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result) && (result.Scheme == "http" || result.Scheme == "https");
        }
    }
}
