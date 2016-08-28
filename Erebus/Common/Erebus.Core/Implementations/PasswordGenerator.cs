using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Implementations
{
    public class PasswordGenerator : IPasswordGenerator
    {
        public string GeneratePassword(int length, bool includeUppercase, bool includeLowerCase, bool includeDigits, bool includeSymbols)
        {
            if(length < 4)
            {
                throw new Exception("Minimum password length is 4");
            }

            while (true)
            {
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    StringBuilder sourceBuilder = new StringBuilder();
                    if (includeLowerCase)
                    {
                        sourceBuilder.Append("abcdefghijklmnopqrstuvwxyz");
                    }
                    if (includeUppercase)
                    {
                        sourceBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                    }
                    if (includeDigits)
                    {
                        sourceBuilder.Append("0123456789");
                    }
                    if (includeSymbols)
                    {
                        sourceBuilder.Append("!@#$%^&*()_");
                    }

                    if (sourceBuilder.Length == 0) throw new Exception("Too few sources");

                    string source = sourceBuilder.ToString();
                    StringBuilder passwordBuilder = new StringBuilder();
                    for (int i = 0; i < length; i++)
                    {
                        int randomIndex = GetRandomNumber(0, source.Length, rng);
                        passwordBuilder.Append(source[randomIndex]);
                    }

                    var password = passwordBuilder.ToString();
                    if (PasswordMeetsRequirements(password, includeUppercase, includeLowerCase, includeDigits, includeSymbols))
                    {

                        return password;
                    }
                }

            }
        }

        private bool PasswordMeetsRequirements(string password, bool includeUppercase, bool includeLowerCase, bool includeDigits, bool includeSymbols)
        {
            return (includeUppercase == password.Any(x => char.IsUpper(x)))
                && (includeLowerCase == password.Any(x => char.IsLower(x)))
                && (includeDigits == password.Any(x => char.IsDigit(x)))
                && (includeSymbols == password.Any(x => !char.IsLetterOrDigit(x)));
        }

        private int GetRandomNumber(int minValue, int maxValue, RandomNumberGenerator random)
        {
            byte[] data = new byte[8];
            ulong value;
            do
            {
                random.GetBytes(data);
                value = BitConverter.ToUInt64(data, 0);
            } while (value == 0);
            return (int)(value % (ulong)maxValue + (ulong)minValue);
        }
    }
}
