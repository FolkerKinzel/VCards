using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests.Intls.Deserializers
{
    [TestClass]
    public class AssignerTests
    {
        [TestMethod]
        public void AssignerTest1()
        {
            var vcard = new VCard();
            var textProp = new RelationTextProperty("Hallo");

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
    }
}
