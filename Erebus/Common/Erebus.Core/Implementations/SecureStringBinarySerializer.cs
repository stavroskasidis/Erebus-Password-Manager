using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace Erebus.Core.Implementations
{
    public class SecureStringBinarySerializer : ISecureStringBinarySerializer
    {
        private ISymetricCryptographer Cryptographer;
        private ISecureStringConverter SecureStringConverter;
        private SecureString EncryptionKey;

        public SecureStringBinarySerializer(ISymetricCryptographer cryptographer, SecureString encryptionKey, ISecureStringConverter secureStringConverter)
        {
            Cryptographer = cryptographer;
            EncryptionKey = encryptionKey;
            SecureStringConverter = secureStringConverter;
        }

        public SecureString FromByteArray(byte[] data)
        {
            return SecureStringConverter.ToSecureString(Encoding.UTF8.GetString(Cryptographer.Decrypt(data, EncryptionKey)));
        }

        public byte[] ToByteArray(SecureString secureString)
        {
            return Cryptographer.Encrypt(Encoding.UTF8.GetBytes(SecureStringConverter.ToString(secureString)), EncryptionKey);
        }
    }
}
