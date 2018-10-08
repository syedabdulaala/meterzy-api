using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Meterzy.Api.Extension
{
    public static class CryptoExtension
    {
        public static string ToSha256Hash(this string input, string salt = null)
        {
            var sha256 = SHA256.Create();
            var data = sha256.ComputeHash(Encoding.UTF8.GetBytes(salt == null ? input : salt + input));
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
