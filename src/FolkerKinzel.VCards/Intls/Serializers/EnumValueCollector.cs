using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal static class EnumValueCollector
{
    /// <summary>Collects the names of the flags set in <paramref name="addressType" /> into <paramref name="list" />. 
    /// </summary>
    /// <param name="addressType">The <see cref="Adr"/> value to parse.</param>
    /// <param name="list">The list to collect in.</param>
    /// <remarks>
    /// <note type="caution">
    /// <paramref name="list" /> is not emptied by the method!
    /// </note>
    /// </remarks>
    internal static void Collect(Adr? addressType, List<string> list)
    {
        Debug.Assert(list is not null);

        if (!addressType.HasValue)
        {
            return;
        }

        Adr value = addressType.Value & AdrConverter.DEFINED_ADDRESS_TYPES_VALUES;

        for (int i = AdrConverter.ADDRESS_TYPES_MIN_BIT; i <= AdrConverter.ADDRESS_TYPES_MAX_BIT; i++)
        {
            var flag = (Adr)(1 << i);

            if (value.HasFlag(flag))
            {
                list.Add(flag.ToVcfString());
            }
        }
    }

    /// <summary>Collects the names of the flags set in <paramref name="imppType" /> 
    /// into <paramref name="list" />. 
    /// </summary>
    /// <param name="imppType">The <see cref="Impp"/> value to parse.</param>
    /// <param name="list">The list to collect in.</param>
    /// <remarks>
    /// <note type="caution">
    /// <paramref name="list" /> is not emptied by the method!
    /// </note>
    /// </remarks>
    internal static void Collect(Impp? imppType, List<string> list)
    {
        Debug.Assert(list is not null);

        if (!imppType.HasValue)
        {
            return;
        }

        Impp value = imppType.Value & ImppConverter.DEFINED_IMPP_TYPES_VALUES;

        for (int i = ImppConverter.IMPP_TYPES_MIN_BIT; i <= ImppConverter.IMPP_TYPES_MAX_BIT; i++)
        {
            var flag = (Impp)(1 << i);

            if (value.HasFlag(flag))
            {
                list.Add(flag.ToVcfString());
            }
        }
    }

    /// <summary>Collects the names of the flags set in <paramref name="phoneType" /> into <paramref name="list" />. 
    /// </summary>
    /// <param name="phoneType">The <see cref="Tel"/> value to parse.</param>
    /// <param name="list">The list to collect in.</param>
    /// <remarks>
    /// <note type="caution">
    /// <paramref name="list" /> is not emptied by the method!
    /// </note>
    /// </remarks>
    internal static void Collect(Tel? phoneType, List<string> list)
    {
        Debug.Assert(list is not null);

        if (!phoneType.HasValue)
        {
            return;
        }

        Tel value = phoneType.Value & TelConverter.DEFINED_PHONE_TYPES_VALUES;

        for (int i = TelConverter.PHONE_TYPES_MIN_BIT; i <= TelConverter.PHONE_TYPES_MAX_BIT; i++)
        {
            var flag = (Tel)(1 << i);

            if (value.HasFlag(flag))
            {
                list.Add(flag.ToVcfString());
            }
        }
    }

    /// <summary>Collects the names of the flags set in <paramref name="propertyClassType" /> into <paramref name="list" />. 
    /// </summary>
    /// <param name="propertyClassType">The <see cref="PCl"/> value to parse.</param>
    /// <param name="list">The list to collect in.</param>
    /// <remarks>
    /// <note type="caution">
    /// <paramref name="list" /> is not emptied by the method!
    /// </note>
    /// </remarks>
    internal static void Collect(PCl? propertyClassType, List<string> list)
    {
        Debug.Assert(list is not null);

        if (!propertyClassType.HasValue)
        {
            return;
        }

        PCl value = propertyClassType.Value;

        if (value.HasFlag(PCl.Home))
        {
            list.Add(ParameterSection.TypeValue.HOME);
        }

        if (value.HasFlag(PCl.Work))
        {
            list.Add(ParameterSection.TypeValue.WORK);
        }
    }

    /// <summary>Collects the names of the flags set in <paramref name="relationType" /> into <paramref name="list" />. 
    /// </summary>
    /// <param name="relationType">The <see cref="Rel"/> value to parse.</param>
    /// <param name="list">The list to collect in.</param>
    /// <remarks>
    /// <note type="caution">
    /// <paramref name="list" /> is not emptied by the method!
    /// </note>
    /// </remarks>
    internal static void Collect(Rel? relationType, List<string> list)
    {
        Debug.Assert(list is not null);

        if (!relationType.HasValue)
        {
            return;
        }

        Rel value = relationType.Value & RelConverter.DEFINED_RELATION_TYPES_VALUES;

        for (int i = RelConverter.RELATION_TYPES_MIN_BIT; i <= RelConverter.RELATION_TYPES_MAX_BIT; i++)
        {
            var flag = (Rel)(1 << i);

            if (value.HasFlag(flag))
            {
                list.Add(flag.ToVcfString());
            }
        }
    }
}
