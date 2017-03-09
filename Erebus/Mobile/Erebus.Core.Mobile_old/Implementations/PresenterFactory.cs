using Autofac;
using Autofac.Core;
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


        public TPresenter Create<TPresenter>(params PresenterParameter[] parameters) where TPresenter : IPresenter
        {
			return parameters == null ? 
				Container.Resolve<TPresenter>() : 
				Container.Resolve<TPresenter>(parameters.Select(x=> new NamedParameter(x.ParameterName, x.Value)));
        }
    }
}
