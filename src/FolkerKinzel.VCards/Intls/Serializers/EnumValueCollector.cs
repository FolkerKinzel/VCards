using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal static class EnumValueCollector
{
    /// <summary>Collects the names of the flags set in <paramref name="addressType" /> into <paramref name="list" />. 
    /// </summary>
    /// <param name="addressType">The <see cref="AddressTypes"/> value to parse.</param>
    /// <param name="list">The list to collect in.</param>
    /// <remarks>
    /// <note type="caution">
    /// <paramref name="list" /> is not emptied by the method!
    /// </note>
    /// </remarks>
    internal static void Collect(AddressTypes? addressType, List<string> list)
    {
        Debug.Assert(list != null);

        if (!addressType.HasValue)
        {
            return;
        }

        AddressTypes value = addressType.Value & AddressTypesConverter.DEFINED_ADDRESS_TYPES_VALUES;

        for (int i = AddressTypesConverter.ADDRESS_TYPES_MIN_BIT; i <= AddressTypesConverter.ADDRESS_TYPES_MAX_BIT; i++)
        {
            var flag = (AddressTypes)(1 << i);

            if (value.HasFlag(flag))
            {
                list.Add(flag.ToVcfString());
            }
        }
    }

    /// <summary>Collects the names of the flags set in <paramref name="imppType" /> 
    /// into <paramref name="list" />. 
    /// </summary>
    /// <param name="imppType">The <see cref="ImppTypes"/> value to parse.</param>
    /// <param name="list">The list to collect in.</param>
    /// <remarks>
    /// <note type="caution">
    /// <paramref name="list" /> is not emptied by the method!
    /// </note>
    /// </remarks>
    internal static void Collect(ImppTypes? imppType, List<string> list)
    {
        Debug.Assert(list != null);

        if (!imppType.HasValue)
        {
            return;
        }

        ImppTypes value = imppType.Value & ImppTypesConverter.DEFINED_IMPP_TYPES_VALUES;

        for (int i = ImppTypesConverter.IMPP_TYPES_MIN_BIT; i <= ImppTypesConverter.IMPP_TYPES_MAX_BIT; i++)
        {
            var flag = (ImppTypes)(1 << i);

            if (value.HasFlag(flag))
            {
                list.Add(flag.ToVcfString());
            }
        }
    }

    /// <summary>Collects the names of the flags set in <paramref name="telType" /> into <paramref name="list" />. 
    /// </summary>
    /// <param name="telType">The <see cref="PhoneTypes"/> value to parse.</param>
    /// <param name="list">The list to collect in.</param>
    /// <remarks>
    /// <note type="caution">
    /// <paramref name="list" /> is not emptied by the method!
    /// </note>
    /// </remarks>
    internal static void Collect(PhoneTypes? telType, List<string> list)
    {
        Debug.Assert(list != null);

        if (!telType.HasValue)
        {
            return;
        }

        PhoneTypes value = telType.Value & PhoneTypesConverter.DEFINED_TEL_TYPES_VALUES;

        for (int i = PhoneTypesConverter.TEL_TYPES_MIN_BIT; i <= PhoneTypesConverter.TEL_TYPES_MAX_BIT; i++)
        {
            var flag = (PhoneTypes)(1 << i);

            if (value.HasFlag(flag))
            {
                list.Add(flag.ToVcfString());
            }
        }
    }

    /// <summary>Collects the names of the flags set in <paramref name="propertyClassType" /> into <paramref name="list" />. 
    /// </summary>
    /// <param name="propertyClassType">The <see cref="PropertyClassTypes"/> value to parse.</param>
    /// <param name="list">The list to collect in.</param>
    /// <remarks>
    /// <note type="caution">
    /// <paramref name="list" /> is not emptied by the method!
    /// </note>
    /// </remarks>
    internal static void Collect(PropertyClassTypes? propertyClassType, List<string> list)
    {
        Debug.Assert(list != null);

        if (!propertyClassType.HasValue)
        {
            return;
        }

        PropertyClassTypes value = propertyClassType.Value;

        if (value.HasFlag(PropertyClassTypes.Home))
        {
            list.Add(ParameterSection.TypeValue.HOME);
        }

        if (value.HasFlag(PropertyClassTypes.Work))
        {
            list.Add(ParameterSection.TypeValue.WORK);
        }
    }

    /// <summary>Collects the names of the flags set in <paramref name="relationType" /> into <paramref name="list" />. 
    /// </summary>
    /// <param name="relationType">The <see cref="RelationTypes"/> value to parse.</param>
    /// <param name="list">The list to collect in.</param>
    /// <remarks>
    /// <note type="caution">
    /// <paramref name="list" /> is not emptied by the method!
    /// </note>
    /// </remarks>
    internal static void Collect(RelationTypes? relationType, List<string> list)
    {
        Debug.Assert(list != null);

        if (!relationType.HasValue)
        {
            return;
        }

        RelationTypes value = relationType.Value & RelationTypesConverter.DEFINED_RELATION_TYPES_VALUES;

        for (int i = RelationTypesConverter.RELATION_TYPES_MIN_BIT; i <= RelationTypesConverter.RELATION_TYPES_MAX_BIT; i++)
        {
            var flag = (RelationTypes)(1 << i);

            if (value.HasFlag(flag))
            {
                list.Add(flag.ToVcfString());
            }
        }
    }
}
