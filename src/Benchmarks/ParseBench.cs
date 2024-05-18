using System.Collections;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;

namespace Benchmarks;


[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80, launchCount: 1, warmupCount: 2, iterationCount: 2, invocationCount: 2)]
public class ParseBench
{
    private readonly VCard _vCard;
    private readonly VCard _vCardPhoto;
    private readonly string _vCardString21;
    private readonly string _vCardString30;
    private readonly string _vCardString40;
    private readonly string _vCardStringPhoto21;
    private readonly string _vCardStringPhoto30;
    private readonly string _vCardStringPhoto40;

    public ParseBench()
    {
        _vCard = VCardBuilder
            .Create()
            .NameViews.Add("Mustermann", "Jürgen", displayName: (dn, n) => dn.Add(n.ToDisplayName()))

            .EMails.Add("juergen@home.de", parameters: p => p.PropertyClass = PCl.Home)
            .EMails.Add("juergen@work.com", parameters: p => p.PropertyClass = PCl.Work)
            .EMails.SetPreferences()

            .Phones.Add("01234-56789876", parameters: p =>  p.PhoneType = Tel.Cell)
            .Phones.Add("09876-54321234", parameters: p => { p.PropertyClass = PCl.Home; p.PhoneType = Tel.Voice | Tel.Fax; })
            .Phones.SetPreferences()

            .Addresses.Add("Blümchenweg 14a", "Göppingen", null, "73035")
            .BirthDayViews.Add(1985, 5, 7)
            .VCard;

        _vCardPhoto = VCardBuilder
            .Create((VCard)_vCard.Clone())
            .Photos.AddFile(@"..\..\..\..\..\..\..\TestFiles\Folker.png")
            .VCard;

        _vCardString21 = _vCard.ToVcfString(VCdVersion.V2_1);
        _vCardString30 = _vCard.ToVcfString();
        _vCardString40 = _vCard.ToVcfString(VCdVersion.V4_0, options: Opts.Default.Set(Opts.SetPropertyIDs));
        _vCardStringPhoto21 = _vCardPhoto.ToVcfString(VCdVersion.V2_1);
        _vCardStringPhoto30 = _vCardPhoto.ToVcfString();
        _vCardStringPhoto40 = _vCardPhoto.ToVcfString(VCdVersion.V4_0, options: Opts.Default.Set(Opts.SetPropertyIDs));
    }

    [Benchmark]
    public IList<VCard> Parse21() => Vcf.Parse(_vCardString21);
}
