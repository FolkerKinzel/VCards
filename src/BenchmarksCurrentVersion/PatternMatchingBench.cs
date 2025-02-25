using BenchmarkDotNet.Attributes;

namespace Benchmarks;

public class PatternMatchingBench
{
    [Params(50, 500)]
    public int N { get; set; }

    [Benchmark]
    public bool OrFalse()
    {
        bool result = false;

        for (int i = 0; i < N; i++)
        {
            result = HasToBeQuoted('z');
        }

        return result;
    }

    [Benchmark]
    public bool OrTrue()
    {
        bool result = false;

        for (int i = 0; i < N; i++)
        {
            result = HasToBeQuoted('ä');
        }

        return result;
    }

    [Benchmark]
    public bool OrFalsePatternMatching()
    {
        bool result = false;

        for (int i = 0; i < N; i++)
        {
            result = HasToBeQuotedPatternMatching('z');
        }

        return result;
    }

    [Benchmark]
    public bool OrTruePatternMatching()
    {
        bool result = false;

        for (int i = 0; i < N; i++)
        {
            result = HasToBeQuotedPatternMatching('ä');
        }

        return result;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0078:Use pattern matching", Justification = "<Pending>")]
    private static bool HasToBeQuoted(int bt) => bt != '\t' && (bt > 126 || bt == '=' || bt < 32);

    private static bool HasToBeQuotedPatternMatching(int bt) => bt is not '\t' and (> 126 or '=' or < 32);


}
