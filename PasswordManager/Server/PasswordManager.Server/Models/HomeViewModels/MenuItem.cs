using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManager.Server.Models.HomeViewModels
{
    public class MenuItemViewModel
    {
        public List<MenuItemViewModel> children { get; set; }
        public bool isExpanded { get; set; }
        public bool isFolder { get; set; }
        public string text { get; set; }
        public string tooltip { get; set; }
        public string href { get; set; }
        public string hrefTarget { get; set; }

        public MenuItemViewModel()
        {
            this.children = new List<MenuItemViewModel>();
        }
    }
}
