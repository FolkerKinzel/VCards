# ContactUris Property


`CONTACT-URI`: URIs representing an email address or a location for a web form. `(4 - RFC 8605)`



## Definition
**Namespace:** <a href="67dce261-ab8f-dd0a-4c0c-bc2633c1719e.md">FolkerKinzel.VCards</a>  
**Assembly:** FolkerKinzel.VCards (in FolkerKinzel.VCards.dll) Version: 7.2.0-beta.1+92a9170dd6e89cb66392c26e0acddb6434e3fbc1

**C#**
``` C#
public IEnumerable<TextProperty?>? ContactUris { get; set; }
```
**VB**
``` VB
Public Property ContactUris As IEnumerable(Of TextProperty)
	Get
	Set
```
**C++**
``` C++
public:
property IEnumerable<TextProperty^>^ ContactUris {
	IEnumerable<TextProperty^>^ get ();
	void set (IEnumerable<TextProperty^>^ value);
}
```
**F#**
``` F#
member ContactUris : IEnumerable<TextProperty> with get, set
```



#### Property Value
<a href="https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1" target="_blank" rel="noopener noreferrer">IEnumerable</a>(<a href="27f474f1-d496-3582-a707-2518da27485f.md">TextProperty</a>)

## Remarks
If the property contains more than one <a href="27f474f1-d496-3582-a707-2518da27485f.md">TextProperty</a> instance, the <a href="50760592-ebd2-d6c5-16b0-f752af7dada1.md">Preference</a> property must be set.

## See Also


#### Reference
<a href="23413828-9a4a-2851-b88b-84d0afcb0031.md">VCard Class</a>  
<a href="67dce261-ab8f-dd0a-4c0c-bc2633c1719e.md">FolkerKinzel.VCards Namespace</a>  
