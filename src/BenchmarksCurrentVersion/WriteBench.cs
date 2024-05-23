using System.Collections;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;

namespace Benchmarks;


[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net48)]
public class WriteBench
{
    private readonly VCard _vCard;
    private readonly VCard _vCardPhoto;
    

    public WriteBench()
    {
        _vCard = VCardProvider.CreateVCard();
        _vCardPhoto = VCardProvider.CreatePhotoVCard();
    }

    [Benchmark]
    public string Write21() => Vcf.ToString(_vCard, VCdVersion.V2_1);

    [Benchmark]
    public string WritePhoto21() => Vcf.ToString(_vCardPhoto, VCdVersion.V2_1);

    [Benchmark]
    public string Write30() => Vcf.ToString(_vCard);

    [Benchmark]
    public string WritePhoto30() => Vcf.ToString(_vCardPhoto);

    [Benchmark]
    public string Write40() => Vcf.ToString(_vCard, VCdVersion.V4_0, options: Opts.Default.Set(Opts.SetPropertyIDs));

    [Benchmark]
    public string WritePhoto40() => Vcf.ToString(_vCardPhoto, VCdVersion.V4_0, options: Opts.Default.Set(Opts.SetPropertyIDs));
}
