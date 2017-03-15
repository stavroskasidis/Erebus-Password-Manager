using Erebus.Core.Contracts;
using Erebus.Core.Implementations;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Xunit;

namespace Erebus.Tests.Common
{
    public class AesCryptographerTests : IDisposable
    {
        private SecureString Key;
        private SecureString SecondKey;
        private Mock<ISecureStringConverter> SecureStringConverterMock;

        public AesCryptographerTests()
        {
            this.Key = new SecureString();
            foreach (char c in "123456")
            {
                Key.AppendChar(c);
            }

            this.SecondKey = new SecureString();
            foreach (char c in "654321")
            {
                SecondKey.AppendChar(c);
            }

            SecureStringConverterMock = new Mock<ISecureStringConverter>(MockBehavior.Strict);
            SecureStringConverterMock.Setup(mock => mock.ToString(this.Key)).Returns("123456");
            SecureStringConverterMock.Setup(mock => mock.ToString(this.SecondKey)).Returns("654321");
        }

        public void Dispose()
        {
            this.Key.Dispose();
            this.SecondKey.Dispose();
        }


        private AesCryptographer CreateDefault()
        {
            var defaultInstance = new AesCryptographer(SecureStringConverterMock.Object);

            return defaultInstance;
        }

        [Theory]
        [MemberData(nameof(RoundtripCases))]
        public void IsKeyValid_KeyIsValid_ReturnsTrue(byte[] input)
        {
            //Arrange
            var sut = CreateDefault();

            //Act
            var encrypted = sut.Encrypt(input, this.Key);
            bool isValid = sut.IsKeyValid(encrypted, this.Key);

            //Assert
            Assert.True(isValid);
        }

        [Theory]
        [MemberData(nameof(RoundtripCases))]
        public void IsKeyValid_KeyIsNotValid_ReturnsFalse(byte[] input)
        {
            //Arrange
            var sut = CreateDefault();

            //Act
            var encrypted = sut.Encrypt(input, this.Key);
            bool isValid = sut.IsKeyValid(encrypted, this.SecondKey);

            //Assert
            Assert.False(isValid);
        }


        [Fact]
        public void Decrypt_InputIsNull_ThrowsArgumentNullException()
        {
            //Arrange
            var sut = CreateDefault();

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = sut.Decrypt(null, this.Key);
            });
        }


        [Fact]
        public void Decrypt_KeyIsNull_ThrowsArgumentNullException()
        {
            //Arrange
            var sut = CreateDefault();

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = sut.Decrypt(new byte[] { 0x34 }, null);
            });
        }


        [Fact]
        public void Encrypt_InputIsNull_ThrowsArgumentNullException()
        {
            //Arrange
            var sut = CreateDefault();

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = sut.Encrypt(null, this.Key);
            });
        }


        [Fact]
        public void Encrypt_KeyIsNull_ThrowsArgumentNullException()
        {
            //Arrange
            var sut = CreateDefault();

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = sut.Encrypt(new byte[] { 0x34 }, null);
            });
        }

        [Theory]
        [MemberData(nameof(RoundtripCases))]
        public void Encrypt_Decrypt_Roundtrip_ResultIsSameAsSource(byte[] input)
        {
            //Arrange
            var sut = CreateDefault();

            //Act
            var encrypted = sut.Encrypt(input, this.Key);
            var result = sut.Decrypt(encrypted, this.Key);

            //Assert

            Assert.Equal(result, input);
            Assert.NotEqual(encrypted, input);
        }


        public static IEnumerable<object[]> RoundtripCases
        {
            get
            {
                return new[]
                {
                    new object[] {  new byte[] { 0x43, 0x23, 0xef } },
                    new object[] { new byte[] { 0x43, 0x23, 0xef, 0x43, 0x23, 0xee , 0x43, 0x23, 0xe2, 0x4a,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23,
                                                0x23, 0xee, 0x43, 0x23, 0xef, 0x43, 0x23, 0xee, 0x43, 0x23
                                             }
                                }
                };
            }
        }

    }
}
