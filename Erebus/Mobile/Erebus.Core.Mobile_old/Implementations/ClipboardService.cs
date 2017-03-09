using Erebus.Core.Mobile.Contracts;
using Plugin.Clipboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Mobile.Implementations
{
    public class ClipboardService : IClipboardService
    {
        public void CopyToClipboard(string text)
        {
            CrossClipboard.Current.SetText(text);
        }
    }
}
