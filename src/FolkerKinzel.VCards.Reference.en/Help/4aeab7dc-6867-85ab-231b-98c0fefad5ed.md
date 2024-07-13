# IsSet Method


Checks whether all flags set in *flags* are also set in *value*.



## Definition
**Namespace:** <a href="ea6bb853-85f2-e58b-0429-68b3fa762c9a.md">FolkerKinzel.VCards.Extensions</a>  
**Assembly:** FolkerKinzel.VCards (in FolkerKinzel.VCards.dll) Version: 7.2.0-beta.1+92a9170dd6e89cb66392c26e0acddb6434e3fbc1

**C#**
``` C#
public static bool IsSet(
	this Tel? value,
	Tel flags
)
```
**VB**
``` VB
<ExtensionAttribute>
Public Shared Function IsSet ( 
	value As Tel?,
	flags As Tel
) As Boolean
```
**C++**
``` C++
public:
[ExtensionAttribute]
static bool IsSet(
	Nullable<Tel> value, 
	Tel flags
)
```
**F#**
``` F#
[<ExtensionAttribute>]
static member IsSet : 
        value : Nullable<Tel> * 
        flags : Tel -> bool 
```



#### Parameters
<dl><dt>  <a href="https://learn.microsoft.com/dotnet/api/system.nullable-1" target="_blank" rel="noopener noreferrer">Nullable</a>(<a href="812fce9d-734d-1493-834c-58f45408588f.md">Tel</a>)</dt><dd>The value, which is checked to see whether all flags set in <em>flags</em> are set on it.</dd><dt>  <a href="812fce9d-734d-1493-834c-58f45408588f.md">Tel</a></dt><dd>A single <a href="812fce9d-734d-1493-834c-58f45408588f.md">Tel</a> value or a combination of several <a href="812fce9d-734d-1493-834c-58f45408588f.md">Tel</a> values.</dd></dl>

#### Return Value
<a href="https://learn.microsoft.com/dotnet/api/system.boolean" target="_blank" rel="noopener noreferrer">Boolean</a>  
Returns `true`, if all flags set in *flags* are also set in *value*. If *value* is `null`, `false` is returned.

#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type <a href="https://learn.microsoft.com/dotnet/api/system.nullable-1" target="_blank" rel="noopener noreferrer">Nullable</a>(<a href="812fce9d-734d-1493-834c-58f45408588f.md">Tel</a>). When you use instance method syntax to call this method, omit the first parameter. For more information, see <a href="https://docs.microsoft.com/dotnet/visual-basic/programming-guide/language-features/procedures/extension-methods" target="_blank" rel="noopener noreferrer">

Extension Methods (Visual Basic)</a> or <a href="https://docs.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/extension-methods" target="_blank" rel="noopener noreferrer">

Extension Methods (C# Programming Guide)</a>.

## See Also


#### Reference
<a href="d8878787-0d18-0761-b4d0-be70d4a9e267.md">TelExtension Class</a>  
<a href="ea6bb853-85f2-e58b-0429-68b3fa762c9a.md">FolkerKinzel.VCards.Extensions Namespace</a>  
