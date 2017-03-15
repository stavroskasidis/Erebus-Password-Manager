using Erebus.Core.Contracts;
using Erebus.Core.Implementations;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Erebus.Tests.Common
{
    public class VaultFileRepositoryTests
    {

        Mock<IFileSystem> FileSystemMock;
        Mock<ISymetricCryptographer> SymetricCryptographerMock;
        Mock<ISerializer> SerializerMock;
        Mock<IClockProvider> ClockProvider;
        Mock<IVaultFileMetadataHandler> VaultFileMetadataHandler;
        string VaultStorageFolder = "Vaults";
        string VaultFileExtension = ".evf";
        
        public VaultFileRepositoryTests()
        {
            FileSystemMock = new Mock<IFileSystem>(MockBehavior.Strict);
            SymetricCryptographerMock = new Mock<ISymetricCryptographer>(MockBehavior.Strict);
            SerializerMock = new Mock<ISerializer>(MockBehavior.Strict);
            ClockProvider = new Mock<IClockProvider>(MockBehavior.Strict);
            VaultFileMetadataHandler = new Mock<IVaultFileMetadataHandler>(MockBehavior.Strict);
        }


        private VaultFileRepository CreateDefault()
        {
            VaultFileRepository vault = new VaultFileRepository(FileSystemMock.Object,
                                                                VaultStorageFolder,
                                                                VaultFileExtension,
                                                                SymetricCryptographerMock.Object,
                                                                SerializerMock.Object,
                                                                ClockProvider.Object,
                                                                VaultFileMetadataHandler.Object);
            ClockProvider.Setup(mock => mock.GetNow()).Returns(DateTime.Now);

            return vault;
        }


        [Fact]
        public void GetAllVaultNames_VaultFolderExistsAndHas3VaultFiles_All3FilesAreReturned()
        {
            //Arrange
            var sut = CreateDefault();
            string[] expected = new string[] { "vault1", "vault2", "vault3" };
            string[] files = new string[] { expected[0] + this.VaultFileExtension,
                                            expected[1] + this.VaultFileExtension,
                                            expected[2] + this.VaultFileExtension
                                             };


            FileSystemMock.Setup(mock => mock.GetDirectoryFiles(this.VaultStorageFolder, "*" + this.VaultFileExtension))
                          .Returns(files);
            FileSystemMock.Setup(mock => mock.DirectoryExists(this.VaultStorageFolder)).Returns(true);

            //Act
            var result = sut.GetAllVaultNames();

            //Assert

            Assert.Equal(3, result.Count());
            Assert.Equal(expected[0], result.ElementAt(0));
            Assert.Equal(expected[1], result.ElementAt(1));
            Assert.Equal(expected[2], result.ElementAt(2));
        }

        [Fact]
        public void GetAllVaultNames_VaultFolderDoesNotExist_FolderIsCreated()
        {
            //Arrange
            var sut = CreateDefault();
            string[] expected = new string[] { };

            FileSystemMock.Setup(mock => mock.GetDirectoryFiles(this.VaultStorageFolder, "*" + this.VaultFileExtension))
                          .Returns(expected);
            FileSystemMock.Setup(mock => mock.DirectoryExists(this.VaultStorageFolder)).Returns(false);
            FileSystemMock.Setup(mock => mock.CreateDirectory(this.VaultStorageFolder));

            //Act
            var result = sut.GetAllVaultNames();

            //Assert
            Assert.Equal(0, result.Count());
            FileSystemMock.Verify(mock => mock.CreateDirectory(this.VaultStorageFolder));
        }

    }
}
