using BenchmarkDotNet.Attributes;
using FolkerKinzel.VCards.Models;

namespace Benchmarks;

public class SingleStringListBench
{
    [Benchmark]
    public AddressProperty StructVsClass()
    {
#pragma warning disable CS0618 // Typ oder Element ist veraltet
        return new AddressProperty("eins", "zwei", "vier", "drei", "fünf", "sechs", "sieben");
#pragma warning restore CS0618 // Typ oder Element ist veraltet
    }

}
