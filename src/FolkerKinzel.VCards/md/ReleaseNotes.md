- **Performance:** The speed of read and write operations has been increased and memory consumption has been reduced.
- The minimum supported framework is .NET Framework 4.6.2 now.
- The method
```csharp
IAsyncEnumerable<VCard> DeserializeManyAsync(IEnumerable<Func<CancellationToken, Task<Stream>>?>,
                                             AnsiFilter?,
                                             [EnumeratorCancellation] CancellationToken)
```
is available now for all frameworks the package supports.
- Dependency updates
&nbsp;
>**Project reference:** On some systems the content of the CHM file in the Assets is blocked. Before opening the file right click on the file icon, select Properties, and **check the "Allow" checkbox** - if it is present - in the lower right corner of the General tab in the Properties dialog.