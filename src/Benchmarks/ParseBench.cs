using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FolkerKinzel.VCards;

namespace Benchmarks;


[MemoryDiagnoser]
public class ParseBench
{

    public ParseBench()
    {
        var vc = VCardBuilder
            .Create()
            .NameViews.Add("Mustermann", "Jürgen", displayName: (dn, n) => dn.Add(n.ToDisplayName()))

            .EMails.Add("juergen@home.de")
            .VCard;
    }
}
