# FolkerKinzel.VCards 5.0.0-beta.1
## Package Release Notes
- Introduces two new helper classes: `AnsiFilter` and `MultiAnsiFilter`, which support you to load VCF files automatically with the right encoding.
- Fixes a bug, which sometimes occurred with the assignment of labels to addresses.
- Fixes an issue that the CHARSET parameter of vCard 2.1 LABEL properties was not preserved.
- Fixes an issue that the `VCard.LoadVcf`, `VCard.ParseVcf` and `VCard.DeserializeVcf` did catch too many exceptions.
- `ParameterSection.Charset` had been renamed to `ParameterSection.CharSet`.
