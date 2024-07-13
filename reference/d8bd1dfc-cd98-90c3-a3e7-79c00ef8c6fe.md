# Keys Property


`KEY`: Public encryption keys associated with the vCard object. `(2,3,4)`



## Definition
**Namespace:** <a href="67dce261-ab8f-dd0a-4c0c-bc2633c1719e.md">FolkerKinzel.VCards</a>  
**Assembly:** FolkerKinzel.VCards (in FolkerKinzel.VCards.dll) Version: 7.2.0-beta.1+92a9170dd6e89cb66392c26e0acddb6434e3fbc1

**C#**
``` C#
public IEnumerable<DataProperty?>? Keys { get; set; }
```
**VB**
``` VB
Public Property Keys As IEnumerable(Of DataProperty)
	Get
	Set
```
**C++**
``` C++
public:
property IEnumerable<DataProperty^>^ Keys {
	IEnumerable<DataProperty^>^ get ();
	void set (IEnumerable<DataProperty^>^ value);
}
```
**F#**
``` F#
member Keys : IEnumerable<DataProperty> with get, set
```



#### Property Value
<a href="https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1" target="_blank" rel="noopener noreferrer">IEnumerable</a>(<a href="aa898609-8843-98f4-56c5-cc0c7bf76b89.md">DataProperty</a>)  
It may point to an external URL, may be plain text, or may be embedded in the VCF file as a Base64 encoded block of text.

## See Also


#### Reference
<a href="23413828-9a4a-2851-b88b-84d0afcb0031.md">VCard Class</a>  
<a href="67dce261-ab8f-dd0a-4c0c-bc2633c1719e.md">FolkerKinzel.VCards Namespace</a>  
