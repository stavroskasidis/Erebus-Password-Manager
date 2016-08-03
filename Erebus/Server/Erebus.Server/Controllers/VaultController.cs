using Erebus.Core.Contracts;
using Erebus.Model;
using Erebus.Resources;
using Erebus.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.Controllers
{
    public class VaultController : Controller
    {
        private IVaultRepositoryFactory VaultRepositoryFactory;
        private IVaultFactory VaultFactory;
        private ISecureStringConverter SecureStringConverter;

        public VaultController(IVaultRepositoryFactory vaultRepositoryFactory, IVaultFactory vaultFactory, ISecureStringConverter secureStringConverter)
        {
            this.VaultRepositoryFactory = vaultRepositoryFactory;
            this.VaultFactory = vaultFactory;
            this.SecureStringConverter = secureStringConverter;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VaultCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var repository = this.VaultRepositoryFactory.CreateInstance();
                if (repository.GetAllVaultNames().Contains(model.VaultName))
                {
                    ModelState.AddModelError(nameof(model.VaultName), StringResources.VaultNameAlreadyExists);
                    return View(model);
                }

                var vault = this.VaultFactory.CreateVault(model.VaultName);
                repository.SaveVault(vault, this.SecureStringConverter.ToSecureString(model.MasterPassword));

                return RedirectToAction("Index", "VaultExplorer");
            }

            return View(model);
        }
    }
}
