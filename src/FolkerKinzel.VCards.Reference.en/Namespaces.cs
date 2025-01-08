// Publish this namespace - it contains commonly used classes such as
// the VCard class, the VCardBuilder class, the NameBuilder class, and the
// AddressBuilder class:
using FolkerKinzel.VCards;

// It's recommended to publish this namespace too -
// it contains useful extension methods:
using FolkerKinzel.VCards.Extensions;

// This namespace contains often used enums. Decide
// yourself whether to publish this namespace or to use
// a namespace alias.
using FolkerKinzel.VCards.Enums;

// This namespace contains the model classes such as GeoCoordinate or
// TimeZoneID:
using FolkerKinzel.VCards.Models;

// Contains the implementations of VCardProperty. If you use VCardBuilder to
// create and manipulate VCard objects, you usually do not need to publish this
// namespace.
//using FolkerKinzel.VCards.Models.Properties;

