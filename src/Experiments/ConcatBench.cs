using BenchmarkDotNet.Attributes;

namespace Experiments;

[MemoryDiagnoser]
public class ConcatBench
{
    //private readonly string[] _empty = [];
    private readonly string[] _empty = ["sdfghjkfgh", "qwertzui"];

    [Benchmark]
    public int ListAddRange()
    {
        List<string> list = [];

        list.AddRange(_empty);
        list.AddRange(_empty);
        list.AddRange(_empty);
        list.AddRange(_empty);
        list.AddRange(_empty);
        list.AddRange(_empty);
        list.AddRange(_empty);
        list.AddRange(_empty);
        list.AddRange(_empty);
        list.AddRange(_empty);
        list.AddRange(_empty);
        list.AddRange(_empty);
        list.AddRange(_empty);
        list.AddRange(_empty);
        list.AddRange(_empty);
        list.AddRange(_empty);
        list.AddRange(_empty);
        list.AddRange(_empty);
        list.AddRange(_empty);

        return list.Count;
    }

    [Benchmark]
    public int EnumerableConcat()
    {
        IEnumerable<string> numerable =
            _empty
            .Concat(_empty)
            .Concat(_empty)
            .Concat(_empty)
            .Concat(_empty)
            .Concat(_empty)
            .Concat(_empty)
            .Concat(_empty)
            .Concat(_empty)
            .Concat(_empty)
            .Concat(_empty)
            .Concat(_empty)
            .Concat(_empty)
            .Concat(_empty)
            .Concat(_empty)
            .Concat(_empty)
            .Concat(_empty)
            .Concat(_empty)
            .Concat(_empty)
            .Concat(_empty);

        return numerable.Count();
    }
}
