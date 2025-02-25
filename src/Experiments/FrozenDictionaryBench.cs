using System.Collections.Frozen;
using BenchmarkDotNet.Attributes;

namespace Experiments;

[MemoryDiagnoser]
public class FrozenDictionaryBench
{
    private readonly KeyValuePair<int, int>[] _items;

    private readonly FrozenDictionary<int, int> _frozen;

    private readonly Dictionary<int, int> _dic;

    private const int LAST = 9;

    public FrozenDictionaryBench()
    {
        _items =
    [
        new KeyValuePair<int, int>(1,1),
        new KeyValuePair<int, int>(2,2),
        new KeyValuePair<int, int>(3,3),
        new KeyValuePair<int, int>(4,4),
        new KeyValuePair<int, int>(5,5),
        new KeyValuePair<int, int>(6,6),
        new KeyValuePair<int, int>(7,7),
        new KeyValuePair<int, int>(8,8),
        new KeyValuePair<int, int>(9,9),
    ];

        _frozen = CreateFrozenDictionary();
        _dic = CreateDictionary();
    }



    [Benchmark]
    public FrozenDictionary<int, int> CreateFrozenDictionary() => _items.ToFrozenDictionary();

    [Benchmark]
    public Dictionary<int, int> CreateDictionary() => _items.ToDictionary();

    [Benchmark]
    public int GetLastFrozen() => _frozen.TryGetValue(LAST, out int result) ? result : 0;

    [Benchmark]
    public int GetLastDic() => _dic.TryGetValue(LAST, out int result) ? result : 0;
}
