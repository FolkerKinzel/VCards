- Implements from [RFC 9555 - JSContact: Converting from and to vCard](https://datatracker.ietf.org/doc/html/rfc9555): `JSPROP`, `JSCOMPS`, and `JSPTR`.
- `X-ABLabel` is now supported like a standard vCard property
- New extension method:
```csharp
IEnumerable<TSource> Items<TSource>(this IEnumerable<TSource?>?, bool discardEmptyItems = true)
        where TSource : VCardProperty
```

&nbsp;
>**Project reference:** On some systems, the content of the CHM file in the Assets is blocked. Before opening the file right click on the file icon, select Properties, and **check the "Allow" checkbox** - if it is present - in the lower right corner of the General tab in the Properties dialog.