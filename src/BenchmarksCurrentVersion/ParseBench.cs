using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;

namespace Benchmarks;


[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80, launchCount: Const.LAUNCH_COUNT, warmupCount: Const.WARMUP_COUNT, iterationCount: Const.ITERATION_COUNT, invocationCount: Const.INVOCATION_COUNT)]
//[SimpleJob(RuntimeMoniker.Net60, launchCount: Const.LAUNCH_COUNT, warmupCount: Const.WARMUP_COUNT, iterationCount: Const.ITERATION_COUNT, invocationCount: Const.INVOCATION_COUNT)]
[SimpleJob(RuntimeMoniker.Net48, launchCount: Const.LAUNCH_COUNT, warmupCount: Const.WARMUP_COUNT, iterationCount: Const.ITERATION_COUNT, invocationCount: Const.INVOCATION_COUNT)]
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
        _vCardString40 = _vCard.ToVcfString(VCdVersion.V4_0, options: Opts.Default.Set(Opts.SetPropertyIDs));
        _vCardStringPhoto21 = _vCardPhoto.ToVcfString(VCdVersion.V2_1);
        _vCardStringPhoto30 = _vCardPhoto.ToVcfString();
        _vCardStringPhoto40 = _vCardPhoto.ToVcfString(VCdVersion.V4_0, options: Opts.Default.Set(Opts.SetPropertyIDs));
    }

    [Benchmark]
    public IList<VCard> Parse21() => Vcf.Parse(_vCardString21);

    [Benchmark]
    public IList<VCard> Parse21Photo() => Vcf.Parse(_vCardStringPhoto21);

    [Benchmark]
    public IList<VCard> Parse30() => Vcf.Parse(_vCardString30);

    [Benchmark]
    public IList<VCard> Parse30Photo() => Vcf.Parse(_vCardStringPhoto30);

    [Benchmark]
    public IList<VCard> Parse40() => Vcf.Parse(_vCardString40);

    [Benchmark]
    public IList<VCard> Parse40Photo() => Vcf.Parse(_vCardStringPhoto40);
}
