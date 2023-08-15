using OneOf;

namespace FolkerKinzel.VCards.Models;

[GenerateOneOf]
public sealed partial class DateAndOrTime : OneOfBase<DateOnly, DateTimeOffset, TimeOnly, string> { }
