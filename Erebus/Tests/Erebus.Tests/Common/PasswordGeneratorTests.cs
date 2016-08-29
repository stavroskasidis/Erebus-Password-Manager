using Erebus.Core.Implementations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Erebus.Tests.Common
{
    public class PasswordGeneratorTests
    {

        //[SetUp]
        //public void Setup()
        //{
        //    this.Key = new SecureString();
        //    foreach (char c in "123456")
        //    {
        //        Key.AppendChar(c);
        //    }

        //    this.SecondKey = new SecureString();
        //    foreach (char c in "654321")
        //    {
        //        SecondKey.AppendChar(c);
        //    }

        //    SecureStringConverterMock = new Mock<ISecureStringConverter>(MockBehavior.Strict);
        //    SecureStringConverterMock.Setup(mock => mock.ToString(this.Key)).Returns("123456");
        //    SecureStringConverterMock.Setup(mock => mock.ToString(this.SecondKey)).Returns("654321");
        //}

        //[TearDown]
        //public void TearDown()
        //{
        //    this.Key.Dispose();
        //}


        private PasswordGenerator CreateDefault()
        {
            var defaultInstance = new PasswordGenerator();
            return defaultInstance;
        }

        [TestCase(10, true,  false, true, true)]
        [TestCase(210, false,true, true, false)]
        [TestCase(30, true,  true, true, false)]
        [TestCase(40, true,  true, false, false)]
        [TestCase(5, false,  false, false, true)]
        [TestCase(4, false,  true, false, false)]
        [TestCase(7, false,  true, true, false)]
        [TestCase(6, true,   false, false, true)]
        [TestCase(32, true,  false, false, false)]
        public void GeneratePassword_GenerateAPasswordWithGivenInput_PasswordFulfilsRequirements(int length, bool includeUppercase, bool includeLowercase, bool includeNumbers, bool includeSymbols)
        {
            //Arrange
            var sut = CreateDefault();

            //Act
            var password = sut.GeneratePassword(length, includeUppercase, includeLowercase, includeNumbers, includeSymbols);

            //Assert
            Assert.AreEqual(length, password.Length,"Invalid length");
            Assert.AreEqual(includeUppercase, password.Any(char.IsUpper), "Invalid uppercase requirement");
            Assert.AreEqual(includeLowercase, password.Any(char.IsLower), "Invalid lowercase requirement");
            Assert.AreEqual(includeNumbers, password.Any(char.IsDigit), "Invalid numbers requirement");
            Assert.AreEqual(includeSymbols, password.Any(c=> !char.IsLetterOrDigit(c)), "Invalid symbols requirement");
            
        }



        [Test]
        public void GeneratePassword_GenerateManyPasswordsSequentiallyWithTheSameParameters_NoDuplicates()
        {
            //Arrange
            var sut = CreateDefault();
            List<string> generatedPasswords = new List<string>();
            int passwordsToGenerate = 40;

            //Act

            for (int i = 0; i < passwordsToGenerate; i++)
            {
                generatedPasswords.Add(sut.GeneratePassword(10, true, true, true, true));
            }

            //Assert
            bool isUnique = generatedPasswords.Distinct().Count() == generatedPasswords.Count();
            Assert.AreEqual(true, isUnique);
        }

    }
}
