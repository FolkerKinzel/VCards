using OneOf;

namespace FolkerKinzel.VCards.Models;

[GenerateOneOf]
public partial class DataPropertyValue : OneOfBase<string, byte[], Uri> { }
