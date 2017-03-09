using Erebus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Mobile.Views.Contracts
{
    public interface IEntryDetailsView
    {
        Entry Entry {set;}
        bool IsPasswordVisible { get; set; }
        event Action ShowHidePassword;
        event Action CopyPasswordToClipboard;
    }
}
