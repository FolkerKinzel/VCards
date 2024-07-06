# UID bug

## Describe the bug

UID of some VCARD 3.0 items are not properly recognized.

##To Reproduce
```csharp
using FolkerKinzel.VCards;

string vcard3 = @"
BEGIN:VCARD
VERSION:3.0
REV:2010-03-29T09:23:34Z
UID:dd72824a-cfda-457f-ae74-d70c2711e532@example.org
N:Dude;Some;Fred;;
FN:Some dude
NOTE:Simplified card for testing (Sogo Connector)
NICKNAME:fred
ROLE:Geek
END:VCARD
";


var vcard = Vcf.Parse(vcard3).FirstOrDefault();
if (vcard is not null)
{
    Console.WriteLine($"Version: {vcard.Version}");
    Console.WriteLine($"UID:     {vcard.ID?.Value}  (Should be 'dd72824a-cfda-457f-ae74-d70c2711e532@example.org')");
    Console.WriteLine($"Note:    {vcard.Notes}");
}
```

## Expected behavior

[see https://datatracker.ietf.org/doc/html/rfc6350#section-6.7.6](https://datatracker.ietf.org/doc/html/rfc6350#section-6.7.6), UID --> "Free-form text MAY also be used."

To avoid breaking the current UID UUID typing, maybe offer the original unmodified UID in a separate property

## Solution

### UID property allows:
- vCard 2.0: 
    - Value: a unique character string 
- vCard 3.0 (RFC 2426):
    - Value: single text value
    - Parameters: TYPE parameter
- vCard 4.0 (RFC 6350):
    - Value: single URI or free-form text (UUID-URN is recommended) 
    - Parameters: VALUE parameter

### Member property allows
- vCard 4.0 (RFC 6350):
    - Value: single URI

### Related property allows
- vCard 4.0 (RFC 6350):
    - Value: A single URI.  (It can also be reset to a single text value. The text value can be used to specify textual information.)