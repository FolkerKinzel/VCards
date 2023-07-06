using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using FolkerKinzel.VCards.Tests;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class VCardPropertyTests
{
    private class ArgumentNullTester : VCardProperty
    {
        public ArgumentNullTester(ParameterSection parameters) : base(parameters, null)
        {
            
        }

        public override object Clone() => throw new NotImplementedException();
        protected override object? GetVCardPropertyValue() => throw new NotImplementedException();
        internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CtorTest1() => _ = new ArgumentNullTester(null!);
}

[TestClass()]
public class TextPropertyTests
{
    //[TestMethod()]
    //public void TextPropertyTest()
    //{

    //}

    //[TestMethod()]
    //public void CloneTest()
    //{

    //}

    [TestMethod]
    public void IEnumerableTest()
    {
        var tProp = new TextProperty("Good value");

        TextProperty value = tProp.AsWeakEnumerable().Cast<TextProperty>().First();

        Assert.AreSame(tProp, value);

    }

    [TestMethod]
    public void IEnumerableTTest()
    {
        var tProp = new TextProperty("Good value");

        TextProperty value = tProp.First();

        Assert.AreSame(tProp, value);

    }


    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new TextProperty(null).ToString());
}