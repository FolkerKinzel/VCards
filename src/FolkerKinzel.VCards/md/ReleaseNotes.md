- New constructor overload `GeoProperty(double, double, string?)`
- New constructor overload `PropertyIDMappingProperty(int, Uri)`
- New extension methods:
```
void SetPreferences<TSource>(this IEnumerable<TSource?>?, bool)
        where TSource : VCardProperty, IEnumerable<TSource>;

void UnsetPreferences<TSource>(this IEnumerable<TSource?>?)
        where TSource : VCardProperty, IEnumerable<TSource>;

void SetIndexes<TSource>(this IEnumerable<TSource?>?, bool) 
        where TSource : VCardProperty, IEnumerable<TSource>;

void UnsetIndexes<TSource>(this IEnumerable<TSource?>?)
        where TSource : VCardProperty, IEnumerable<TSource>;

void SetAltID<TSource>(this IEnumerable<TSource?>?, string?) 
        where TSource : VCardProperty, IEnumerable<TSource>;
```

.
>**Project reference:** On some systems the content of the CHM file in the Assets is blocked. Before opening the file right click on the file icon, select Properties, and **check the "Allow" checkbox** - if it is present - in the lower right corner of the General tab in the Properties dialog.