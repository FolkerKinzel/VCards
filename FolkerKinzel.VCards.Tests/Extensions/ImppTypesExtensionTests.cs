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
    public class ImppTypesExtensionTests
    {
        [TestMethod()]
        public void ImppTypesTest()
        {
            ImppTypes? tp = null;

            Assert.IsFalse(tp.IsSet(ImppTypes.Personal));

            tp = tp.Set(ImppTypes.Personal);
            Assert.IsTrue(tp.IsSet(ImppTypes.Personal));
            Assert.IsTrue(tp.HasValue);

            // Set doppelt aufrufen
            tp = tp.Set(ImppTypes.Personal);
            Assert.IsTrue(tp.IsSet(ImppTypes.Personal));
            Assert.IsTrue(tp.HasValue);

            tp = tp.Set(ImppTypes.Business);
            Assert.IsTrue(tp.IsSet(ImppTypes.Business));
            Assert.IsTrue(tp.IsSet(ImppTypes.Personal));
            Assert.IsTrue(tp.HasValue);

            tp = tp.Unset(ImppTypes.Business);
            Assert.IsFalse(tp.IsSet(ImppTypes.Business));
            Assert.IsTrue(tp.IsSet(ImppTypes.Personal));
            Assert.IsTrue(tp.HasValue);

            // Unset doppelt aufrufen:
            tp = tp.Unset(ImppTypes.Business);
            Assert.IsFalse(tp.IsSet(ImppTypes.Business));
            Assert.IsTrue(tp.IsSet(ImppTypes.Personal));
            Assert.IsTrue(tp.HasValue);

            // letztes Flag löschen
            tp = tp.Unset(ImppTypes.Personal);
            Assert.IsFalse(tp.IsSet(ImppTypes.Personal));
            Assert.IsFalse(tp.HasValue);

            // Unset auf null aufrufen:
            tp = tp.Unset(ImppTypes.Personal);

            Assert.IsFalse(tp.IsSet(ImppTypes.Personal));
            Assert.IsFalse(tp.HasValue);
        }
    }
}