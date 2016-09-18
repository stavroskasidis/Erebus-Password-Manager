using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Erebus.Model;

namespace Erebus.Core.Implementations
{
    public class VaultFileMetadataHandler : IVaultFileMetadataHandler
    {
        private IByteArrayHelper ByteArrayHelper;
        private readonly byte[] HeaderSeperator = new byte[] { 0x1b, 0x1b, 0x1b };
        private readonly int TotalHeaderLength = 500;

        public VaultFileMetadataHandler(IByteArrayHelper byteArrayHelper)
        {
            this.ByteArrayHelper = byteArrayHelper;
        }

        private byte[] GetVaultFileMetadataHeader(byte[] fileBytes)
        {
            var split = ByteArrayHelper.Separate(fileBytes, HeaderSeperator);
            if (split.Length == 1)
            {
                return null;
            }
            else
            {
                return split[0];
            }
        }

        public VaultMetadata GetVaultFileMetadata(byte[] vaultFileBytesWithMetadata)
        {
            int versionBytesCount = sizeof(long);
            int createLocationBytesCount = sizeof(int);

            byte[] metadataBytes = GetVaultFileMetadataHeader(vaultFileBytesWithMetadata);
            var result = new VaultMetadata();

            if (metadataBytes != null)
            {
                var versionBytes = metadataBytes.Take(versionBytesCount).ToArray();
                var createLocationBytes = metadataBytes.Skip(versionBytesCount).Take(createLocationBytesCount).ToArray();
                result.Version = BitConverter.ToInt64(versionBytes, 0);
                result.CreateLocation = (VaultCreateLocation)BitConverter.ToInt32(createLocationBytes, 0);
            }

            return result;
        }

        private byte[] CreateVaultMetadataHeader(VaultMetadata vaultMetadata)
        {
            int versionBytesCount = sizeof(long);
            var versionBytes = BitConverter.GetBytes(vaultMetadata.Version);
            var createLocationBytes = BitConverter.GetBytes((int)vaultMetadata.CreateLocation);
            
            var headerBytes = new byte[TotalHeaderLength];
            versionBytes.CopyTo(headerBytes, 0);
            createLocationBytes.CopyTo(headerBytes, versionBytesCount);

            return headerBytes;
        }

        public byte[] GetVaultFileBytesWithoutMetadataHeader(byte[] vaultBytesWithMetadata)
        {
            var split = ByteArrayHelper.Separate(vaultBytesWithMetadata, this.HeaderSeperator);

            if (split.Length == 1)
            {
                return vaultBytesWithMetadata;
            }
            else
            {
                return split[1];
            }
        }

        public byte[] AddMetadataHeader(VaultMetadata metadata, byte[] encryptedVaultBytes)
        {
            byte[] metadataBytes = CreateVaultMetadataHeader(metadata);
            byte[] finalBytes = new byte[metadataBytes.Length + encryptedVaultBytes.Length + this.HeaderSeperator.Length];

            metadataBytes.CopyTo(finalBytes, 0);
            HeaderSeperator.CopyTo(finalBytes, metadataBytes.Length);
            encryptedVaultBytes.CopyTo(finalBytes, metadataBytes.Length + HeaderSeperator.Length);

            return finalBytes;
        }
    }
}
