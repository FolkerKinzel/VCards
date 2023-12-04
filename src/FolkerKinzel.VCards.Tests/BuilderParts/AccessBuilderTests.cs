using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class XmlBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new XmlBuilder().Add(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new XmlBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new XmlBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new XmlBuilder().Equals((XmlBuilder?)null));

        var builder = new XmlBuilder();
        Assert.AreEqual(builder.GetHashCode(),((object) builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new XmlBuilder().ToString());
}

[TestClass]
public class UuidBuilderTests
{
    

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetTest1() => new UuidBuilder().Set(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new UuidBuilder().Clear();

    

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new UuidBuilder().Equals((UuidBuilder?)null));

        var builder = new UuidBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new UuidBuilder().ToString());
}

[TestClass]
public class TimeZoneBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new TimeZoneBuilder().Add(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new TimeZoneBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new TimeZoneBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new TimeZoneBuilder().Equals((TimeZoneBuilder?)null));

        var builder = new TimeZoneBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new TimeZoneBuilder().ToString());
}

[TestClass]
public class TimeStampBuilderTests
{
    

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetTest1() => new TimeStampBuilder().Set(DateTimeOffset.UtcNow);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new TimeStampBuilder().Clear();

    

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new TimeStampBuilder().Equals((TimeStampBuilder?)null));

        var builder = new TimeStampBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new TimeStampBuilder().ToString());
}

[TestClass]
public class TextViewBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new TextViewBuilder().Add(null);

    

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new TextViewBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new TextViewBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new TextViewBuilder().Equals((TextViewBuilder?)null));

        var builder = new TextViewBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new TextViewBuilder().ToString());
}

[TestClass]
public class TextSingletonBuilderTests
{
    

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetTest1() => new TextSingletonBuilder().Set(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new TextSingletonBuilder().Clear();

    

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new TextSingletonBuilder().Equals((TextSingletonBuilder?)null));

        var builder = new TextSingletonBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new TextSingletonBuilder().ToString());
}


[TestClass]
public class TextBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new TextBuilder().Add(null);

    

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new TextBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new TextBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new TextBuilder().Equals((TextBuilder?)null));

        var builder = new TextBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new TextBuilder().ToString());
}


[TestClass]
public class StringCollectionBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new StringCollectionBuilder().Add((string?)null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest2() => new StringCollectionBuilder().Add(Enumerable.Empty<string>());

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new StringCollectionBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new StringCollectionBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new StringCollectionBuilder().Equals((StringCollectionBuilder?)null));

        var builder = new StringCollectionBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new StringCollectionBuilder().ToString());
}


[TestClass]
public class RelationBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new RelationBuilder().Add(Guid.Empty);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest2() => new RelationBuilder().Add("");

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest3() => new RelationBuilder().Add((Uri?)null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest4() => new RelationBuilder().Add((VCard?)null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new RelationBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new RelationBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new RelationBuilder().Equals((RelationBuilder?)null));

        var builder = new RelationBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new RelationBuilder().ToString());
}

[TestClass]
public class ProfileBuilderTests
{
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetTest1() => new ProfileBuilder().Set(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new ProfileBuilder().Clear();

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new ProfileBuilder().Equals((ProfileBuilder?)null));

        var builder = new ProfileBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new ProfileBuilder().ToString());
}

[TestClass]
public class OrgBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new OrgBuilder().Add(null);

    

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new OrgBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new OrgBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new OrgBuilder().Equals((OrgBuilder?)null));

        var builder = new OrgBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new OrgBuilder().ToString());
}


[TestClass]
public class NonStandardBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new NonStandardBuilder().Add("X-TEST", null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new NonStandardBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new NonStandardBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new NonStandardBuilder().Equals((NonStandardBuilder?)null));

        var builder = new NonStandardBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new NonStandardBuilder().ToString());
}

[TestClass]
public class NameBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new NameBuilder().Add((string?)null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest2() => new NameBuilder().Add(Enumerable.Empty<string>());

    

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new NameBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new NameBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new NameBuilder().Equals((NameBuilder?)null));

        var builder = new NameBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new NameBuilder().ToString());
}

[TestClass]
public class KindBuilderTests
{
    

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetTest1() => new KindBuilder().Set(Enums.Kind.Individual);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new KindBuilder().Clear();

    

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new KindBuilder().Equals((KindBuilder?)null));

        var builder = new KindBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new KindBuilder().ToString());
}

[TestClass]
public class GeoBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new GeoBuilder().Add(null);

    

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new GeoBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new GeoBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new GeoBuilder().Equals((GeoBuilder?)null));

        var builder = new GeoBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new GeoBuilder().ToString());
}

[TestClass]
public class GenderBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new GenderBuilder().Add(null);

    

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new GenderBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new GenderBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new GenderBuilder().Equals((GenderBuilder?)null));

        var builder = new GenderBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new GenderBuilder().ToString());
}

[TestClass]
public class DateAndOrTimeBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new DateAndOrTimeBuilder().Add(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest2() => new DateAndOrTimeBuilder().Add(new DateOnly(2023, 12, 4));

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest3() => new DateAndOrTimeBuilder().Add(2023, 12, 4);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest4() => new DateAndOrTimeBuilder().Add(12, 4);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest5() => new DateAndOrTimeBuilder().Add(DateTimeOffset.Now);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest6() => new DateAndOrTimeBuilder().Add(TimeOnly.FromDateTime(DateTime.Now));

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new DateAndOrTimeBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new DateAndOrTimeBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new DateAndOrTimeBuilder().Equals((DateAndOrTimeBuilder?)null));

        var builder = new DateAndOrTimeBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new DateAndOrTimeBuilder().ToString());
}

[TestClass]
public class DataBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddFileTest1() => new DataBuilder().AddFile("file");

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddUriTest1() => new DataBuilder().AddUri(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddBytesTest1() => new DataBuilder().AddBytes(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTextTest1() => new DataBuilder().AddText(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new DataBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new DataBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new DataBuilder().Equals((DataBuilder?)null));

        var builder = new DataBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new DataBuilder().ToString());
}

[TestClass]
public class AddressBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new AddressBuilder().Add("", null, null, null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest2() => new AddressBuilder().Add(Enumerable.Empty<string>(), null, null, null);


    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new AddressBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new AddressBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new AddressBuilder().Equals((AddressBuilder?)null));

        var builder = new AddressBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new AddressBuilder().ToString());
}

[TestClass]
public class AccessBuilderTests
{
    

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetTest1() => new AccessBuilder().Set(Enums.Access.Public);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new AccessBuilder().Clear();

    

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new AccessBuilder().Equals((AccessBuilder?)null));

        var builder = new AccessBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new AccessBuilder().ToString());
}
