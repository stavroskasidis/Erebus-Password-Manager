using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;
using System.IO;

namespace Erebus.Core.Implementations
{
    public class AesCryptographer : ISymetricCryptographer
    {
        private static readonly byte[] Salt = new byte[] { 0x2a, 0xdc, 0xfa, 0x00, 0xa1, 0xed, 0x7a, 0xed, 0xc5, 0xfe, 0x07, 0xad, 0x4d, 0x08, 0x21, 0x3d };
        private static readonly int BufferSize = 1024;

        public byte[] Decrypt(byte[] input, SecureString key)
        {
            GuardClauses.ArgumentIsNotNull(nameof(input), input);
            GuardClauses.ArgumentIsNotNull(nameof(key), key);

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key.ToActualString(), Salt);
                aesAlg.Key = pdb.GetBytes(32);
                aesAlg.IV = pdb.GetBytes(16);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msInput = new MemoryStream(input))
                using (CryptoStream csDecrypt = new CryptoStream(msInput, decryptor, CryptoStreamMode.Read))
                using (MemoryStream msOutput = new MemoryStream())
                {
                    var buffer = new byte[BufferSize];
                    var read = csDecrypt.Read(buffer, 0, buffer.Length);
                    while (read > 0)
                    {
                        msOutput.Write(buffer, 0, read);
                        read = csDecrypt.Read(buffer, 0, buffer.Length);
                    }
                    return msOutput.ToArray();
                }
            }

        }

        public byte[] Encrypt(byte[] input, SecureString key)
        {
            GuardClauses.ArgumentIsNotNull(nameof(input), input);
            GuardClauses.ArgumentIsNotNull(nameof(key), key);

            using (Aes aesAlg = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key.ToActualString(), Salt);
                aesAlg.Key = pdb.GetBytes(32);
                aesAlg.IV = pdb.GetBytes(16);
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msInput = new MemoryStream(input))
                using (MemoryStream msOutput = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msOutput, encryptor, CryptoStreamMode.Write))
                {
                    var buffer = new byte[BufferSize];
                    var read = msInput.Read(buffer, 0, buffer.Length);
                    while (read > 0)
                    {
                        csEncrypt.Write(buffer, 0, read);
                        read = msInput.Read(buffer, 0, buffer.Length);
                    }
                    csEncrypt.FlushFinalBlock();
                    return msOutput.ToArray();
                }
            }
        }
    }
}
