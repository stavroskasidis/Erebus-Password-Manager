using Erebus.Core.Contracts;
using Erebus.Core.Implementations;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Tests.Common
{
    [TestFixture]
    public class VaultFileRepositoryTests
    {

        Mock<IFileSystem> FileSystemMock;
        Mock<ISymetricCryptographer> SymetricCryptographerMock;
        Mock<ISerializer> SerializerMock;
        string VaultStorageFolder = "Vaults";
        string VaultFileExtension = ".evf";

        [SetUp]
        public void Setup()
        {
            FileSystemMock = new Mock<IFileSystem>(MockBehavior.Strict);
            SymetricCryptographerMock = new Mock<ISymetricCryptographer>(MockBehavior.Strict);
            SerializerMock = new Mock<ISerializer>(MockBehavior.Strict);
        }


        private VaultFileRepository CreateDefault()
        {
            VaultFileRepository vault = new VaultFileRepository(FileSystemMock.Object,
                                                                VaultStorageFolder,
                                                                VaultFileExtension,
                                                                SymetricCryptographerMock.Object,
                                                                SerializerMock.Object);

            return vault;
        }


        [Test]
        public void GetAllVaultNames_VaultFolderHas3VaultFiles_All3FilesAreReturned()
        {
            //Arrange
            var sut = CreateDefault();
            string[] expected = new string[] { "vault1." + this.VaultFileExtension,
                                               "vault2." + this.VaultFileExtension,
                                               "vault3." + this.VaultFileExtension
                                             };

            FileSystemMock.Setup(x => x.GetDirectoryFiles(this.VaultStorageFolder, "*" + this.VaultFileExtension))
                          .Returns(expected);

            //Act
            var result = sut.GetAllVaultNames();

            //Assert

            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(expected[0], result.ElementAt(0));
            Assert.AreEqual(expected[1], result.ElementAt(1));
            Assert.AreEqual(expected[2], result.ElementAt(2));
        }
        
    }
}
