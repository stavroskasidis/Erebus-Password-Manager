using Erebus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security;
using System.Diagnostics;
using Erebus.Core.Implementations;
using Xunit;

namespace Erebus.Tests.Common
{
    public class SecureStringConverterTests
    {

        private SecureStringConverter CreateDefault()
        {
            var defaultInstance = new SecureStringConverter();
            return defaultInstance;
        }

        [Theory]
        [InlineData("hello !")]
        [InlineData("asdf12345!")]
        [InlineData("@#!$@%%&ασδασδ;3ςε;2ε!")]
        public void ToSecureString_StringIsNotNull_SecureStringCreated(string stringToConvert)
        {
            //Arrange
            var sut = CreateDefault();

            //Act
            var secureString = sut.ToSecureString(stringToConvert);

            //Assert
            Assert.NotNull(secureString);
        }

        [Fact]
        public void ToSecureString_StringIsNull_ThrowsArgumentNullException()
        {
            //Arrange
            var sut = CreateDefault();

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                var secureString = sut.ToSecureString(null);
            });
        }

        [Theory]
        [InlineData("heasdasd232ασδασδllo !")]
        [InlineData("asdf12345!")]
        [InlineData("@#!$@%%&ασδασδ;3ςε;2ε!")]
        public void ToString_SecureStringIsNotNull_ConvertedSuccessfully(string source)
        {
            //Arrange
            var secureString = CreateSecureString(source);
            var sut = CreateDefault();

            //Act
            var actualString = sut.ToString(secureString);
            
            //Assert
            Assert.Equal(source, actualString);
        }

        [Fact]
        public void ToString_SecureStringIsNull_ThrowsArgumentNullException()
        {
            //Arrange
            var sut = CreateDefault();

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = sut.ToString(null);
            });
        }

        [Theory]
        [InlineData("heasdasd232ασδασδllo !")]
        [InlineData("asdf12345!")]
        [InlineData("@#!$@%%&ασδασδ;3ςε;2ε!")]
        [InlineData("heασδ2314ασ@#@#$@!?>}¨}{}~!@δασδllo !")]
        [InlineData("γφγφογηξοφγη=-=-076=045=67=04760-=δ=φγ=δφ")]
        public void ToSecureString_ToString_Roundtrip_ResultIsSameAsSource(string source)
        {
            //Arrange
            var sut = CreateDefault();

            //Act
            var secure = sut.ToSecureString(source);
            var unsecure = sut.ToString(secure);
            
            //Assert
            Assert.Equal(source, unsecure);
        }


        private SecureString CreateSecureString(string source)
        {
            var ss = new SecureString();
            foreach(char c in source)
            {
                ss.AppendChar(c);
            }
            return ss;
        }

    }
}
