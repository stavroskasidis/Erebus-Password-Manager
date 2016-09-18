using Erebus.Core.Contracts;
using Erebus.Core.Server;
using Erebus.Core.Server.Contracts;
using Erebus.Model.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.Controllers
{
    [AllowAnonymous]
    public class MobileServiceController : Controller
    {
        private IVaultRepositoryFactory VaultRepositoryFactory;
        private IServerConfigurationReader ServerConfigurationReader;
        private IFileSystem FileSystem;

        public MobileServiceController(IVaultRepositoryFactory vaultRepositoryFactory, IServerConfigurationReader serverConfigurationReader,
                                       IFileSystem fileSystem)
        {
            this.VaultRepositoryFactory = vaultRepositoryFactory;
            this.ServerConfigurationReader = serverConfigurationReader;
            this.FileSystem = fileSystem;
        }

        [HttpGet]
        public JsonResult CommunicationCheck()
        {
            return Json(true);
        }

        [HttpGet]
        public JsonResult GetVaultsInfo()
        {
            var vaultRepository = VaultRepositoryFactory.CreateInstance();
            var vaultNames = vaultRepository.GetAllVaultNames();
            var result = new List<VaultInfo>();
            foreach(string vault in vaultNames)
            {
                var metadata=vaultRepository.GetVaultMetadata(vault);
                result.Add(new VaultInfo
                {
                    VaultName = vault,
                    CreateLocation = metadata.CreateLocation,
                    Version = metadata.Version
                });
            }

            return Json(result);
        }

        [HttpGet]
        public FileResult DownloadVault(string vaultName)
        {
            var configuration = this.ServerConfigurationReader.GetConfiguration();
            var storageFolder = configuration.VaultsFolder;
            string path = Path.ChangeExtension(Path.Combine(storageFolder, vaultName), Constants.VAULT_FILE_NAME_EXTENSION);
            if (!this.FileSystem.FileExists(path)) throw new FileNotFoundException();

            byte[] fileBytes = this.FileSystem.ReadAllBytes(path);
            return File(fileBytes, "application/octet-stream");
        }
    }
}
