using System.Text;

using System.Security.Cryptography;
using System.ComponentModel.Composition;

namespace CORESI.Security
{
    [Export(typeof(IStringCryptor))]
    public class StringCryptor : IStringCryptor
    {
        MD5CryptoServiceProvider mD5CryptoServiceProvider { get; set; }
        StringBuilder stringBuilder = new StringBuilder();

        public StringCryptor()
        {
            mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
        }

        public string Crypte(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            mD5CryptoServiceProvider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(value.ToString()));
            var result = mD5CryptoServiceProvider.Hash;
            stringBuilder.Clear();
            foreach (var item in result)
            {
                stringBuilder.Append(item.ToString("x2"));
            }
            return stringBuilder.ToString();
        }

    }
}
