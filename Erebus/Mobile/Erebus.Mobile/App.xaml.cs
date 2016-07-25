﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Erebus.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //PCLTest.Class1 test = new PCLTest.Class1();
            PCLTest2.Class1 test2 = new PCLTest2.Class1();
            
            MainPage = new Erebus.Mobile.MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
