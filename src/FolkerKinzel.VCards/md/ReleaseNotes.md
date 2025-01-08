- Version 8 fixes an issue where in previous versions, UID properties containing types other 
than Guid were not preserved. In order to fix this issue, breaking changes at the public interface 
of the library were required.

It's recommended to start new projects using this version. When updating from version 7.x.x,
meaningful compiler error messages will help you to update existing code from obsolete symbols.

The current version is a preview version. Don't use it in production. (It's not yet properly 
tested and things can still change until the final release.)

&nbsp;
>**Project reference:** On some systems, the content of the CHM file in the Assets is blocked. Before opening the file right click on the file icon, select Properties, and **check the "Allow" checkbox** - if it is present - in the lower right corner of the General tab in the Properties dialog.