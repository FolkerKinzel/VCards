using System.Net;
using FolkerKinzel.VCards;

namespace Examples;

public static class WebExample
{
    private static readonly HttpClient _client = new();

    public static void Example()
    {
        // In order to initialize the library, the executing application MUST be registered
        // with the VCard class. 
        VCard.RegisterApp(new Uri("urn:uuid:53e374d9-337e-4727-8803-a1e9c14e0556"));

        using HttpResponseMessage response = _client.Send(new HttpRequestMessage
             (HttpMethod.Get, 
             "https://raw.githubusercontent.com/FolkerKinzel/VCards/master/src/Examples/AnsiFilterExamples/German.vcf"));

        IList<VCard> vc = Vcf.Deserialize(
            () => response.Content.ReadAsStream(),
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