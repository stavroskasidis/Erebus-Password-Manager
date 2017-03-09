using Erebus.Mobile.Presenters.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Mobile.Views.Contracts
{
    public interface IView
    {
        void Attach(TPresenterCallbacks presenterCallbacks);
    }
}
