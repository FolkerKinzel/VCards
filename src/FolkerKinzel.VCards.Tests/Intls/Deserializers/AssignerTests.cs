using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Deserializers.Tests;

[TestClass]
public class AssignerTests
{
    [TestMethod]
    public void AssignerTest1()
    {
        var vcard = new VCard();
        var textProp = RelationProperty.FromText("Hallo");

        IEnumerable<RelationProperty>? assignment = Assigner.GetAssignment(textProp, vcard.Relations);
        Assert.AreSame(textProp, assignment);

        vcard.Relations = assignment;

        assignment = Assigner.GetAssignment(textProp, vcard.Relations);
        Assert.AreNotSame(textProp, assignment);
        Assert.IsInstanceOfType(assignment, typeof(List<RelationProperty>));

        IEnumerable<RelationProperty>? list = assignment;

        vcard.Relations = assignment;

        assignment = Assigner.GetAssignment(textProp, vcard.Relations);
        Assert.IsInstanceOfType(assignment, typeof(List<RelationProperty>));
        Assert.AreSame(list, assignment);
        Assert.AreEqual(3, assignment.Count());
    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void AssignTest2() => _ = new TextProperty("test").GetAssignment(new TextProperty[1]);
}
