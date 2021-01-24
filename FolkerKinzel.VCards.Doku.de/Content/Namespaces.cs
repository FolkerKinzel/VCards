// Publish this namespace - it contains the VCard class:
using FolkerKinzel.VCards;

// It's recommended to publish also this namespace -
// it contains useful extension methods:
using FolkerKinzel.VCards.Extensions;

// These two namespaces may be published, but it's not
// recommended as they contain lots of classes and enums:
// using FolkerKinzel.VCards.Models;
// using FolkerKinzel.VCards.Models.Enums;

// Instead of publishing the two namespaces above
// better use a namespace alias:
using VC = FolkerKinzel.VCards.Models;

namespace NameSpaceAliasDemos
{
    public static class NameSpaceAliasDemo
    {
        public static void HowToUseTheNameSpaceAlias() =>
            _ = new VC::RelationTextProperty("Folker", VC::Enums.RelationTypes.Contact);
    }
}

