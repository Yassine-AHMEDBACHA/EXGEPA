using EXGEPA.Model;
using NUnit.Framework;
using System;
using System.Linq;
using CORESI.Data;
using CORESI.IoC;

namespace CORESI.DataAccess.Core.Tests
{
    [TestFixture]
    public class FieldExtractorTest
    {
        [Test]
        public void Should_Map_All_Property_Infos_With_Fields()
        {
            System.Collections.Generic.List<Field> fields = typeof(Reference).GetAllFields();
            System.Collections.Generic.List<System.Reflection.PropertyInfo> properties = typeof(Reference).GetProperties().ToList();
            Assert.AreEqual(properties.Count - 1, fields.Count);
        }

        [Test]
        public void ShouldGetReferenceFields()
        {
            System.Collections.Generic.List<Field> fields = typeof(Reference).GetAllFields();
            System.Collections.Generic.List<Field> referances = fields.Where(x => x.IsReference).ToList();
            Assert.AreEqual(4, referances.Count());
        }

        [TestCase(typeof(GeneralAccount))]
        [TestCase(typeof(Reference))]
        [TestCase(typeof(Person))]
        [TestCase(typeof(Provider))]
        public void Should_Return_TrueWhen_Type_Is(Type type)
        {
            bool result = type.IsReference();
            Assert.AreEqual(true, result);
        }
        [Test]
        public void Should_IndicateIfColumnMustBeUnique()
        {
            System.Collections.Generic.List<Field> fields = typeof(GeneralAccount).GetAllFields();
            System.Collections.Generic.List<Field> uniquefields = fields.Where(x => x.IsUnique).ToList();
            Assert.AreEqual(2, uniquefields.Count);
        }

        [Test]
        public void Should_tte()
        {
            Type typeR = typeof(IDataProvider<Item>);

            Type type = typeof(IDataProvider<>);
            type = type.MakeGenericType(typeof(Item));

            Assert.AreEqual(typeR, type);
            object instance = ServiceLocator.Resolve(type);
            Assert.IsNotInstanceOf(typeof(IDataProvider<Item>), instance);
        }

    }
}
