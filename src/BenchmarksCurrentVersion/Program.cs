using System;
using BenchmarkDotNet.Running;
using FolkerKinzel.VCards;

namespace Benchmarks;


internal class Program
{
    private static void Main()
    {
        VCard.RegisterApp(new Uri("urn:uuid:53e374d9-337e-4727-8803-a1e9c14e0556"));

        //new AppendBase64Bench().AppendBase64New();
        //_ = new ParseBench().Parse40();
        //_ = new WriteBench().Write40();
        //_ = BenchmarkRunner.Run<ParseBench>();
        //_ = BenchmarkRunner.Run<WriteBench>();
        _ = BenchmarkRunner.Run<ListConcatBench>();

        //_ = BenchmarkRunner.Run<AppendBase64Bench>();
        //_ = BenchmarkRunner.Run<PatternMatchingBench>();

        //_ = BenchmarkRunner.Run<EnumerableBench>();
        //_ = BenchmarkRunner.Run<HasFlagBench>();

        //_ = BenchmarkRunner.Run<ConcatWithBench>();

        //_ = BenchmarkRunner.Run<MustMaskBench>();
        //_ = BenchmarkRunner.Run<AnsiFilterBench2>();

        //_ = BenchmarkRunner.Run<QuotedPrintableBench>();
        //_ = BenchmarkRunner.Run<VCardBench>();
        //_ = BenchmarkRunner.Run<KeyValuePairBench>();
        //_ = BenchmarkRunner.Run<SingleStringListBench>();

        //_ = BenchmarkRunner.Run<AddressOrderBench>();
    }
}
