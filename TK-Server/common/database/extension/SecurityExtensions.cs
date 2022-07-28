using System.Security.Cryptography;
using System.Text;

namespace common.database.extension
{
    public static class SecurityExtensions
    {
        public static byte[] Sha1Digest(this string text) => new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(text));
    }
}
