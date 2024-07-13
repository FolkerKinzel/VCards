# DataProperty Methods




## Methods
<table>
<tr>
<td><a href="9970dc97-aa43-98fe-7476-56192b2f3f1a.md">Clone</a></td>
<td>Creates a new object that is a copy of the current instance.<br />(Inherited from <a href="e1395eb9-792c-c4d8-ee22-97939a91c58e.md">VCardProperty</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.equals#system-object-equals(system-object)" target="_blank" rel="noopener noreferrer">Equals</a></td>
<td>Determines whether the specified object is equal to the current object.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.finalize" target="_blank" rel="noopener noreferrer">Finalize</a></td>
<td>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="d5a1709b-3f55-b527-5b13-3b3b60efe3e2.md">FromBytes</a></td>
<td>Creates a new <a href="aa898609-8843-98f4-56c5-cc0c7bf76b89.md">DataProperty</a> instance that embeds an array of <a href="https://learn.microsoft.com/dotnet/api/system.byte" target="_blank" rel="noopener noreferrer">Byte</a>s in a VCF file.</td></tr>
<tr>
<td><a href="95c831b9-21b4-0c19-3ba3-6f8ac0ccada0.md">FromFile</a></td>
<td>Creates a new <a href="aa898609-8843-98f4-56c5-cc0c7bf76b89.md">DataProperty</a> instance that embeds the binary content of a file in a vCard.</td></tr>
<tr>
<td><a href="314b3d72-d4d9-4f97-afdb-1bd734585dc7.md">FromText</a></td>
<td>Creates a new <a href="aa898609-8843-98f4-56c5-cc0c7bf76b89.md">DataProperty</a> instance that embeds text in a vCard.</td></tr>
<tr>
<td><a href="9dce2992-9c0c-e546-0c4c-5f17d037ac0e.md">FromUri</a></td>
<td>Creates a new <a href="aa898609-8843-98f4-56c5-cc0c7bf76b89.md">DataProperty</a> instance from an absolute <a href="https://learn.microsoft.com/dotnet/api/system.uri" target="_blank" rel="noopener noreferrer">Uri</a> that references external data.</td></tr>
<tr>
<td><a href="8ed28acf-2b68-d093-ea29-af378806306e.md">GetFileTypeExtension</a></td>
<td>Gets an appropriate file type extension for <a href="8705913c-d6bf-8fbd-bbf5-aad607dd900a.md">Value</a>.</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.gethashcode" target="_blank" rel="noopener noreferrer">GetHashCode</a></td>
<td>Serves as the default hash function.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.gettype" target="_blank" rel="noopener noreferrer">GetType</a></td>
<td>Gets the <a href="https://learn.microsoft.com/dotnet/api/system.type" target="_blank" rel="noopener noreferrer">Type</a> of the current instance.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="ecf46e89-e960-eca1-8c2e-e4d9799598a9.md">GetVCardPropertyValue</a></td>
<td>Abstract access method to get the data from <a href="e1395eb9-792c-c4d8-ee22-97939a91c58e.md">VCardProperty</a>.<br />(Overrides <a href="8bc1d2ed-936e-e28b-5cf1-795671991d80.md">VCardProperty.GetVCardPropertyValue()</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.memberwiseclone" target="_blank" rel="noopener noreferrer">MemberwiseClone</a></td>
<td>Creates a shallow copy of the current <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="1fd93534-9644-5aa3-94e8-7e9a77c2c178.md">ToString</a></td>
<td>Returns a string that represents the current object.<br />(Overrides <a href="4c3798ad-932d-677a-6567-4b09fdec74e1.md">VCardProperty.ToString()</a>)</td></tr>
</table>

## Extension Methods
<table>
<tr>
<td><a href="5c83af36-d085-f664-32ca-2bb35abf3545.md">ConcatWith(DataProperty)</a></td>
<td>Concatenates two sequences of <a href="e1395eb9-792c-c4d8-ee22-97939a91c58e.md">VCardProperty</a> objects. (Most <a href="e1395eb9-792c-c4d8-ee22-97939a91c58e.md">VCardProperty</a> ojects implement <a href="https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1" target="_blank" rel="noopener noreferrer">IEnumerable&lt;VCardPoperty&gt;</a> and are themselves such a sequence.)<br />(Defined by <a href="c35d9134-4046-9ae5-662b-f2be39e4b469.md">IEnumerableExtension</a>)</td></tr>
<tr>
<td><a href="efb35da5-3e26-82a0-e3c5-1493d26a693e.md">ContainsGroup(DataProperty)</a></td>
<td>Indicates whether <em>values</em> contains an item that has the specified <a href="5d210979-76a6-b032-7b0c-02cffdbba833.md">Group</a> identifier.<br />(Defined by <a href="c35d9134-4046-9ae5-662b-f2be39e4b469.md">IEnumerableExtension</a>)</td></tr>
<tr>
<td><a href="11466c87-aadb-04fd-fe3d-7125281eb2af.md">FirstOrNull(DataProperty)</a></td>
<td>Gets the first <a href="e1395eb9-792c-c4d8-ee22-97939a91c58e.md">VCardProperty</a> from a collection of <a href="e1395eb9-792c-c4d8-ee22-97939a91c58e.md">VCardProperty</a> objects. The method takes the <a href="70c82664-4c95-c20f-f819-7fba4087eead.md">Index</a> property into account and allows to specify whether or not to ignore empty items.<br />(Defined by <a href="c35d9134-4046-9ae5-662b-f2be39e4b469.md">IEnumerableExtension</a>)</td></tr>
<tr>
<td><a href="55518c83-06ae-d0b3-2b9b-248467057638.md">FirstOrNull(DataProperty)</a></td>
<td>Gets the first <a href="e1395eb9-792c-c4d8-ee22-97939a91c58e.md">VCardProperty</a> from a collection of <a href="e1395eb9-792c-c4d8-ee22-97939a91c58e.md">VCardProperty</a> objects and allows filtering of the items, and to specify whether or not to ignore empty items.<br />(Defined by <a href="c35d9134-4046-9ae5-662b-f2be39e4b469.md">IEnumerableExtension</a>)</td></tr>
<tr>
<td><a href="8b1cf361-d9c0-21a1-9fdd-aa193ca0d6c4.md">FirstOrNullIsMemberOf(DataProperty)</a></td>
<td>Gets the first <a href="e1395eb9-792c-c4d8-ee22-97939a91c58e.md">VCardProperty</a> in a collection whose <a href="5d210979-76a6-b032-7b0c-02cffdbba833.md">Group</a> identifier matches the specified <a href="5d210979-76a6-b032-7b0c-02cffdbba833.md">Group</a> identifier.<br />(Defined by <a href="c35d9134-4046-9ae5-662b-f2be39e4b469.md">IEnumerableExtension</a>)</td></tr>
<tr>
<td><a href="a45403df-1f64-d22e-1df7-3479a2dd40f7.md">OrderByIndex(DataProperty)</a></td>
<td>Sorts the elements in <em>values</em> ascending by the value of their <a href="70c82664-4c95-c20f-f819-7fba4087eead.md">Index</a> property.<br />(Defined by <a href="c35d9134-4046-9ae5-662b-f2be39e4b469.md">IEnumerableExtension</a>)</td></tr>
<tr>
<td><a href="cd6e33b3-a481-08de-5643-cf1676d60d82.md">OrderByPref(DataProperty)</a></td>
<td>Sorts the elements in <em>values</em> ascending by the value of their <a href="50760592-ebd2-d6c5-16b0-f752af7dada1.md">Preference</a> property.<br />(Defined by <a href="c35d9134-4046-9ae5-662b-f2be39e4b469.md">IEnumerableExtension</a>)</td></tr>
<tr>
<td><a href="458f852f-6b59-c580-a2a0-7ad8ab8c805b.md">PrefOrNull(DataProperty)</a></td>
<td>Gets the most preferred <a href="e1395eb9-792c-c4d8-ee22-97939a91c58e.md">VCardProperty</a> from a collection of <a href="e1395eb9-792c-c4d8-ee22-97939a91c58e.md">VCardProperty</a> objects and allows to specify whether or not to ignore empty items.<br />(Defined by <a href="c35d9134-4046-9ae5-662b-f2be39e4b469.md">IEnumerableExtension</a>)</td></tr>
<tr>
<td><a href="956f934a-929d-76c6-2666-1fe7187245ca.md">PrefOrNull(DataProperty)</a></td>
<td>Gets the most preferred <a href="e1395eb9-792c-c4d8-ee22-97939a91c58e.md">VCardProperty</a> from a collection of <a href="e1395eb9-792c-c4d8-ee22-97939a91c58e.md">VCardProperty</a> objects and allows additional filtering of the items, and to specify whether or not to ignore empty items.<br />(Defined by <a href="c35d9134-4046-9ae5-662b-f2be39e4b469.md">IEnumerableExtension</a>)</td></tr>
<tr>
<td><a href="cc654469-216d-291e-83c4-99bccd18bd48.md">Remove(DataProperty)</a></td>
<td>Removes each occurrence of <em>value</em> from <em>values</em>.<br />(Defined by <a href="c35d9134-4046-9ae5-662b-f2be39e4b469.md">IEnumerableExtension</a>)</td></tr>
<tr>
<td><a href="1aaa5a6e-e6a3-6ae0-f17c-f9d05c588160.md">Remove(DataProperty)</a></td>
<td>Removes each item that matches the specified <em>predicate</em> from <em>values</em>.<br />(Defined by <a href="c35d9134-4046-9ae5-662b-f2be39e4b469.md">IEnumerableExtension</a>)</td></tr>
</table>

## Explicit Interface Implementations
<table>
<tr>
<td><a href="b7b25bd2-3065-a511-853f-a0992ed917b5.md">IEnumerable.GetEnumerator</a></td>
<td>Returns an enumerator that iterates through a collection.</td></tr>
<tr>
<td><a href="93c90ba5-3062-ff9d-01f4-3e55439ecf8b.md">IEnumerable(DataProperty).GetEnumerator</a></td>
<td>Returns an enumerator that iterates through the collection.</td></tr>
</table>

## See Also


#### Reference
<a href="aa898609-8843-98f4-56c5-cc0c7bf76b89.md">DataProperty Class</a>  
<a href="10623553-9342-5b8f-9df4-6e7d1075f3df.md">FolkerKinzel.VCards.Models Namespace</a>  
