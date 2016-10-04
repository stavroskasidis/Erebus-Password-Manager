using Erebus.Core.Mobile.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace Erebus.Mobile.iOS.PlatformImplementations
{
    public class iOSSynchronizationServiceManager : ISynchronizationServiceManager
    {
        public void StartSynchronizationService()
        {
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);
        }

        public void StopSynchronizationService()
        {

            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalNever);
        }
    }
}