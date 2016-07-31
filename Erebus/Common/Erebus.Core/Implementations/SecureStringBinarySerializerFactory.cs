using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace Erebus.Core.Implementations
{
    public class SecureStringBinarySerializerFactory : ISecureStringBinarySerializerFactory
    {
        private ISymetricCryptographer SymetricCryptographer { get; set; }
        private ISecureStringConverter SecureStringConverter { get; set; }

        public SecureStringBinarySerializerFactory(ISymetricCryptographer symetricCryptographer, ISecureStringConverter secureStringConverter)
        {
            GuardClauses.ArgumentIsNotNull(nameof(symetricCryptographer), symetricCryptographer);
            GuardClauses.ArgumentIsNotNull(nameof(secureStringConverter), secureStringConverter);

            SymetricCryptographer = symetricCryptographer;
            SecureStringConverter = secureStringConverter;
        }

        public ISecureStringBinarySerializer CreateInstance()
        {
            SecureString serializerEncryptionKey = new SecureString();
            serializerEncryptionKey.AppendChar('T');
            serializerEncryptionKey.AppendChar('3');
            serializerEncryptionKey.AppendChar('K');
            serializerEncryptionKey.AppendChar('s');
            serializerEncryptionKey.AppendChar('t');
            serializerEncryptionKey.AppendChar('B');
            serializerEncryptionKey.AppendChar('u');
            serializerEncryptionKey.AppendChar('H');
            serializerEncryptionKey.AppendChar('W');
            serializerEncryptionKey.AppendChar('t');
            serializerEncryptionKey.AppendChar('b');
            serializerEncryptionKey.AppendChar('x');
            serializerEncryptionKey.AppendChar('H');
            serializerEncryptionKey.AppendChar('M');
            serializerEncryptionKey.AppendChar('P');
            serializerEncryptionKey.AppendChar('b');
            return new SecureStringBinarySerializer(SymetricCryptographer, serializerEncryptionKey, SecureStringConverter);
        }
    }
}
