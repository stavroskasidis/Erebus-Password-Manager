using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Server.Models.HomeViewModels;

namespace PasswordManager.Server.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult GetMenuData()
        {
            //http://www.easyjstree.com/Demos/Initialisation

            MenuItemViewModel root = new MenuItemViewModel()
            {
                text = "Root",
                isFolder = true
            };
            root.children.Add(new MenuItemViewModel()
            {
                text = "Sub item"
            });
            List<MenuItemViewModel> menuNodes = new List<MenuItemViewModel>();
            menuNodes.Add(root);
            return Json(menuNodes.ToArray());
        }
    }
}
