using EXGEPA.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
            var fields = typeof(Reference).GetAllFields();
            var properties = typeof(Reference).GetProperties().ToList();
            Assert.AreEqual(properties.Count-1, fields.Count);
        }

        [Test]
        public void ShouldGetReferenceFields()
        {
            var fields = typeof(Reference).GetAllFields();
            var referances = fields.Where(x => x.IsReference).ToList();
            Assert.AreEqual(4, referances.Count());
        }

        [TestCase(typeof(GeneralAccount))]
        [TestCase(typeof(Reference))]
        [TestCase(typeof(Person))]
        [TestCase(typeof(Provider))]
        public void Should_Return_TrueWhen_Type_Is(Type type)
        {
            var result = type.IsReference();
            Assert.AreEqual(true, result);
        }
        [Test]
        public void Should_IndicateIfColumnMustBeUnique()
        {
            var fields = typeof(GeneralAccount).GetAllFields();
            var uniquefields = fields.Where(x => x.IsUnique).ToList();
            Assert.AreEqual(2, uniquefields.Count);
        }

        [Test]
        public void Should_tte()
        {
           var typeR = typeof(IDataProvider<Item>);

           var type = typeof(IDataProvider<>);
            type = type.MakeGenericType(typeof(Item));

            Assert.AreEqual(typeR, type);
           var instance = ServiceLocator.Resolve(type);
           Assert.IsNotInstanceOf(typeof(IDataProvider<Item>), instance);
        }

    }
}
