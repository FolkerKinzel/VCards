using BenchmarkDotNet.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace Benchmarks;

[MemoryDiagnoser]
[RyuJitX64Job]
public class AddressOrderBench
{
    private readonly Address _address = new AddressProperty(null, null, null, null, country: "Germany", autoLabel: false).Value;

    [Benchmark]
    public object? GetAddressOrder() => _address.GetAddressOrder();
}
