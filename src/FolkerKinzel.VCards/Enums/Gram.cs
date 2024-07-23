using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Enums;

/// <summary>
/// Named constants to define which grammatical gender to use in salutations and other grammatical constructs. (4 - RFC 9554)
/// </summary>
public enum Gram
{
    // CAUTION: If the enum is expanded, GramConverter must be updated

    /// <summary> <c>animate</c> Animate (4 - RFC 9554)</summary>
    Animate,

    /// <summary> <c>common</c> Common (4 - RFC 9554)</summary>
    Common,

    /// <summary> <c>feminine</c> Feminine (4 - RFC 9554)</summary>
    Feminine,

    /// <summary> <c>inanimate</c> Inanimate (4 - RFC 9554)</summary>
    Inanimate,

    /// <summary> <c>masculine</c> Masculine (4 - RFC 9554)</summary>
    Masculine,

    /// <summary> <c>neuter</c> Neuter (4 - RFC 9554)</summary>
    Neuter
}
