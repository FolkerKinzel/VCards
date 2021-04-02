# FolkerKinzel.VCards
## Roadmap

### 2.1.1
- [ ] Cleanup: Remove obsolete symbols that are still marked as errors.

### 3.0.0  
- [ ] Move FolkerKinzel.VCards.Models.PropertyParts.GeoCoordinate to 
FolkerKinzel.VCards.Models.GeoCoordinate
- [ ] Mark the usage of VCdDataType.Timestamp as Obsolete-Error.
- [ ] Change the parameter List&lt;VCard&gt; vCardList to IEnumerable&lt;VCard?&gt; vCards.
- [ ] Add extension methods for IEnumerable&lt;VCard?&gt; to serialize this as VCF.

### 3.0.1
- [ ] Cleanup: Remove VCdDataType.Timestamp.

### 4.0.0
- [ ] Replace net4.0 support through net4.5 support.
- [ ] Remove the DataUrl class and the MimeType class and replace it with something new like
FolkerKinzel.URIs.DataUrlHelper.