using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;

namespace Benchmarks;


[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
//[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net48)]
public class ParseBench
{
    private readonly string _vCardString21;
    private readonly string _vCardString30;
    private readonly string _vCardString40;
    private readonly string _vCardStringPhoto21;
    private readonly string _vCardStringPhoto30;
    private readonly string _vCardStringPhoto40;

    public ParseBench()
    {
        VCard _vCard = VCardProvider.CreateVCard();
        VCard _vCardPhoto = VCardProvider.CreatePhotoVCard();

        _vCardString21 = _vCard.ToVcfString(VCdVersion.V2_1);
        _vCardString30 = _vCard.ToVcfString();
        _vCardString40 = _vCard.ToVcfString(VCdVersion.V4_0, options: VcfOpts.Default.Set(VcfOpts.SetPropertyIDs));
        _vCardStringPhoto21 = _vCardPhoto.ToVcfString(VCdVersion.V2_1);
        _vCardStringPhoto30 = _vCardPhoto.ToVcfString();
        _vCardStringPhoto40 = _vCardPhoto.ToVcfString(VCdVersion.V4_0, options: VcfOpts.Default.Set(VcfOpts.SetPropertyIDs));
    }

    [Benchmark]
    public object Parse21() => Vcf.Parse(_vCardString21);

    [Benchmark]
    public object Parse21Photo() => Vcf.Parse(_vCardStringPhoto21);

    [Benchmark]
    public object Parse30() => Vcf.Parse(_vCardString30);

    [Benchmark]
    public object Parse30Photo() => Vcf.Parse(_vCardStringPhoto30);

    [Benchmark]
    public object Parse40() => Vcf.Parse(_vCardString40);

    [Benchmark]
    public object Parse40Photo() => Vcf.Parse(_vCardStringPhoto40);
}
