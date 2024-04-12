using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Syncs;
using static FolkerKinzel.VCards.VCard;

namespace FolkerKinzel.VCards.Models.PropertyParts;

public sealed partial class ParameterSection
{
    internal ParameterSection() { }

    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="paraSec">The <see cref="ParameterSection"/> instance to clone.</param>
    private ParameterSection(ParameterSection paraSec)
    {
        foreach (var kvp in paraSec._propDic)
        {
            _propDic.Add(kvp.Key, kvp.Value switch
            {
                PropertyID pid => pid,
                IEnumerable<PropertyID?> pidEnumerable => pidEnumerable.ToArray(),
                IEnumerable<string?> stringEnumerable => stringEnumerable.ToArray(),
                IEnumerable<KeyValuePair<string, string>> kvpEnumerable => kvpEnumerable.ToArray(),
                _ => kvp.Value
            });
        }
    }



    internal ParameterSection(string propertyKey,
                              ReadOnlySpan<char> parameterSection,
                              VcfDeserializationInfo info)
    {

        IEnumerable<KeyValuePair<string, string>> propertyParameters = GetParameters(parameterSection, info.ParameterList);

        Asserts(propertyKey, propertyParameters);

        StringBuilder builder = info.Builder;

        foreach (KeyValuePair<string, string> parameter in propertyParameters)
        {
            switch (parameter.Key)
            {
                case ParameterKey.LANGUAGE:
                    this.Language = parameter.Value.Trim();
                    break;
                case ParameterKey.VALUE:
                    {
                        string valValue = parameter.Value.Trim(info.TrimCharArray).ToUpperInvariant();
                        Data? dataType = DataConverter.Parse(valValue);
                        this.DataType = dataType;

                        if (!dataType.HasValue)
                        {
                            Loc contentLocation = LocConverter.Parse(valValue);
                            this.ContentLocation = contentLocation;

                            if (contentLocation == Loc.Url)
                            {
                                this.DataType = Data.Uri;
                            }
                        }

                        break;
                    }
                case ParameterKey.PREF:
                    {
                        if (TryParseInt(parameter.Value, info, out int intVal))
                        {
                            this.Preference = intVal;
                        }
                        break;
                    }
                case ParameterKey.PID:
                    {
                        this.PropertyIDs = PropertyID.Parse(parameter.Value);
                        break;
                    }
                case ParameterKey.TYPE:
                    {
                        ValueSplitter commaSplitter = info.CommaSplitter;
                        commaSplitter.ValueString = parameter.Value;

                        foreach (var s in commaSplitter)
                        {
                            if (!ParseTypeParameter(s, propertyKey, info))
                            {
                                AddNonStandardParameter(new KeyValuePair<string, string>(parameter.Key, s));
                            }
                        }
                        break;
                    }
                case ParameterKey.GEO:
                    if (GeoCoordinate.TryParse(parameter.Value.AsSpan().Trim(TRIM_CHARS),
                                               out GeoCoordinate? geo))
                    {
                        this.GeoPosition = geo;
                    }
                    break;
                case ParameterKey.TZ:
                    if (TimeZoneID.TryParse(parameter.Value.Trim(info.TrimCharArray), out TimeZoneID? tzID))
                    {
                        this.TimeZone = tzID;
                    }
                    break;
                case ParameterKey.SORT_AS:
                    {
                        List<string> list = (List<string>?)this.SortAs ?? [];
                        this.SortAs = list;

                        ValueSplitter commaSplitter = info.CommaSplitter;

                        commaSplitter.ValueString = parameter.Value;
                        foreach (var s in commaSplitter)
                        {
                            string sortString = s.UnMask(builder, VCdVersion.V4_0);
                            list.Add(sortString);
                        }

                        break;
                    }
                case ParameterKey.CALSCALE:
                    this.Calendar = parameter.Value.Trim(info.TrimCharArray);
                    break;
                case ParameterKey.ENCODING:
                    this.Encoding = EncConverter.Parse(parameter.Value);
                    break;
                case ParameterKey.CHARSET:
                    this.CharSet = parameter.Value.Trim(info.TrimCharArray);
                    break;
                case ParameterKey.ALTID:
                    this.AltID = parameter.Value.Trim(info.TrimCharArray);
                    break;
                case ParameterKey.MEDIATYPE:
                    this.MediaType = parameter.Value.Trim(info.TrimCharArray);
                    break;
                case ParameterKey.LABEL:
                    this.Label = builder
                        .Clear()
                        .Append(parameter.Value)
                        .Trim(info.TrimCharArray)
                        .Replace(@"\n", Environment.NewLine)
                        .Replace(@"\N", Environment.NewLine)
                        .ToString();
                    break;
                case ParameterKey.CONTEXT:
                    this.Context = parameter.Value.Trim(info.TrimCharArray);
                    break;
                case ParameterKey.INDEX:
                    {
                        if (TryParseInt(parameter.Value, info, out int result))
                        {
                            this.Index = result;
                        }
                        break;
                    }
                case ParameterKey.LEVEL:
                    if (propertyKey == VCard.PropKeys.NonStandard.EXPERTISE)
                    {
                        Expertise? expertise =
                            ExpertiseConverter.Parse(parameter.Value.AsSpan().TrimStart(TRIM_CHARS));

                        if (expertise.HasValue)
                        {
                            this.Expertise = expertise;
                        }
                        else
                        {
                            AddNonStandardParameter(parameter);
                        }
                    }
                    else // HOBBY oder INTEREST
                    {
                        Interest? interest = 
                            InterestConverter.Parse(parameter.Value.AsSpan().TrimStart(TRIM_CHARS));

                        if (interest.HasValue)
                        {
                            this.Interest = interest;
                        }
                        else
                        {
                            AddNonStandardParameter(parameter);
                        }
                    }//else
                    break;

                default:
                    {
                        AddNonStandardParameter(parameter);
                        break;
                    }

            }//switch

        }//foreach

    }//ctor

    private static List<KeyValuePair<string, string>> GetParameters(ReadOnlySpan<char> parameterSection,
                                                                List<KeyValuePair<string, string>> parameterTuples)
    {
        int splitIndex;
        ReadOnlySpan<char> parameter;
        int parameterStartIndex = 0;

        parameterTuples.Clear();

        // key=value;key="value,value,va;lue";key="val;ue" wird zu
        // key=value | key="value,value,va;lue" | key="val;ue"
        while (-1 != (splitIndex = GetNextParameterSplitIndex(parameterStartIndex, parameterSection)))
        {
            int paramLength = splitIndex - parameterStartIndex;

            if (paramLength != 0)
            {
                parameter = parameterSection.Slice(parameterStartIndex, paramLength);
                parameterStartIndex = splitIndex + 1;

                if (parameter.IsWhiteSpace())
                {
                    continue;
                }

                SplitParameterKeyAndValue(parameterTuples, parameter);
            }
            else
            {
                parameterStartIndex = splitIndex + 1;
            }
        }

        int length = parameterSection.Length - parameterStartIndex;

        if (length > 0)
        {
            parameter = parameterSection.Slice(parameterStartIndex, parameterSection.Length - parameterStartIndex);

            if (!parameter.IsWhiteSpace())
            {
                SplitParameterKeyAndValue(parameterTuples, parameter);
            }
        }

        return parameterTuples;

        ////////////////////////////////////////////////////////////////////

        // key=value;key="value,value,va;lue";key="val;ue" wird zu
        // key=value | key="value,value,va;lue" | key="val;ue"
        static int GetNextParameterSplitIndex(int parameterStartIndex, ReadOnlySpan<char> parameterSection)
        {
            bool isInDoubleQuotes = false;

            for (int i = parameterStartIndex; i < parameterSection.Length; i++)
            {
                char c = parameterSection[i];

                if (c == '"')
                {
                    isInDoubleQuotes = !isInDoubleQuotes;
                }
                else if (c == ';' && !isInDoubleQuotes)
                {
                    return i;
                }
            }//for

            return -1;
        }

        static void SplitParameterKeyAndValue(List<KeyValuePair<string, string>> parameterTuples, ReadOnlySpan<char> parameter)
        {
            int splitIndex = parameter.IndexOf('=');

            if (splitIndex == -1)
            {
                // in vCard 2.1. kann direkt das Value angegeben werden, z.B. Note;Quoted-Printable;UTF-8:Text des Kommentars
                string parameterString = parameter.ToString();
                parameterTuples.Add(
                    new KeyValuePair<string, string>(ParseAttributeKeyFromValue(parameterString), parameterString));
            }
            else
            {
                int valueStart = splitIndex + 1;
                int valueLength = parameter.Length - valueStart;

                if (valueLength != 0)
                {
                    parameterTuples.Add(
                            new KeyValuePair<string, string>(
                               ParameterKeyConverter.ParseParameterKey(parameter.Slice(0, splitIndex)),
                               parameter.Slice(valueStart, valueLength).ToString()));
                }
            }
        }
    }

    private static bool TryParseInt(string value, VcfDeserializationInfo info, out int result) =>
#if NET461 || NETSTANDARD2_0
        int.TryParse(value.Trim(info.TrimCharArray), out result);
#else
        int.TryParse(value.AsSpan().Trim(TRIM_CHARS), out result);
#endif

    [ExcludeFromCodeCoverage]
    [Conditional("DEBUG")]
    private static void Asserts(string propertyKey, IEnumerable<KeyValuePair<string, string>> propertyParameters)
    {
        Debug.Assert(propertyKey is not null);
        Debug.Assert(propertyParameters is not null);
        Debug.Assert(!propertyParameters.Any(
            x => string.IsNullOrWhiteSpace(x.Key) || string.IsNullOrWhiteSpace(x.Value)
            ));
        Debug.Assert(StringComparer.Ordinal.Equals(propertyKey, propertyKey.ToUpperInvariant()));
    }

    private void AddNonStandardParameter(KeyValuePair<string, string> parameter)
    {
        List<KeyValuePair<string, string>> userAttributes
                                        = (List<KeyValuePair<string, string>>?)this.NonStandard
                                          ?? [];

        this.NonStandard = userAttributes;
        userAttributes.Add(parameter);
    }

    private bool ParseTypeParameter(string typeValue, string propertyKey, VcfDeserializationInfo info)
    {
        typeValue = typeValue.Trim(info.TrimCharArray).ToUpperInvariant();
        Debug.Assert(typeValue.Length != 0);

        switch (typeValue)
        {
            case TypeValue.PREF:
                this.Preference = 1;
                return true;
            case TypeValue.HOME:
                this.PropertyClass = this.PropertyClass.Set(VCards.Enums.PCl.Home);
                return true;
            case TypeValue.WORK:
                this.PropertyClass = this.PropertyClass.Set(VCards.Enums.PCl.Work);
                return true;
            default:
                break;
        }

        switch (propertyKey)
        {
            case VCard.PropKeys.LABEL:
            case VCard.PropKeys.ADR:
                {
                    Adr? addressType = AdrConverter.Parse(typeValue);

                    if (addressType.HasValue)
                    {
                        this.AddressType = this.AddressType.Set(addressType.Value);
                        return true;
                    }

                    return false;
                }
            case VCard.PropKeys.TEL:
            case PropKeys.NonStandard.InstantMessenger.X_AIM:
            case PropKeys.NonStandard.InstantMessenger.X_GADUGADU:
            case PropKeys.NonStandard.InstantMessenger.X_GOOGLE_TALK:
            case PropKeys.NonStandard.InstantMessenger.X_GROUPWISE:
            case PropKeys.NonStandard.InstantMessenger.X_GTALK:
            case PropKeys.NonStandard.InstantMessenger.X_ICQ:
            case PropKeys.NonStandard.InstantMessenger.X_JABBER:
            case PropKeys.NonStandard.InstantMessenger.X_KADDRESSBOOK_X_IMADDRESS:
            case PropKeys.NonStandard.InstantMessenger.X_MSN:
            case PropKeys.NonStandard.InstantMessenger.X_MS_IMADDRESS:
            case PropKeys.NonStandard.InstantMessenger.X_SKYPE:
            case PropKeys.NonStandard.InstantMessenger.X_SKYPE_USERNAME:
            case PropKeys.NonStandard.InstantMessenger.X_TWITTER:
            case PropKeys.NonStandard.InstantMessenger.X_YAHOO:
                {
                    Tel? phoneType = TelConverter.Parse(typeValue);

                    if (phoneType.HasValue)
                    {
                        this.PhoneType = this.PhoneType.Set(phoneType.Value);
                        return true;
                    }

                    return false;
                }
            case VCard.PropKeys.RELATED:
                {
                    Rel? relType = RelConverter.Parse(typeValue);

                    if (relType.HasValue)
                    {
                        this.RelationType = this.RelationType.Set(relType.Value);
                        return true;
                    }

                    return false;
                }
            case VCard.PropKeys.EMAIL:
                this.EMailType = typeValue;
                break;
            case VCard.PropKeys.KEY:
                this.MediaType = MimeTypeConverter.MimeTypeFromKeyType(typeValue);
                break;
            case VCard.PropKeys.SOUND:
                this.MediaType = MimeTypeConverter.MimeTypeFromSoundType(typeValue);
                break;
            case VCard.PropKeys.PHOTO:
            case VCard.PropKeys.LOGO:
                this.MediaType = MimeTypeConverter.MimeTypeFromImageType(typeValue);
                break;
            case VCard.PropKeys.IMPP:
                {
                    Impp? imppType = ImppConverter.Parse(typeValue);

                    if (imppType.HasValue)
                    {
                        this.InstantMessengerType = this.InstantMessengerType.Set(imppType.Value);
                        return true;
                    }
                    return false;
                }
            default:
                return false;
        }//switch

        return true;
    }
}
