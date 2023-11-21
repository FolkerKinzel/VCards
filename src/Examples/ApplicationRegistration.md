# Application registration
As the first step, the executing application has to be registered with the `VCard` class
when the program starts. This section describes this process by answering fictitious questions.
## What's the use of application registration?
With the vCard 4.0 standard a data synchronization mechanism using PID parameters and CLIENTPIDMAP
properties has been introduced. This is very helpful when merging or updating vCards.

vCard properties get assigned a local ID that is connected with a global identifier of the application
that created the local ID. The global identifier is used to distinguish between the different local IDs 
that might have been assigned to the same vCard property by different applications.

Each VCard object needs to be registered with the application to make this work. The library does this 
automatically in the background; however, this requires the application to register with the VCard 
class. In order to avoid unexpected results, the registration has to be done before the first VCard
object is instantiated.
## Do I have to register my application even if I only use the vCard standard 3.0?
Yes. The library cannot guess your intentions.
## How is the application registration to be done?
Call the static `VCard.RegisterApp` method with an absolute URI as an argument. Although it is 
allowed to call this method with the <c>null</c> argument, this is
not recommended. (UUID-URNs are ideal for the task.) Call the method before any other
method of the library and only once in the lifetime of your application. The URI
should be the same everytime the application runs.

Example:
```csharp
VCard.RegisterApp(new Uri("urn:uuid:53e374d9-337e-4727-8803-a1e9c14e0556"));
```
## If I don't like PIDs and CLIENTPIDMAPs, can I write vCard 4.0 without them?
Of course, but application registration is still needed.

Example:
```csharp
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;

namespace Examples;

public static class NoPidExample
{
    public static void SaveWithoutPropertyIdentification()
    {
        VCard.RegisterApp(new Uri("urn:uuid:53e374d9-337e-4727-8803-a1e9c14e0556"));

        const string vcf = """
            BEGIN:VCARD
            VERSION:4.0
            REV:20231121T200704Z
            UID:urn:uuid:5ad11c78-f4b1-4e70-b0ef-6f4c29cf97ea
            FN;PID=1.1:John Doe
            CLIENTPIDMAP:1;http://other.com/
            END:VCARD
            """;

        VCard vCard = VCard.ParseVcf(vcf)[0];

        // Removes all existing PIDs and CLIENTPIDMAPs
        vCard.Sync.Reset();

        VcfOptions options = VcfOptions.Default.Unset(VcfOptions.SetPropertyIDs);

        Console.WriteLine(vCard.ToVcfString(VCdVersion.V4_0, options: options));
    }
}

/*
Console Output:

BEGIN:VCARD
VERSION:4.0
REV:20231121T201552Z
UID:urn:uuid:5ad11c78-f4b1-4e70-b0ef-6f4c29cf97ea
FN:John Doe
END:VCARD
*/
```