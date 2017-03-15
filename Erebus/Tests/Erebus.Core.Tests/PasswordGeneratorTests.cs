using Erebus.Core.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace Erebus.Tests.Common
{
    public class PasswordGeneratorTests
    {
        private PasswordGenerator CreateDefault()
        {
            var defaultInstance = new PasswordGenerator();
            return defaultInstance;
        }

        [Theory]
        [InlineData(10, true,  false, true, true)]
        [InlineData(210, false,true, true, false)]
        [InlineData(30, true,  true, true, false)]
        [InlineData(40, true,  true, false, false)]
        [InlineData(5, false,  false, false, true)]
        [InlineData(4, false,  true, false, false)]
        [InlineData(7, false,  true, true, false)]
        [InlineData(6, true,   false, false, true)]
        [InlineData(32, true,  false, false, false)]
        public void GeneratePassword_GenerateAPasswordWithGivenInput_PasswordFulfilsRequirements(int length, bool includeUppercase, bool includeLowercase, bool includeNumbers, bool includeSymbols)
        {
            //Arrange
            var sut = CreateDefault();

            //Act
            var password = sut.GeneratePassword(length, includeUppercase, includeLowercase, includeNumbers, includeSymbols);

            //Assert
            Assert.Equal(length, password.Length);
            Assert.Equal(includeUppercase, password.Any(char.IsUpper));
            Assert.Equal(includeLowercase, password.Any(char.IsLower));
            Assert.Equal(includeNumbers, password.Any(char.IsDigit));
            Assert.Equal(includeSymbols, password.Any(c=> !char.IsLetterOrDigit(c)));
            
        }



        [Fact]
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
            Assert.True(isUnique);
        }

    }
}
