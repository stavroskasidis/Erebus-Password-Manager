using Erebus.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security;
using System.Diagnostics;
using Erebus.Core.Implementations;

namespace Erebus.Tests.Common
{
    [TestFixture]
    public class SecureStringConverterTests
    {

        private SecureStringConverter CreateDefault()
        {
            var defaultInstance = new SecureStringConverter();
            return defaultInstance;
        }

        [TestCase("hello !")]
        [TestCase("asdf12345!")]
        [TestCase("@#!$@%%&ασδασδ;3ςε;2ε!")]
        public void ToSecureString_StringIsNotNull_SecureStringCreated(string stringToConvert)
        {
            //Arrange
            var sut = CreateDefault();

            //Act
            var secureString = sut.ToSecureString(stringToConvert);

            //Assert
            Assert.IsNotNull(secureString);
        }

        
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

        [TestCase("heasdasd232ασδασδllo !")]
        [TestCase("asdf12345!")]
        [TestCase("@#!$@%%&ασδασδ;3ςε;2ε!")]
        public void ToString_SecureStringIsNotNull_ConvertedSuccessfully(string source)
        {
            //Arrange
            var secureString = CreateSecureString(source);
            var sut = CreateDefault();

            //Act
            var actualString = sut.ToString(secureString);
            
            //Assert
            Assert.AreEqual(source, actualString);
        }

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

        [TestCase("heasdasd232ασδασδllo !")]
        [TestCase("asdf12345!")]
        [TestCase("@#!$@%%&ασδασδ;3ςε;2ε!")]
        [TestCase("heασδ2314ασ@#@#$@!?>}¨}{}~!@δασδllo !")]
        [TestCase("γφγφογηξοφγη=-=-076=045=67=04760-=δ=φγ=δφ")]
        public void ToSecureString_ToString_Roundtrip_ResultIsSameAsSource(string source)
        {
            //Arrange
            var sut = CreateDefault();

            //Act
            var secure = sut.ToSecureString(source);
            var unsecure = sut.ToString(secure);
            
            //Assert
            Assert.AreEqual(source, unsecure);
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
