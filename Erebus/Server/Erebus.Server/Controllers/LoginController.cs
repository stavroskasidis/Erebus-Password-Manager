using Erebus.Core.Contracts;
using Erebus.Core.Server;
using Erebus.Core.Server.Contracts;
using Erebus.Resources;
using Erebus.Server.Authorization;
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
        private IAuthorizationLogic AuthorizationLogic;

        public LoginController(IVaultRepositoryFactory vaultRepositoryFactory, ISecureStringConverter secureStringConverter,
                               ISecureStringBinarySerializer secureStringBinarySerializer, ISessionContext sessionContext,
                               IAuthorizationLogic authorizationLogic)
        {
            this.VaultRepositoryFactory = vaultRepositoryFactory;
            this.SecureStringConverter = secureStringConverter;
            this.SessionContext = sessionContext;
            this.AuthorizationLogic = authorizationLogic;
        }

        [AllowAnonymous]
        public IActionResult Index(bool expired)
        {
            if (this.AuthorizationLogic.IsLoggedIn) return RedirectToAction("Index", "VaultExplorer");

            var repository = VaultRepositoryFactory.CreateInstance();
            var vaults = repository.GetAllVaultNames();
            if (vaults.Count() == 0) return RedirectToAction("Create", "Vault");

            var viewModel = new LoginIndexViewModel()
            {
                VaultNames = vaults,
                Expired = expired
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
                        this.SessionContext.SetCurrentVaultName(model.SelectedVault);
                        this.SessionContext.SetMasterPassword(masterPasswordSecure);
                        return RedirectToAction("Index", "VaultExplorer");
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



        public IActionResult Logout(bool expired)
        {
            this.SessionContext.ClearSession();
            return RedirectToAction("Index", "Login", new { expired = expired });
        }

        public JsonResult RenewSession()
        {
            this.HttpContext.Session.SetString("KeepSessionAlive", DateTime.Now.ToString());
            return Json(new { success = true });
        }
    }
}
