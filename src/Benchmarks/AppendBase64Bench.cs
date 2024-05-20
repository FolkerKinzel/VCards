using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using FolkerKinzel.Strings;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Serializers;
using Base64Bcl = System.Buffers.Text.Base64;

namespace Benchmarks;

[MemoryDiagnoser]
[BaselineColumn]
[SimpleJob(RuntimeMoniker.Net80)]
public class AppendBase64Bench
{
    private const string VCARD_NEWLINE = "\r\n";
    private const int VCARD_MAX_BYTES_PER_LINE = 75;
    private readonly byte[] _bytes = new byte[78];

    public AppendBase64Bench() => new Random().NextBytes(_bytes);


    [Benchmark(Baseline = true)]
    public StringBuilder AppendBase64Current()
    {
        var builder = new StringBuilder();
        AppendBase64EncodedData(builder, _bytes);
        return builder;
    }

    [Benchmark()]
    public StringBuilder AppendBase64New()
    {
        var builder = new StringBuilder();
        AppendBase64EncodedData2(builder, _bytes);
        return builder;
    }

    private static void AppendBase64EncodedData2(StringBuilder Builder, byte[]? data)
    {
        // Append the NewLine in any case: The parser
        // needs it to detect the end of the Base64
        _ = Builder.Append(VCARD_NEWLINE);

        if (data is null || data.Length == 0)
        {
            return;
        }

        int base64CharsCount = Base64Bcl.GetMaxEncodedToUtf8Length(data.Length);

        using ArrayPoolHelper.SharedArray<byte> byteBuf = ArrayPoolHelper.Rent<byte>(base64CharsCount);
        Span<byte> byteSpan = byteBuf.Array.AsSpan();
        System.Buffers.OperationStatus status = Base64Bcl.EncodeToUtf8(data, byteSpan, out int bytesConsumed, out int bytesWritten);

        using ArrayPoolHelper.SharedArray<char> charBuf = ArrayPoolHelper.Rent<char>(base64CharsCount);
        int charsWritten = Encoding.UTF8.GetChars(byteBuf.Array, 0, base64CharsCount, charBuf.Array, 0);

        int builderCapacity = Builder.Length + base64CharsCount + (base64CharsCount / VCARD_MAX_BYTES_PER_LINE + 1) * 3;
        Builder.EnsureCapacity(builderCapacity);

        const int chunkLength = VCARD_MAX_BYTES_PER_LINE - 1;

        int end = base64CharsCount - chunkLength;
        int i = 0;

        for (; i < end; i += chunkLength)
        {
            _ = Builder.Append(' ')
                       .Append(charBuf.Array, i, chunkLength)
                       .Append(VCARD_NEWLINE);
        }

        _ = Builder.Append(' ')
                   .Append(charBuf.Array, i, base64CharsCount - i)
                   .Append(VCARD_NEWLINE);

        //string s = Builder.ToString() + "XXXXXXXXXXXXXXXXXXXXXXXXXXX";
    }


    private static void AppendBase64EncodedData(StringBuilder Builder, byte[]? data)
    {
        // Append the NewLine in any case: The parser
        // needs it to detect the end of the Base64
        _ = Builder.Append(VCARD_NEWLINE);

        if (data is null)
        {
            return;
        }

        int i = Builder.Length;
        _ = Builder.AppendBase64(data);

        while (i < Builder.Length)
        {
            _ = Builder.Insert(i, ' ');

            i = Math.Min(i + VCARD_MAX_BYTES_PER_LINE, Builder.Length);

            _ = Builder.Insert(i, VCARD_NEWLINE);

            i += VCARD_NEWLINE.Length;
        }
    }


}
