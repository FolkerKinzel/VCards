using BenchmarkDotNet.Attributes;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;

namespace Benchmarks;

[MemoryDiagnoser]
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
    public string Write21() => Vcf.AsString(_vCard, VCdVersion.V2_1);

    [Benchmark]
    public string WritePhoto21() => Vcf.AsString(_vCardPhoto, VCdVersion.V2_1);

    [Benchmark]
    public string Write30() => Vcf.AsString(_vCard);

    [Benchmark]
    public string WritePhoto30() => Vcf.AsString(_vCardPhoto);

    [Benchmark]
    public string Write40() => Vcf.AsString(_vCard, VCdVersion.V4_0, options: VcfOpts.Default.Set(VcfOpts.SetPropertyIDs));

    [Benchmark]
    public string WritePhoto40() => Vcf.AsString(_vCardPhoto, VCdVersion.V4_0, options: VcfOpts.Default.Set(VcfOpts.SetPropertyIDs));
}
