using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Extensions.Tests
{
    [TestClass()]
    public class AddressTypesTests
    {
        [TestMethod()]
        public void AddressTypesTest()
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
    }
}