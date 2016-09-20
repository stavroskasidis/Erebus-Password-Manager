using Erebus.Core;
using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Erebus.Core.Mobile.Contracts;
using UIKit;

namespace Erebus.Mobile.iOS.PlatformImplementations
{
    public class iOSClipboardService : IClipboardService
    {
        public void CopyToClipboard(string text)
        {
            UIPasteboard clipboard = UIPasteboard.General;
            clipboard.String = text;
        }
    }

}
