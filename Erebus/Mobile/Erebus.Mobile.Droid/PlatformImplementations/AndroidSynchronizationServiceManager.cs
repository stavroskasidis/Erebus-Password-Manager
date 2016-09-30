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

namespace Erebus.Mobile.Droid.PlatformImplementations
{
    public class AndroidSynchronizationServiceManager : ISynchronizationServiceManager
    {
        private IMobileConfigurationReader MobileConfigurationReader;

        public AndroidSynchronizationServiceManager(IMobileConfigurationReader mobileConfigurationReader)
        {
            this.MobileConfigurationReader = mobileConfigurationReader;
        }

        public void StartSynchronizationService()
        {
            var alarmMgr = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
            var intent = new Intent(Application.Context, typeof(SynchronizationService));
            var alarmIntent = PendingIntent.GetService(Application.Context,0 , intent, PendingIntentFlags.UpdateCurrent);
            
            alarmMgr.Cancel(alarmIntent);
            alarmMgr.SetInexactRepeating(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + 5 * 1000, AlarmManager.IntervalHour, alarmIntent);
            //Application.Context.StartService());
        }

        public void StopSynchronizationService()
        {
            var alarmMgr = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
            var alarmIntent = PendingIntent.GetService(Application.Context, 0, new Intent(Application.Context, typeof(SynchronizationService)), PendingIntentFlags.UpdateCurrent);
            alarmMgr.Cancel(alarmIntent);
        }
    }
}