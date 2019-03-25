// <copyright file="StringHelperTests.cs" company="PlaceholderCompany">
// Copyright (c) CORESI. All rights reserved.
// </copyright>

namespace CORESI.Tools.Tests
{
    using CORESI.Tools.StringTools;
    using NUnit.Framework;

    [TestFixture]
    public class StringHelperTests
    {
        [Test]
        public void TestAlign()
        {
            string expectedString = "**1254";
            string str = "1254";
            var actualString = str.Align(6, "*");
            Assert.AreEqual(expectedString, actualString);
        }
    }
}