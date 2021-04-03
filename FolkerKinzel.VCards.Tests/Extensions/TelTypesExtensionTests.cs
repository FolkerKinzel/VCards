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
    public class TelTypesExtensionTests
    {
        [TestMethod()]
        public void TelTypesTest()
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
    }
}