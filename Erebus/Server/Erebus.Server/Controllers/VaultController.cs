using Erebus.Core.Contracts;
using Erebus.Core.Server.Contracts;
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
        private ISessionContext SessionContext;
        private ISyncContext SyncContext;

        public VaultController(IVaultRepositoryFactory vaultRepositoryFactory, IVaultFactory vaultFactory, ISecureStringConverter secureStringConverter,
                               ISessionContext sessionContext, ISyncContext syncContext)
        {
            this.VaultRepositoryFactory = vaultRepositoryFactory;
            this.VaultFactory = vaultFactory;
            this.SecureStringConverter = secureStringConverter;
            this.SessionContext = sessionContext;
            this.SyncContext = syncContext;
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
                var vaultRepository = this.VaultRepositoryFactory.CreateInstance();
                if (vaultRepository.VaultExists(model.VaultName))
                {
                    ModelState.AddModelError(nameof(model.VaultName), StringResources.VaultNameAlreadyExists);
                    return View(model);
                }

                var vault = this.VaultFactory.CreateVault(model.VaultName);
                vaultRepository.SaveVault(vault, this.SecureStringConverter.ToSecureString(model.MasterPassword));

                return RedirectToAction("Index", "VaultExplorer");
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Rename()
        {
            var model = new VaultRenameViewModel()
            {
                VaultName = this.SessionContext.GetCurrentVaultName()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rename(VaultRenameViewModel model)
        {
            SyncContext.Lock();
            try
            {
                using (var masterPassword = SessionContext.GetMasterPassword())
                {
                    if (ModelState.IsValid)
                    {
                        var vaultRepository = VaultRepositoryFactory.CreateInstance();
                        if (vaultRepository.VaultExists(model.NewVaultName))
                        {
                            ModelState.AddModelError(nameof(model.NewVaultName), StringResources.VaultNameAlreadyExists);
                            return View(model);
                        }

                        var currentVaultName = SessionContext.GetCurrentVaultName();
                        var vault = vaultRepository.GetVault(currentVaultName, masterPassword);
                        vault.Name = model.NewVaultName;

                        vaultRepository.SaveVault(vault, masterPassword);
                        vaultRepository.DeleteVault(currentVaultName);
                        SessionContext.SetCurrentVaultName(model.NewVaultName);

                        return RedirectToAction("Index", "VaultExplorer");

                    }
                    else
                    {
                        return View(model);
                    }
                }
            }
            finally
            {
                SyncContext.Release();
            }
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            var model = new VaultChangePasswordViewModel()
            {
                VaultName = this.SessionContext.GetCurrentVaultName()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(VaultChangePasswordViewModel model)
        {
            SyncContext.Lock();
            try
            {
                if (ModelState.IsValid)
                {
                    using (var oldPassword = this.SecureStringConverter.ToSecureString(model.MasterPassword))
                    using (var newPassword = this.SecureStringConverter.ToSecureString(model.NewMasterPassword))
                    {
                        var vaultRepository = VaultRepositoryFactory.CreateInstance();
                        var currentVaultName = SessionContext.GetCurrentVaultName();
                        if (vaultRepository.IsPasswordValid(currentVaultName, oldPassword) == false)
                        {
                            ModelState.AddModelError(nameof(model.MasterPassword), StringResources.IncorrectPassword);
                            return View(model);
                        }

                        var vault = vaultRepository.GetVault(currentVaultName, oldPassword);
                        vaultRepository.SaveVault(vault, newPassword);
                        SessionContext.SetMasterPassword(newPassword);
                        return RedirectToAction("Index", "VaultExplorer");
                    }
                }
                else
                {
                    return View(model);
                }
            }
            finally
            {
                SyncContext.Release();
            }
        }

    }
}
