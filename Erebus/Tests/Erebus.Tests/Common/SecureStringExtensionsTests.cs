using Erebus.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security;
using System.Diagnostics;

namespace Erebus.Tests.Common
{
    [TestFixture]
    public class SecureStringExtensionsTests
    {

        [TestCase("hello !")]
        [TestCase("asdf12345!")]
        [TestCase("@#!$@%%&ασδασδ;3ςε;2ε!")]
        public void ToSecureString_StringIsNotNull_SecureStringCreated(string stringToConvert)
        {
            //Arrange
            //Act
            var secureString = SecureStringExtensions.ToSecureString(stringToConvert);

            //Assert
            Assert.IsNotNull(secureString);
        }

        
        public void ToSecureString_StringIsNull_ThrowsArgumentNullException()
        {
            //Arrange
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                var secureString = SecureStringExtensions.ToSecureString(null);
            });
        }

        [TestCase("heasdasd232ασδασδllo !")]
        [TestCase("asdf12345!")]
        [TestCase("@#!$@%%&ασδασδ;3ςε;2ε!")]
        public void ToActualString_SecureStringIsNotNull_ConvertedSuccessfully(string source)
        {
            //Arrange
            var secureString = CreateSecureString(source);
            
            //Act
            var actualString = SecureStringExtensions.ToActualString(secureString);
            
            //Assert
            Assert.AreEqual(source, actualString);
        }

        public void ToActualString_SecureStringIsNull_ThrowsArgumentNullException()
        {
            //Arrange
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = SecureStringExtensions.ToActualString(null);
            });
        }

        [TestCase("heasdasd232ασδασδllo !")]
        [TestCase("asdf12345!")]
        [TestCase("@#!$@%%&ασδασδ;3ςε;2ε!")]
        [TestCase("heασδ2314ασ@#@#$@!?>}¨}{}~!@δασδllo !")]
        [TestCase("γφγφογηξοφγη=-=-076=045=67=04760-=δ=φγ=δφ")]
        public void ToSecureString_ToActualString_Roundtrip_ResultIsSameAsSource(string source)
        {
            //Arrange
            //Act
            var secure = SecureStringExtensions.ToSecureString(source);
            var unsecure = SecureStringExtensions.ToActualString(secure);
            
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
