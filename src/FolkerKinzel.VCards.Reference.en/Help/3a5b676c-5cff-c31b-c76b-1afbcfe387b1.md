# NonStandards Property


vCard-Properties that don't belong to the standard.



## Definition
**Namespace:** <a href="67dce261-ab8f-dd0a-4c0c-bc2633c1719e.md">FolkerKinzel.VCards</a>  
**Assembly:** FolkerKinzel.VCards (in FolkerKinzel.VCards.dll) Version: 7.2.0-beta.1+92a9170dd6e89cb66392c26e0acddb6434e3fbc1

**C#**
``` C#
public NonStandardBuilder NonStandards { get; }
```
**VB**
``` VB
Public ReadOnly Property NonStandards As NonStandardBuilder
	Get
```
**C++**
``` C++
public:
property NonStandardBuilder NonStandards {
	NonStandardBuilder get ();
}
```
**F#**
``` F#
member NonStandards : NonStandardBuilder with get
```



#### Property Value
<a href="4975b130-bbf1-7c0e-31de-f1f8d80e095d.md">NonStandardBuilder</a>

## Remarks

NonStandards contains all vCard properties that could not be evaluated, when parsing the vCard. To serialize the content of NonStandards into a VCF file, the flag <a href="30bedfe8-6ddb-6b4e-f5cf-c3f361041435.md">WriteNonStandardProperties</a> has to be set.

Some <a href="96debf4b-ac3d-b14a-1b24-db26564c0795.md">NonStandardProperty</a> objects are automatically added to the VCF file, if there is no standard equivalent for it. You can control this behavior with <a href="30bedfe8-6ddb-6b4e-f5cf-c3f361041435.md">Opts</a>. It is therefore not recommended to assign <a href="96debf4b-ac3d-b14a-1b24-db26564c0795.md">NonStandardProperty</a> objects with these <a href="6a4b0773-5337-b445-059e-6e6748dac589.md">XName</a>s to this property.

These vCard properties are the following:
<ul><li><code>X-AIM</code></li><li><code>X-ANNIVERSARY</code></li><li><code>X-EVOLUTION-SPOUSE</code></li><li><code>X-EVOLUTION-ANNIVERSARY</code></li><li><code>X-GADUGADU</code></li><li><code>X-GENDER</code></li><li><code>X-GOOGLE-TALK</code></li><li><code>X-GROUPWISE</code></li><li><code>X-GTALK</code></li><li><code>X-ICQ</code></li><li><code>X-JABBER</code></li><li><code>X-KADDRESSBOOK-X-ANNIVERSARY</code></li><li><code>X-KADDRESSBOOK-X-IMADDRESS</code></li><li><code>X-KADDRESSBOOK-X-SPOUSENAME</code></li><li><code>X-MS-IMADDRESS</code></li><li><code>X-MSN</code></li><li><code>X-SKYPE</code></li><li><code>X-SKYPE-USERNAME</code></li><li><code>X-SPOUSE</code></li><li><code>X-TWITTER</code></li><li><code>X-WAB-GENDER</code></li><li><code>X-WAB-WEDDING_ANNIVERSARY</code></li><li><code>X-WAB-SPOUSE_NAME</code></li><li><code>X-YAHOO</code></li></ul>



## See Also


#### Reference
<a href="4254b25b-c39b-3224-d22e-0072642cabb3.md">VCardBuilder Class</a>  
<a href="67dce261-ab8f-dd0a-4c0c-bc2633c1719e.md">FolkerKinzel.VCards Namespace</a>  
