using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Mobile.Contracts
{
    public interface IPresenterFactory
    {
        TPresenter Create<TPresenter>(params PresenterParameter[] parameters) where TPresenter : IPresenter;
    }
}
