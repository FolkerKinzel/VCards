- Fixes the issue that `AddressProperty.IsEmpty` returns `true` although AddressProperty.Parameters.GeoPosition 
or AddressProperty.Parameters.TimeZone is not `null`. GeoCoordinates and TimeZones are relevant data and 
`AddressProperty.IsEmpty` should return `false` if such data is associated with it.
- Fixes an issue that X-NAME values for the `VALUE` parameter was not serialized and not parsed in vCard 4.0
&nbsp;
>**Project reference:** On some systems the content of the CHM file in the Assets is blocked. Before opening the file right click on the file icon, select Properties, and **check the "Allow" checkbox** - if it is present - in the lower right corner of the General tab in the Properties dialog.