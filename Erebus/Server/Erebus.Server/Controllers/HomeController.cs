using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Erebus.Core.Contracts;

namespace Erebus.Server.Controllers
{
    public class HomeController : Controller
    {
        private IVaultRepository VaultRepository;

        public HomeController(IVaultRepository vaultRepository)
        {
            this.VaultRepository = vaultRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
