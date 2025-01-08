using FolkerKinzel.VCards;

namespace Examples;

public static class WebExample
{
    private const string URI =
        "https://raw.githubusercontent.com/FolkerKinzel/VCards/master/src/Examples/AnsiFilterExamples/German.vcf";

    private static readonly HttpClient _client = new();

    public static async Task AsyncExample()
    {
        using var cts = new CancellationTokenSource();

        IReadOnlyList<VCard> vc = await Vcf.DeserializeAsync(t => _client.GetStreamAsync(URI, t),
                                                     new AnsiFilter(),
                                                     cts.Token);
        Console.WriteLine(vc[0]);
    }

    public static void SynchronousExample()
    {
        using HttpResponseMessage response =
            _client.Send(new HttpRequestMessage(HttpMethod.Get, URI));

        IReadOnlyList<VCard> vc = Vcf.Deserialize(() => response.Content.ReadAsStream(),
                                          new AnsiFilter());
        Console.WriteLine(vc[0]);
    }
}

/*
Console Output:

Version: 2.1

DisplayNames: Sören Täve Nüßlebaum

NameViews:
    FamilyNames:     Nüßlebaum
    GivenNames:      Sören
    AdditionalNames: Täve

[Label: Mößlitz]
[CharSet: Windows-1252]
Addresses: Locality: Mößlitz
 */