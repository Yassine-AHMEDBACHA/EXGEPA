using System.Text;

using System.Security.Cryptography;
using System.ComponentModel.Composition;
using CORESI.Tools.Collections;
using CORESI.Data.Tools;

namespace CORESI.Security
{
    [Export(typeof(IStringCryptor))]
    public class StringCryptor : IStringCryptor
    {
        private readonly MD5CryptoServiceProvider mD5CryptoServiceProvider;

        private readonly StringBuilder stringBuilder;

        public StringCryptor()
        {
            this.mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            this.stringBuilder = new StringBuilder();
        }

        public string Crypte(string value)
        {
            if (value.IsNotValid())
            {
                return value;
            }

            mD5CryptoServiceProvider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(value.ToString()));
            var result = mD5CryptoServiceProvider.Hash;
            stringBuilder.Clear();
            result.ForEach(x => stringBuilder.Append(x.ToString("x2")));

            return stringBuilder.ToString();
        }

    }
}
