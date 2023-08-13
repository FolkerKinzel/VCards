using System.Collections.ObjectModel;
using OneOf;

namespace FolkerKinzel.VCards.Models;

[GenerateOneOf]
public partial class DataPropertyValue : OneOfBase<ReadOnlyCollection<byte>, Uri, string> { }
