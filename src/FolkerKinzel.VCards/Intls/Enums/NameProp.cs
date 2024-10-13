using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Intls.Enums;

internal enum NameProp
{
    /// <summary>Family Name(s) (also known as surname(s)). (2,3,4)</summary>
    FamilyNames,

    /// <summary>Given Name(s) (first name(s)). (2,3,4)</summary>
    GivenNames,

    /// <summary>Additional Name(s) (middle name(s)). (2,3,4)</summary>
    AdditionalNames,

    /// <summary>Honorific Prefix(es). (2,3,4)</summary>
    Prefixes,

    /// <summary>Honorific Suffix(es). (2,3,4)</summary>
    Suffixes,

    /// <summary>Secondary surnames (used in some cultures), also known as "maternal surnames". (4 - RFC 9554)</summary>
    Surnames2,

    /// <summary>Generation markers or qualifiers, e.g., "Jr." or "III". (4 - RFC 9554)</summary>
    Generations
}
