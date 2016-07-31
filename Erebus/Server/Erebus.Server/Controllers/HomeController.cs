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
    public class HomeController : Controller
    {
        private IVaultRepositoryFactory VaultRepositoryFactory;
        //private ISecureStringConverter SecureStringConverter;
        private ISessionContext SessionContext;
        private IVaultExplorerFactory VaultExplorerFactory;

        public HomeController(IVaultRepositoryFactory vaultRepositoryFactory, ISessionContext sessionContext, IVaultExplorerFactory vaultExplorerFactory)
        {
            this.VaultRepositoryFactory = vaultRepositoryFactory;
            this.SessionContext = sessionContext;
            this.VaultExplorerFactory = vaultExplorerFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetNodeItems(string parentId)
        {
            var treeNodes = new List<TreeNode>();
            using (var masterPassword = SessionContext.GetMasterPassword())
            {
                var currentVault = SessionContext.GetCurrentVault();

                var vaultRepository = VaultRepositoryFactory.CreateInstance();
                var vault = vaultRepository.GetVault(currentVault, masterPassword);
                if (parentId == "#")
                {
                    //Root
                    treeNodes.AddRange(vault.Groups.Select(group =>
                    {
                        var treeNode = new TreeNode()
                        {
                            Id = group.Id.ToString(),
                            Text = group.Name,
                            Parent = parentId,
                            HasChildren = group.Groups.Count > 0 || group.Entries.Count > 0,
                            
                        };
                        treeNode.State.Opened = true;
                        treeNode.LiAttributes.Add("node-type", "group");

                        return treeNode;
                    }));
                }
                else
                {
                    var vaultExplorer = VaultExplorerFactory.CreateInstance(vault);
                    var parentGroup = vaultExplorer.GetGroupById(Guid.Parse(parentId));

                    treeNodes.AddRange(parentGroup.Groups.Select(group =>
                    {
                        var treeNode = new TreeNode()
                        {
                            Id = group.Id.ToString(),
                            Text = group.Name,
                            Parent = parentId,
                            HasChildren = group.Groups.Count > 0 || group.Entries.Count > 0,
                        };
                        treeNode.LiAttributes.Add("node-type", "group");
                        return treeNode;
                    }));

                    treeNodes.AddRange(parentGroup.Entries.Select(entry =>
                    {
                        var treeNode = new TreeNode()
                        {
                            Id = entry.Id.ToString(),
                            Text = entry.Title,
                            Parent = parentId,
                            HasChildren = false
                        };
                        treeNode.LiAttributes.Add("node-type", "entry");
                        return treeNode;
                    }));
                }

                return Json(treeNodes);
            }
        }
    }
}
