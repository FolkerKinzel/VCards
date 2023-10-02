using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#if !NET45
using FolkerKinzel.Strings;
#endif

namespace FolkerKinzel.VCards.Intls.Extensions.Tests;

[TestClass]
public class CharExtensionTests
{
    [TestMethod]
    public void IsDecimalDigitTest1()
    {
        for (int i = 0; i < 10; i++)
        {
            Assert.IsTrue(i.ToString()[0].IsDecimalDigit());
        }

        Assert.IsFalse(((char)((int)'0' - 1)).IsDecimalDigit());
        Assert.IsFalse(((char)((int)'9' + 1)).IsDecimalDigit());
    }
}
