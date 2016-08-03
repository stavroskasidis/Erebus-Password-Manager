using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Erebus.Core.Contracts;
using Erebus.Server.ViewModels;
using Erebus.Core.Server.Contracts;

namespace Erebus.Server.Controllers
{
    public class VaultExplorerController : Controller
    {
        private IVaultRepositoryFactory VaultRepositoryFactory;
        //private ISecureStringConverter SecureStringConverter;
        private ISessionContext SessionContext;
        private IVaultManipulatorFactory VaultManipulatorFactory;

        public VaultExplorerController(IVaultRepositoryFactory vaultRepositoryFactory, ISessionContext sessionContext, IVaultManipulatorFactory vaultManipulatorFactory)
        {
            this.VaultRepositoryFactory = vaultRepositoryFactory;
            this.SessionContext = sessionContext;
            this.VaultManipulatorFactory = vaultManipulatorFactory;
        }

        public IActionResult Index()
        {
            return View();
        }


        private TreeNode CreateTreeNode(string id, string text, string parentId, bool hasChildren, bool opened, bool isGroup)
        {
            var treeNode = new TreeNode()
            {
                Id = id,
                Text = text,
                Parent = parentId,
                HasChildren = hasChildren,
            };
            treeNode.State.Opened = opened;

            if (isGroup)
            {
                treeNode.LiAttributes.Add("node-type", "group");
            }
            else
            {
                treeNode.LiAttributes.Add("node-type", "entry");
            }
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
                    treeNodes.AddRange(vault.Groups.Select(group => CreateTreeNode(group.Id.ToString(), group.Name, parentId, group.Groups.Count > 0 || group.Entries.Count > 0, true, true)));
                }
                else
                {
                    var vaultExplorer = VaultManipulatorFactory.CreateInstance(vault);
                    var parentGroup = vaultExplorer.GetGroupById(Guid.Parse(parentId));

                    treeNodes.AddRange(parentGroup.Groups.Select(group => CreateTreeNode(group.Id.ToString(), group.Name, parentId, group.Groups.Count > 0 || group.Entries.Count > 0, false, true)));
                    treeNodes.AddRange(parentGroup.Entries.Select(entry => CreateTreeNode(entry.Id.ToString(), entry.Title, parentId, false, false, false)));
                }

                return Json(treeNodes);
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
                var vaultExplorer = this.VaultManipulatorFactory.CreateInstance(vault);

                if (id == null)
                {
                    return PartialView(new GroupEditViewModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ParentId = parentId
                    });
                }
                else
                {
                    var group = vaultExplorer.GetGroupById(Guid.Parse(id));
                    if (group == null) throw new Exception($"Group with id {id} was not found");
                    return PartialView(new GroupEditViewModel()
                    {
                        Id = id,
                        Name = group.Name,
                        ParentId = parentId
                    });
                }

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrEditGroup(GroupEditViewModel model)
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
    }
}
