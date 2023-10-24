using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Extensions;

namespace Benchmarks;

public class VCardBench
{
    private const string VCARD_PATH = @"C:\Users\fkinz\OneDrive\Kontakte\Thunderbird\21-01-13.vcf";
    private readonly string _vcardString;

    public VCardBench() => this._vcardString = File.ReadAllText(VCARD_PATH);

    //[Benchmark]
    //public List<VCard> LoadLongVcf() => VCard.Load(VCARD_PATH);

    [Benchmark]
    public string ParseAndSerialize()
    {
        IList<VCard> list = VCard.ParseVcf(_vcardString);
        return list.ToVcfString();
    }

}
