using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Erebus.Core.Mobile;
using Android.Content;

namespace Erebus.Mobile.Droid
{
    [Activity(Label = "Erebus.Mobile", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            //this.StartService(new Android.Content.Intent())
            global::Xamarin.Forms.Forms.Init(this, bundle);
            
            LoadApplication(new App(new ContainerFactory()));
        }
    }
}

