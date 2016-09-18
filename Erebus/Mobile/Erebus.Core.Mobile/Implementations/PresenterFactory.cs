using Autofac;
using Erebus.Core.Mobile.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Mobile.Implementations
{
    public class PresenterFactory : IPresenterFactory
    {
        private IContainer Container;

        public PresenterFactory(IContainer container)
        {
            this.Container = container;
        }


        public TPresenter Create<TPresenter>() where TPresenter : IPresenter
        {
            return Container.Resolve<TPresenter>();
        }
    }
}
