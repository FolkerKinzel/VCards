# GeoProperty(Double, Double, String) Constructor


Initializes a new <a href="cebf2b25-a331-1126-b40d-697dc18dcb72.md">GeoProperty</a> object.



## Definition
**Namespace:** <a href="10623553-9342-5b8f-9df4-6e7d1075f3df.md">FolkerKinzel.VCards.Models</a>  
**Assembly:** FolkerKinzel.VCards (in FolkerKinzel.VCards.dll) Version: 7.2.0-beta.1+92a9170dd6e89cb66392c26e0acddb6434e3fbc1

**C#**
``` C#
public GeoProperty(
	double latitude,
	double longitude,
	string? group = null
)
```
**VB**
``` VB
Public Sub New ( 
	latitude As Double,
	longitude As Double,
	Optional group As String = Nothing
)
```
**C++**
``` C++
public:
GeoProperty(
	double latitude, 
	double longitude, 
	String^ group = nullptr
)
```
**F#**
``` F#
new : 
        latitude : float * 
        longitude : float * 
        ?group : string 
(* Defaults:
        let _group = defaultArg group null
*)
-> GeoProperty
```



#### Parameters
<dl><dt>  <a href="https://learn.microsoft.com/dotnet/api/system.double" target="_blank" rel="noopener noreferrer">Double</a></dt><dd>Latitude (value between -90 and 90).</dd><dt>  <a href="https://learn.microsoft.com/dotnet/api/system.double" target="_blank" rel="noopener noreferrer">Double</a></dt><dd>Longitude (value between -180 and 180).</dd><dt>  <a href="https://learn.microsoft.com/dotnet/api/system.string" target="_blank" rel="noopener noreferrer">String</a>  (Optional)</dt><dd>Identifier of the group of <a href="e1395eb9-792c-c4d8-ee22-97939a91c58e.md">VCardProperty</a> objects, which the <a href="e1395eb9-792c-c4d8-ee22-97939a91c58e.md">VCardProperty</a> should belong to, or <code>null</code> to indicate that the <a href="e1395eb9-792c-c4d8-ee22-97939a91c58e.md">VCardProperty</a> does not belong to any group.</dd></dl>

## Exceptions
<table>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception" target="_blank" rel="noopener noreferrer">ArgumentOutOfRangeException</a></td>
<td><em>latitude</em> or <em>longitude</em> does not have a valid value.</td></tr>
</table>

## See Also


#### Reference
<a href="cebf2b25-a331-1126-b40d-697dc18dcb72.md">GeoProperty Class</a>  
<a href="f4656e22-6ac6-5eaa-200f-dfd7e0068f6c.md">GeoProperty Overload</a>  
<a href="10623553-9342-5b8f-9df4-6e7d1075f3df.md">FolkerKinzel.VCards.Models Namespace</a>  
