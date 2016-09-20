using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Erebus.Core.Mobile.Contracts;
using Xamarin.Forms;

namespace Erebus.Mobile.Droid.PlatformImplementations
{
    public class AndroidClipboardService : IClipboardService
    {
        public void CopyToClipboard(string text)
        {
            var clipboardManager = (ClipboardManager)Forms.Context.GetSystemService(Context.ClipboardService);
            clipboardManager.Text = text;
        }
    }
}