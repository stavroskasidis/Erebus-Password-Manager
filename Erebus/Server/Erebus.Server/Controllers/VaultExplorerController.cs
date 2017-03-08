using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Erebus.Core.Contracts;
using Erebus.Server.ViewModels;
using Erebus.Core.Server.Contracts;
using Erebus.Localization;

namespace Erebus.Server.Controllers
{
    public class VaultExplorerController : Controller
    {
        private IVaultRepositoryFactory VaultRepositoryFactory;
        private ISessionContext SessionContext;
        private IVaultManipulatorFactory VaultManipulatorFactory;
        private ISyncContext SyncContext;
        private IPasswordGenerator PasswordGenerator;

        public VaultExplorerController(IVaultRepositoryFactory vaultRepositoryFactory, ISessionContext sessionContext,
            IVaultManipulatorFactory vaultManipulatorFactory, ISyncContext syncContext,
            IPasswordGenerator passwordGenerator)
        {
            this.VaultRepositoryFactory = vaultRepositoryFactory;
            this.SessionContext = sessionContext;
            this.VaultManipulatorFactory = vaultManipulatorFactory;
            this.SyncContext = syncContext;
            this.PasswordGenerator = passwordGenerator;

        }

        public IActionResult Index()
        {
            this.ViewBag.Title = SessionContext.GetCurrentVaultName();
            this.ViewBag.SubTitle = StringResources.VaultExplorer;

            var test = this.User.Identity.IsAuthenticated;
            return View();
        }


        private TreeNode CreateTreeNode(string id, string text, string parentId, bool hasChildren, bool opened, int entriesCount)
        {
            var treeNode = new TreeNode()
            {
                Id = id,
                Text = text + (entriesCount > 0 ? $" <span class='badge'>{entriesCount}</span>" : ""),
                Parent = parentId,
                HasChildren = hasChildren,
            };
            treeNode.State.Opened = opened;

            //if (isGroup)
            //{
            //    treeNode.LiAttributes.Add("node-type", "group");
            //}
            //else
            //{
            //    treeNode.LiAttributes.Add("node-type", "entry");
            //}
            return treeNode;
        }

        public JsonResult GetNodeItems(string parentId)
        {
            var treeNodes = new List<TreeNode>();
            using (var masterPassword = SessionContext.GetMasterPassword())
            {
                var currentVault = SessionContext.GetCurrentVaultName();

                var vaultRepository = VaultRepositoryFactory.CreateInstance();
                var vault = vaultRepository.GetVault(currentVault, masterPassword);
                if (parentId == "#")
                {
                    //Root
                    treeNodes.AddRange(vault.Groups.Select(group => CreateTreeNode(group.Id.ToString(), group.Name, parentId, group.Groups.Count > 0, true, group.Entries.Count)));
                }
                else
                {
                    var vaultManipulator = VaultManipulatorFactory.CreateInstance(vault);
                    var parentGroup = vaultManipulator.GetGroupById(Guid.Parse(parentId));

                    treeNodes.AddRange(parentGroup.Groups.Select(group => CreateTreeNode(group.Id.ToString(), group.Name, parentId, group.Groups.Count > 0, false, group.Entries.Count)));
                    //treeNodes.AddRange(parentGroup.Entries.Select(entry => CreateTreeNode(entry.Id.ToString(), entry.Title, parentId, false, false, false)));
                }

                return Json(treeNodes);
            }
        }

        public ActionResult GroupEntriesGrid(Guid groupId)
        {
            using (var masterPassword = SessionContext.GetMasterPassword())
            {
                var currentVault = SessionContext.GetCurrentVaultName();

                var vaultRepository = VaultRepositoryFactory.CreateInstance();
                var vault = vaultRepository.GetVault(currentVault, masterPassword);
                var vaultManipulator = VaultManipulatorFactory.CreateInstance(vault);
                var group = vaultManipulator.GetGroupById(groupId);
                var model = group.Entries.Select(entry => new EntryGridViewModel()
                {
                    Id = entry.Id.ToString(),
                    Title = entry.Title,
                    UserName = entry.UserName,
                    Password = entry.Password,
                    Description = entry.Description,
                    Url = entry.Url
                });
                return PartialView(model);
            }
        }


        [HttpGet]
        public IActionResult AddOrEditGroup(string id, string parentId)
        {
            using (var masterPassword = SessionContext.GetMasterPassword())
            {
                var currentVaultName = SessionContext.GetCurrentVaultName();
                var vaultRepository = VaultRepositoryFactory.CreateInstance();
                var vault = vaultRepository.GetVault(currentVaultName, masterPassword);
                var vaultManipulator = this.VaultManipulatorFactory.CreateInstance(vault);

                if (id == null)
                {
                    return PartialView(new GroupEditViewModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ParentId = parentId,
                        ModalTitle = (parentId == "#") ? StringResources.NewGroup : StringResources.NewSubGroup
                    });
                }
                else
                {
                    var group = vaultManipulator.GetGroupById(Guid.Parse(id));
                    if (group == null) throw new Exception($"Group with id {id} was not found");
                    return PartialView(new GroupEditViewModel()
                    {
                        Id = id,
                        Name = group.Name,
                        ParentId = parentId,
                        ModalTitle = StringResources.GroupEdit
                    });
                }

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrEditGroup(GroupEditViewModel model)
        {
            SyncContext.Lock();
            try
            {
                using (var masterPassword = SessionContext.GetMasterPassword())
                {
                    if (ModelState.IsValid)
                    {

                        var currentVaultName = SessionContext.GetCurrentVaultName();
                        var vaultRepository = VaultRepositoryFactory.CreateInstance();
                        var vault = vaultRepository.GetVault(currentVaultName, masterPassword);
                        var vaultManipulator = this.VaultManipulatorFactory.CreateInstance(vault);

                        var group = vaultManipulator.GetGroupById(Guid.Parse(model.Id));

                        bool isNew = false;
                        if (group == null)
                        {
                            group = new Model.Group();
                            group.Id = Guid.Parse(model.Id);
                            isNew = true;
                        }

                        group.Name = model.Name;

                        if (isNew)
                        {
                            Guid? parentId = model.ParentId == "#" ? null : (Guid?)Guid.Parse(model.ParentId);
                            vaultManipulator.AddGroup(parentId, group);
                        }
                        else
                        {
                            vaultManipulator.UpdateGroup(group);
                        }

                        vaultRepository.SaveVault(vault, masterPassword);
                        return Json(new { success = true });

                    }
                    else
                    {
                        return Json(new { success = false });
                    }
                }
            }
            finally
            {
                SyncContext.Release();
            }
        }

        [HttpPost]
        public JsonResult DeleteGroup(Guid id)
        {
            SyncContext.Lock();
            try
            {
                using (var masterPassword = SessionContext.GetMasterPassword())
                {
                    var currentVaultName = SessionContext.GetCurrentVaultName();
                    var vaultRepository = VaultRepositoryFactory.CreateInstance();
                    var vault = vaultRepository.GetVault(currentVaultName, masterPassword);
                    var vaultManipulator = this.VaultManipulatorFactory.CreateInstance(vault);
                    vaultManipulator.DeleteGroupById(id);
                    vaultRepository.SaveVault(vault, masterPassword);

                    return Json(new { success = true });
                }
            }
            finally
            {
                SyncContext.Release();
            }
        }


        [HttpGet]
        public IActionResult AddOrEditEntry(string id, string parentId)
        {
            using (var masterPassword = SessionContext.GetMasterPassword())
            {
                var currentVaultName = SessionContext.GetCurrentVaultName();
                var vaultRepository = VaultRepositoryFactory.CreateInstance();
                var vault = vaultRepository.GetVault(currentVaultName, masterPassword);
                var vaultManipulator = this.VaultManipulatorFactory.CreateInstance(vault);

                if (id == null)
                {
                    return PartialView(new EntryEditViewModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ParentId = parentId,
                        ModalTitle = StringResources.NewEntry
                    });
                }
                else
                {
                    var entry = vaultManipulator.GetEntryById(Guid.Parse(id));
                    if (entry == null) throw new Exception($"Entry with id '{id}' was not found");
                    return PartialView(new EntryEditViewModel()
                    {
                        Id = id,
                        ParentId = parentId,
                        Title = entry.Title,
                        UserName = entry.UserName,
                        Password = entry.Password,
                        Url = entry.Url,
                        Description = entry.Description,
                        ModalTitle = StringResources.EntryEdit
                    });
                }

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrEditEntry(EntryEditViewModel model)
        {
            SyncContext.Lock();
            try
            {
                using (var masterPassword = SessionContext.GetMasterPassword())
                {
                    if (ModelState.IsValid)
                    {

                        var currentVaultName = SessionContext.GetCurrentVaultName();
                        var vaultRepository = VaultRepositoryFactory.CreateInstance();
                        var vault = vaultRepository.GetVault(currentVaultName, masterPassword);
                        var vaultManipulator = this.VaultManipulatorFactory.CreateInstance(vault);

                        var entry = vaultManipulator.GetEntryById(Guid.Parse(model.Id));

                        bool isNew = false;
                        if (entry == null)
                        {
                            entry = new Model.Entry();
                            entry.Id = Guid.Parse(model.Id);
                            isNew = true;
                        }

                        entry.Title = model.Title;
                        entry.UserName = model.UserName;
                        entry.Password = model.Password;
                        entry.Url = model.Url;
                        entry.Description = model.Description;

                        if (isNew)
                        {
                            Guid parentId = Guid.Parse(model.ParentId);
                            vaultManipulator.AddEntry(parentId, entry);
                        }
                        else
                        {
                            vaultManipulator.UpdateEntry(entry);
                        }

                        vaultRepository.SaveVault(vault, masterPassword);
                        return Json(new { success = true });

                    }
                    else
                    {
                        return Json(new { success = false });
                    }
                }
            }
            finally
            {
                SyncContext.Release();
            }
        }

        [HttpPost]
        public JsonResult DeleteEntry(Guid id)
        {
            SyncContext.Lock();
            try
            {
                using (var masterPassword = SessionContext.GetMasterPassword())
                {
                    var currentVaultName = SessionContext.GetCurrentVaultName();
                    var vaultRepository = VaultRepositoryFactory.CreateInstance();
                    var vault = vaultRepository.GetVault(currentVaultName, masterPassword);
                    var vaultManipulator = this.VaultManipulatorFactory.CreateInstance(vault);
                    vaultManipulator.DeleteEntryById(id);
                    vaultRepository.SaveVault(vault, masterPassword);

                    return Json(new { success = true });
                }
            }
            finally
            {
                SyncContext.Release();
            }
        }

        [HttpGet]
        public IActionResult GeneratePassword(string id)
        {
            var model = new GeneratePasswordViewModel()
            {
                Id = id,
                IncludeLowerCase = true,
                IncludeUpperCase = true,
                PasswordLength = 20,
                ModalTitle = StringResources.GeneratePassword
            };
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GeneratePassword(GeneratePasswordViewModel model)
        {
            if (!ModelState.IsValid ||
                (!model.IncludeDigits && !model.IncludeLowerCase && !model.IncludeSymbols && !model.IncludeUpperCase) ||
                model.PasswordLength <= 0)
            {
                return Json(new { success = false, error = StringResources.InvalidInput });
            }

            var generatedPassword = this.PasswordGenerator.GeneratePassword(model.PasswordLength, model.IncludeUpperCase, model.IncludeLowerCase, model.IncludeDigits, model.IncludeSymbols);
            return Json(new { success = true, password = generatedPassword });
        }
    }
}
