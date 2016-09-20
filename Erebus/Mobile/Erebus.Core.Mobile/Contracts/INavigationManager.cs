using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Mobile.Contracts
{
    public interface INavigationManager
    {
        Task NavigateAsync<TPresenter>(params PresenterParameter[] parameters) where TPresenter : IPresenter;
        Task NavigateByPopingCurrent<TPresenter>(params PresenterParameter[] parameters) where TPresenter : IPresenter;
    }
}
