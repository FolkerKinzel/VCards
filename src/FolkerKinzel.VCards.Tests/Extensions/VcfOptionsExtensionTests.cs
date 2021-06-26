using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Extensions.Tests
{
    [TestClass()]
    public class VcfOptionsExtensionTests
    {
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