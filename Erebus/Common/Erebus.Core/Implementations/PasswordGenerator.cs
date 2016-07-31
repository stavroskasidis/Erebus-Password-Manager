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
        public string GeneratePassword(int length, bool includeUppercase, bool includeNumbers, bool includeSymbols)
        {
            while (true)
            {
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    StringBuilder sourceBuilder = new StringBuilder("abcdefghijklmnopqrstuvwxyz");
                    if (includeUppercase)
                    {
                        sourceBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                    }
                    if (includeNumbers)
                    {
                        sourceBuilder.Append("0123456789");
                    }
                    if (includeSymbols)
                    {
                        sourceBuilder.Append("!@#$%^&*()_");
                    }

                    string source = sourceBuilder.ToString();
                    StringBuilder passwordBuilder = new StringBuilder();
                    for (int i = 0; i < length; i++)
                    {
                        int randomIndex = GetRandomNumber(0, source.Length, rng);
                        passwordBuilder.Append(source[randomIndex]);
                    }

                    var password = passwordBuilder.ToString();
                    if (PasswordMeetsRequirements(password, includeUppercase, includeNumbers, includeSymbols))
                    {

                        return password;
                    }

                }
            }
        }

        private bool PasswordMeetsRequirements(string password, bool includeUppercase, bool includeNumbers, bool includeSymbols)
        {
            return (includeUppercase == password.Any(x => char.IsUpper(x)))
                && (includeNumbers == password.Any(x => char.IsDigit(x)))
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
