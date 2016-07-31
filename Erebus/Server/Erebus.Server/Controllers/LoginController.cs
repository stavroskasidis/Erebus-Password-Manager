using Erebus.Core.Contracts;
using Erebus.Core.Server;
using Erebus.Core.Server.Contracts;
using Erebus.Resources;
using Erebus.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.Controllers
{
    public class LoginController : Controller
    {
        private IVaultRepositoryFactory VaultRepositoryFactory;
        private ISecureStringConverter SecureStringConverter;
        private ISessionContext SessionContext;

        public LoginController(IVaultRepositoryFactory vaultRepositoryFactory, ISecureStringConverter secureStringConverter, 
                               ISecureStringBinarySerializer secureStringBinarySerializer, ISessionContext sessionContext)
        {
            this.VaultRepositoryFactory = vaultRepositoryFactory;
            this.SecureStringConverter = secureStringConverter;
            this.SessionContext= sessionContext;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var repository = VaultRepositoryFactory.CreateInstance();
            var vaults = repository.GetAllVaultNames();
            if (vaults.Count() == 0) return RedirectToAction("Create", "Vault");

            var viewModel = new LoginIndexViewModel()
            {
                VaultNames = vaults
            };

            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Validate(LoginIndexViewModel model)
        {
            var repository = VaultRepositoryFactory.CreateInstance();
            if (ModelState.IsValid)
            {
                using (var masterPasswordSecure = SecureStringConverter.ToSecureString(model.MasterPassword))
                {
                    if (repository.IsPasswordValid(model.SelectedVault, masterPasswordSecure))
                    {
                        this.SessionContext.SetCurrentVault(model.SelectedVault);
                        this.SessionContext.SetMasterPassword(masterPasswordSecure);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.MasterPassword), StringResources.IncorrectPassword);
                    }
                }
            }

            model.VaultNames = repository.GetAllVaultNames();
            return View("Index", model);
        }
    }
}
