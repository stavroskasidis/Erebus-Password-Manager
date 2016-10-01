using Autofac;
using Erebus.Core.Mobile;
using Erebus.Core.Mobile.Contracts;
using Erebus.Mobile.UWP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Erebus.Mobile.UWP.Tasks
{
        public sealed class SynchronizationBackgroundTask : IBackgroundTask
        {
            private BackgroundTaskDeferral Deferral;

            public async void Run(IBackgroundTaskInstance taskInstance)
            {
                try
                {
                    Deferral = taskInstance.GetDeferral();
                    var containerFactory = new ContainerFactory();
                    containerFactory.AddRegistrations(builder =>
                    {
                        var uwpRegistrations = new UWPPlatformServicesRegistrator();
                        uwpRegistrations.RegisterPlatformSpecificServices(builder);
                    });

                    var container = containerFactory.Build();
                    var synchronizer = container.Resolve<ISynchronizer>();

                    var serverOnline = await synchronizer.Synchronize();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.ToString());
                }
                finally
                {
                    Deferral.Complete();
                }
            }
    }
}
