using System.Collections.ObjectModel;
using OneOf;

namespace FolkerKinzel.VCards.Models;

[GenerateOneOf]
public sealed partial class DataPropertyValue : OneOfBase<ReadOnlyCollection<byte>, Uri, string> { }
