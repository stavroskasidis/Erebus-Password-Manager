using Erebus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Contracts
{
    public interface IVaultFileMetadataHandler
    {
        VaultMetadata GetVaultFileMetadata(byte[] vaultFileBytesWithMetadata);
        byte[] AddMetadataHeader(VaultMetadata metadata, byte[] encryptedVaultBytes);
        byte[] GetVaultFileBytesWithoutMetadataHeader(byte[] vaultBytesWithMetadata);
    }
}
