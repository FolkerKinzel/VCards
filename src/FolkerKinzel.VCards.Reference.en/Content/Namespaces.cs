﻿// Publish this namespace - it contains the VCard class
// and the VCardBuilder class:
using FolkerKinzel.VCards;

// It's recommended to publish also this namespace -
// it contains useful extension methods:
using FolkerKinzel.VCards.Extensions;

// This namespace contains often used enums. Decide
// yourself whether to publish this namespace or to use
// a namespace alias.
using FolkerKinzel.VCards.Enums;

// Since VCardBuilder exists, the model classes normally
// don't need to be instantiated in own code:
// using FolkerKinzel.VCards.Models;