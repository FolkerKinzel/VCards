// Publish this namespace - it contains the VCard class:
using FolkerKinzel.VCards;

// It's recommended to publish also this namespace -
// it contains useful extension methods:
using FolkerKinzel.VCards.Extensions;

// This namespace contains often used enums. Decide
// yourself whether to publish this namespace or to use
// a namespace alias.
using FolkerKinzel.VCards.Models.Enums;

// Instead of publishing the following namespace better
// use a namespace alias, because the namespace contains
// a lot of classes:
using VC = FolkerKinzel.VCards.Models;

namespace NameSpaceAliasDemos;

public static class NameSpaceAliasDemo
{
    public static void HowToUseTheNameSpaceAlias() =>
        _ = VC::RelationProperty.FromText("Folker", Rel.Contact);
}

