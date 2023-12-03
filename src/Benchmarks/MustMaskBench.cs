using BenchmarkDotNet.Attributes;
using FolkerKinzel.Strings;
using FolkerKinzel.VCards.Enums;

namespace Benchmarks;

public class MustMaskBench
{
    private const string NONE = "none";
    private const string SEMI_COLON = "semicolon;";
    private const string COMMA = "comma,";
    private const string BACK_SLASH = "backslash\\";

    [Benchmark]
    public void V2None_1() => MustMask1(NONE, VCdVersion.V2_1);
    [Benchmark]
    public void V2None_2() => MustMask2(NONE, VCdVersion.V2_1);
    [Benchmark]
    public void V3None_1() => MustMask1(NONE, VCdVersion.V3_0);
    [Benchmark]
    public void V3None_2() => MustMask2(NONE, VCdVersion.V3_0);
    [Benchmark]
    public void V4None_1() => MustMask1(NONE, VCdVersion.V4_0);
    [Benchmark]
    public void V4None_2() => MustMask2(NONE, VCdVersion.V4_0);

    [Benchmark]
    public bool V2SemiColon_1() => MustMask1(SEMI_COLON, VCdVersion.V2_1);
    [Benchmark]
    public bool V2SemiColon_2() => MustMask2(SEMI_COLON, VCdVersion.V2_1);
    [Benchmark]
    public bool V3SemiColon_1() => MustMask1(SEMI_COLON, VCdVersion.V3_0);
    [Benchmark]
    public bool V3SemiColon_2() => MustMask2(SEMI_COLON, VCdVersion.V3_0);
    [Benchmark]
    public bool V4SemiColon_1() => MustMask1(SEMI_COLON, VCdVersion.V4_0);
    [Benchmark]
    public bool V4SemiColon_2() => MustMask2(SEMI_COLON, VCdVersion.V4_0);

    [Benchmark]
    public bool V3Comma_1() => MustMask1(COMMA, VCdVersion.V3_0);
    [Benchmark]
    public bool V3Comma_2() => MustMask2(COMMA, VCdVersion.V3_0);
    [Benchmark]
    public bool V4Comma_1() => MustMask1(COMMA, VCdVersion.V4_0);
    [Benchmark]
    public bool V4Comma_2() => MustMask2(COMMA, VCdVersion.V4_0);

    [Benchmark]
    public bool V4BackSlash_2() => MustMask2(BACK_SLASH, VCdVersion.V4_0);

    private static bool MustMask1(string? value, VCdVersion version)
    {
        return value is not null &&
              (
                value.Contains(';') ||
                (version >= VCdVersion.V3_0 && value.Contains(',')) ||
                (version >= VCdVersion.V4_0 && value.Contains('\\'))
               );
    }

    private static bool MustMask2(string? value, VCdVersion version)
    {
        return value is not null &&
              (
                (version == VCdVersion.V2_1 && value.Contains(';')) ||
                (version == VCdVersion.V3_0 && value.ContainsAny(";,")) ||
                (version >= VCdVersion.V4_0 && value.ContainsAny(";,\\"))
               );
    }
}
