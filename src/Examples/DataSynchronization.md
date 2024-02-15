# The vCard 4.0 data synchronization
With the vCard 4.0 standard a data synchronization mechanism using PID parameters and CLIENTPIDMAP
properties has been introduced. This is very helpful when merging or updating vCards.

In order to identify vCard properties globally between diffferent version states of the same vCard,
vCard properties get assigned a local ID that is connected with a global identifier of the application
that created the local ID. The global identifier is used to distinguish between different local IDs 
that might have been assigned to the same vCard property by different applications.

Each VCard object needs to be registered with the application to make this work. The library does this 
automatically in the background; however, this requires the application to register with the VCard 
class. In order to avoid unexpected results, the registration has to be done before the first VCard
object is instantiated.

## How the application registration has to be done?
Call the static `VCard.RegisterApp` method with an absolute URI as an argument. Although it is 
allowed to call this method with the `null` argument, this is
not recommended. (UUID-URNs are ideal for the task.) Call the method before any other
method of the library and only once in the lifetime of the application. The URI
should be the same everytime the application runs.

Example:
```csharp
VCard.RegisterApp(new Uri("urn:uuid:53e374d9-337e-4727-8803-a1e9c14e0556"));
```

## How to apply PIDs to the vCard properties and a CLIENTPIDMAP property to the VCard?
Write a vCard 4.0 with the option `VcfOptions.SetPropertyIDs`:
```csharp
Vcf.Save(vCard,
         v4FilePath,
         VCdVersion.V4_0,
         options: VcfOptions.Default.Set(VcfOptions.SetPropertyIDs));
```
(Alternatively call `VCard.Sync.SetPropertyIDs()` manually on the VCard instance.)

## How can I remove PIDs and CLIENTPIDMAPs from a vCard 4.0?
Call `VCard.Sync.Reset()` on the VCard instance. This is is especially useful after a synchronization
process has been finished. Normally PIDs and CLIENTPIDMAPs are renewed after that,
writing a vCard 4.0 with the option `VcfOptions.SetPropertyIDs`, but you can remove them
completely like the following example shows:

```csharp
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;

namespace Examples;

public static class NoPidExample
{
    public static void RemovePropertyIdentification()
    {
        const string vcf = """
            BEGIN:VCARD
            VERSION:4.0
            REV:20231121T200704Z
            UID:urn:uuid:5ad11c78-f4b1-4e70-b0ef-6f4c29cf97ea
            FN;PID=1.1:John Doe
            CLIENTPIDMAP:1;http://other.com/
            END:VCARD
            """;

        VCard vCard = Vcf.Parse(vcf)[0];

        // Removes all existing PIDs and CLIENTPIDMAPs
        vCard.Sync.Reset();

        Console.WriteLine(vCard.ToVcfString(VCdVersion.V4_0));
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