using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Models.Helpers.Tests
{
    [TestClass()]
    public class FlagsEnumExtensionsTest
    {
        [TestMethod()]
        public void PropertyClassesHelperTest()
        {
            PropertyClassTypes? tp = null;

            Assert.IsFalse(tp.IsSet(PropertyClassTypes.Home));

            tp = tp.Set(PropertyClassTypes.Home);
            Assert.IsTrue(tp.IsSet(PropertyClassTypes.Home));
            Assert.IsTrue(tp.HasValue);

            // Set doppelt aufrufen
            tp = tp.Set(PropertyClassTypes.Home);
            Assert.IsTrue(tp.IsSet(PropertyClassTypes.Home));
            Assert.IsTrue(tp.HasValue);

            tp = tp.Set(PropertyClassTypes.Work);
            Assert.IsTrue(tp.IsSet(PropertyClassTypes.Work));
            Assert.IsTrue(tp.IsSet(PropertyClassTypes.Home));
            Assert.IsTrue(tp.HasValue);

            tp = tp.Unset(PropertyClassTypes.Work);
            Assert.IsFalse(tp.IsSet(PropertyClassTypes.Work));
            Assert.IsTrue(tp.IsSet(PropertyClassTypes.Home));
            Assert.IsTrue(tp.HasValue);

            // Unset doppelt aufrufen:
            tp = tp.Unset(PropertyClassTypes.Work);
            Assert.IsFalse(tp.IsSet(PropertyClassTypes.Work));
            Assert.IsTrue(tp.IsSet(PropertyClassTypes.Home));
            Assert.IsTrue(tp.HasValue);

            // letztes Flag löschen
            tp = tp.Unset(PropertyClassTypes.Home);
            Assert.IsFalse(tp.IsSet(PropertyClassTypes.Home));
            Assert.IsFalse(tp.HasValue);

            // Unset auf null aufrufen:
            tp = tp.Unset(PropertyClassTypes.Home);

            Assert.IsFalse(tp.IsSet(PropertyClassTypes.Home));
            Assert.IsFalse(tp.HasValue);
        }

        [TestMethod()]
        public void TelephoneTypesHelperTest()
        {
            TelTypes? tp = null;

            Assert.IsFalse(tp.IsSet(TelTypes.Cell));

            tp = tp.Set(TelTypes.Cell);
            Assert.IsTrue(tp.IsSet(TelTypes.Cell));
            Assert.IsTrue(tp.HasValue);

            // Set doppelt aufrufen
            tp = tp.Set(TelTypes.Cell);
            Assert.IsTrue(tp.IsSet(TelTypes.Cell));
            Assert.IsTrue(tp.HasValue);

            tp = tp.Set(TelTypes.Voice);
            Assert.IsTrue(tp.IsSet(TelTypes.Voice));
            Assert.IsTrue(tp.IsSet(TelTypes.Cell));
            Assert.IsTrue(tp.HasValue);

            tp = tp.Unset(TelTypes.Voice);
            Assert.IsFalse(tp.IsSet(TelTypes.Voice));
            Assert.IsTrue(tp.IsSet(TelTypes.Cell));
            Assert.IsTrue(tp.HasValue);

            // Unset doppelt aufrufen:
            tp = tp.Unset(TelTypes.Voice);
            Assert.IsFalse(tp.IsSet(TelTypes.Voice));
            Assert.IsTrue(tp.IsSet(TelTypes.Cell));
            Assert.IsTrue(tp.HasValue);

            // letztes Flag löschen
            tp = tp.Unset(TelTypes.Cell);
            Assert.IsFalse(tp.IsSet(TelTypes.Cell));
            Assert.IsFalse(tp.HasValue);

            // Unset auf null aufrufen:
            tp = tp.Unset(TelTypes.Cell);

            Assert.IsFalse(tp.IsSet(TelTypes.Cell));
            Assert.IsFalse(tp.HasValue);
        }

        [TestMethod()]
        public void AddressTypesHelperTest()
        {
            AddressTypes? tp = null;

            Assert.IsFalse(tp.IsSet(AddressTypes.Dom));

            tp = tp.Set(AddressTypes.Dom);
            Assert.IsTrue(tp.IsSet(AddressTypes.Dom));
            Assert.IsTrue(tp.HasValue);

            // Set doppelt aufrufen
            tp = tp.Set(AddressTypes.Dom);
            Assert.IsTrue(tp.IsSet(AddressTypes.Dom));
            Assert.IsTrue(tp.HasValue);

            tp = tp.Set(AddressTypes.Postal);
            Assert.IsTrue(tp.IsSet(AddressTypes.Postal));
            Assert.IsTrue(tp.IsSet(AddressTypes.Dom));
            Assert.IsTrue(tp.HasValue);

            tp = tp.Unset(AddressTypes.Postal);
            Assert.IsFalse(tp.IsSet(AddressTypes.Postal));
            Assert.IsTrue(tp.IsSet(AddressTypes.Dom));
            Assert.IsTrue(tp.HasValue);

            // Unset doppelt aufrufen:
            tp = tp.Unset(AddressTypes.Postal);
            Assert.IsFalse(tp.IsSet(AddressTypes.Postal));
            Assert.IsTrue(tp.IsSet(AddressTypes.Dom));
            Assert.IsTrue(tp.HasValue);

            // letztes Flag löschen
            tp = tp.Unset(AddressTypes.Dom);
            Assert.IsFalse(tp.IsSet(AddressTypes.Dom));
            Assert.IsFalse(tp.HasValue);

            // Unset auf null aufrufen:
            tp = tp.Unset(AddressTypes.Dom);

            Assert.IsFalse(tp.IsSet(AddressTypes.Dom));
            Assert.IsFalse(tp.HasValue);

        }

        [TestMethod()]
        public void RelationTypesHelperTest()
        {
            RelationTypes? tp = null;

            Assert.IsFalse(tp.IsSet(RelationTypes.Agent));

            tp = tp.Set(RelationTypes.Agent);
            Assert.IsTrue(tp.IsSet(RelationTypes.Agent));
            Assert.IsTrue(tp.HasValue);

            // Set doppelt aufrufen
            tp = tp.Set(RelationTypes.Agent);
            Assert.IsTrue(tp.IsSet(RelationTypes.Agent));
            Assert.IsTrue(tp.HasValue);

            tp = tp.Set(RelationTypes.Contact);
            Assert.IsTrue(tp.IsSet(RelationTypes.Contact));
            Assert.IsTrue(tp.IsSet(RelationTypes.Agent));
            Assert.IsTrue(tp.HasValue);

            tp = tp.Unset(RelationTypes.Contact);
            Assert.IsFalse(tp.IsSet(RelationTypes.Contact));
            Assert.IsTrue(tp.IsSet(RelationTypes.Agent));
            Assert.IsTrue(tp.HasValue);

            // Unset doppelt aufrufen:
            tp = tp.Unset(RelationTypes.Contact);
            Assert.IsFalse(tp.IsSet(RelationTypes.Contact));
            Assert.IsTrue(tp.IsSet(RelationTypes.Agent));
            Assert.IsTrue(tp.HasValue);

            // letztes Flag löschen
            tp = tp.Unset(RelationTypes.Agent);
            Assert.IsFalse(tp.IsSet(RelationTypes.Agent));
            Assert.IsFalse(tp.HasValue);

            // Unset auf null aufrufen:
            tp = tp.Unset(RelationTypes.Agent);

            Assert.IsFalse(tp.IsSet(RelationTypes.Agent));
            Assert.IsFalse(tp.HasValue);
        }

        [TestMethod()]
        public void VcfOptionsHelperTest()
        {
            VcfOptions tp = VcfOptions.None;

            Assert.IsFalse(tp.IsSet(VcfOptions.WriteGroups));
            Assert.IsTrue(tp.IsSet(VcfOptions.None));

            tp = tp.Set(VcfOptions.WriteGroups);
            Assert.IsTrue(tp.IsSet(VcfOptions.WriteGroups));
            Assert.IsFalse(tp.IsSet(VcfOptions.None));
         

            // Set doppelt aufrufen
            tp = tp.Set(VcfOptions.WriteGroups);
            Assert.IsTrue(tp.IsSet(VcfOptions.WriteGroups));
            Assert.IsFalse(tp.IsSet(VcfOptions.None));


            tp = tp.Set(VcfOptions.WriteEmptyProperties);
            Assert.IsTrue(tp.IsSet(VcfOptions.WriteEmptyProperties));
            Assert.IsTrue(tp.IsSet(VcfOptions.WriteGroups));
            Assert.IsFalse(tp.IsSet(VcfOptions.None));


            tp = tp.Unset(VcfOptions.WriteEmptyProperties);
            Assert.IsFalse(tp.IsSet(VcfOptions.WriteEmptyProperties));
            Assert.IsTrue(tp.IsSet(VcfOptions.WriteGroups));
            Assert.IsFalse(tp.IsSet(VcfOptions.None));


            // Unset doppelt aufrufen:
            tp = tp.Unset(VcfOptions.WriteEmptyProperties);
            Assert.IsFalse(tp.IsSet(VcfOptions.WriteEmptyProperties));
            Assert.IsTrue(tp.IsSet(VcfOptions.WriteGroups));
            Assert.IsFalse(tp.IsSet(VcfOptions.None));


            // letztes Flag löschen
            tp = tp.Unset(VcfOptions.WriteGroups);
            Assert.IsFalse(tp.IsSet(VcfOptions.WriteGroups));
            Assert.IsTrue(tp.IsSet(VcfOptions.None));


            // Unset auf None aufrufen:
            tp = tp.Unset(VcfOptions.WriteGroups);

            Assert.IsFalse(tp.IsSet(VcfOptions.WriteGroups));
            Assert.IsTrue(tp.IsSet(VcfOptions.None));

        }

        
    }
}