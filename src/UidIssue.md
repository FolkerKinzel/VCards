Hi @dmitryi0404,

a Universally Unique Identifier (UUID) is a 128-bit number used to uniquely 
identify information in computer systems. The term "UUID" is often used 
interchangeably with "GUID" (Globally Unique Identifier), which is a Microsoft implementation of the UUID standard.

The following string representations of the same GUID all identify the **exact same** thing:

```
d290f1ee-6c54-4b01-90e6-d701748f0851
urn:uuid:d290f1ee-6c54-4b01-90e6-d701748f0851
d290f1ee6c544b0190e6d701748f0851
{d290f1ee-6c54-4b01-90e6-d701748f0851}
(d290f1ee-6c54-4b01-90e6-d701748f0851)
{0xd290f1ee,0x6c54,0x4b01,{0x90,0xe6,0xd7,0x01,0x74,0x8f,0x08,0x51}}
D290F1EE6C544B0190E6D701748F0851
{D290F1EE-6C54-4B01-90E6-D701748F0851}
(D290F1EE-6C54-4B01-90E6-D701748F0851)
{0XD290F1EE,0X6C54,0X4B01,{0X90,0XE6,0XD7,0X01,0X74,0X8F,0X08,0X51}}
```

Different email clients could variously use all these different 
character strings to identify the same vCard. So, saving the original string wouldn't actually help you at all.

The problem you are struggling with is that your application does 
not control how diverse items - such as GUIDs or URLs - are converted into strings for
the database. You need to write your own normalization method. Here's a simple 
example of how this could be done:

```csharp
private static string NormalizeToString(ContactID id)
    {
        return id.Convert(
            guid => guid.ToString(),
            uri => uri.AbsoluteUri.ToLowerInvariant().TrimEnd('/'),
            str => str.Trim()
        );
    }
```

Then use this method to normalize ContactIDs before saving them to the database. 
This way, you ensure that all representations of the same GUID are stored in a consistent format, 
making it easier to retrieve them later.

Since the problem does not lie with the library, I would close this issue.