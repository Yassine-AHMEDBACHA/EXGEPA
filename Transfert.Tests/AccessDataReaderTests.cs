using EXGEPA.DataLoader.Access;
using EXGEPA.Model;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Transfert.Tests
{
    [TestFixture]
    public class AccessDataReaderTests
    {
        [Ignore("Inutile")]
        public void TestLoadData()
        {
            var filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\AccessDb.accdb";
            Assert.IsTrue(File.Exists(filePath));
            var result = AccessDatabaseReader.SelectAll<Item>(filePath, "Select * From equip", (reader) =>
              {
                  return new Item() { Key = reader["Code"].ToString() };
              });
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }
    }
}
