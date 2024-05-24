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
        foreach (KeyValuePair<VCdParam, object> kvp in paraSec._propDic)
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


#if NET8_0_OR_GREATER
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters",
        Justification = "TRIM_CHARS are not localizable")]
#endif
    internal ParameterSection(string propertyKey,
                              in ReadOnlyMemory<char> parameterSection,
                              VcfDeserializationInfo info)
    {

        List<KeyValuePair<string, ReadOnlyMemory<char>>> propertyParameters = info.ParameterList;
        GetParameters(in parameterSection, propertyParameters);

        Asserts(propertyKey, propertyParameters);

        foreach (KeyValuePair<string, ReadOnlyMemory<char>> parameter in propertyParameters)
        {
            switch (parameter.Key)
            {
                case ParameterKey.LANGUAGE:
                    this.Language = parameter.Value.Span.Trim().ToString();
                    break;
                case ParameterKey.VALUE:
                    {
                        string valValue = parameter.Value.Span.Trim(TRIM_CHARS).ToString().ToUpperInvariant();
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
                        if (_Int.TryParse(parameter.Value.Span.Trim(TRIM_CHARS), out int intVal))
                        {
                            this.Preference = intVal;
                        }
                        break;
                    }
                case ParameterKey.PID:
                    {
                        this.PropertyIDs = PropertyID.Parse(parameter.Value.ToString());
                        break;
                    }
                case ParameterKey.TYPE:
                    {
                        foreach (string s in ValueSplitter2.Split(
                            parameter.Value, ',', StringSplitOptions.RemoveEmptyEntries, unMask: false, VCdVersion.V3_0))
                        {
                            if (!ParseTypeParameter(s, propertyKey))
                            {
                                List<KeyValuePair<string, string>> userAttributes
                                        = (List<KeyValuePair<string, string>>?)this.NonStandard
                                          ?? [];

                                this.NonStandard = userAttributes;
                                userAttributes.Add(new KeyValuePair<string, string>(parameter.Key, s));
                            }
                        }
                        break;
                    }
                case ParameterKey.GEO:
                    if (GeoCoordinate.TryParse(parameter.Value.Span.Trim(TRIM_CHARS),
                                               out GeoCoordinate? geo))
                    {
                        this.GeoPosition = geo;
                    }
                    break;
                case ParameterKey.TZ:
                    if (TimeZoneID.TryParse(parameter.Value.Span.Trim(TRIM_CHARS).ToString(), out TimeZoneID? tzID))
                    {
                        this.TimeZone = tzID;
                    }
                    break;
                case ParameterKey.SORT_AS:
                    {
                        this.SortAs = new List<string>
                            (
                            ValueSplitter2.Split(
                            parameter.Value, ',', StringSplitOptions.RemoveEmptyEntries, unMask: true, VCdVersion.V4_0)
                            );

                        break;
                    }
                case ParameterKey.CALSCALE:
                    this.Calendar = parameter.Value.Span.Trim(TRIM_CHARS).ToString();
                    break;
                case ParameterKey.ENCODING:
                    this.Encoding = EncConverter.Parse(parameter.Value.ToString());
                    break;
                case ParameterKey.CHARSET:
                    this.CharSet = parameter.Value.Span.Trim(TRIM_CHARS).ToString();
                    break;
                case ParameterKey.ALTID:
                    this.AltID = parameter.Value.Span.Trim(TRIM_CHARS).ToString();
                    break;
                case ParameterKey.MEDIATYPE:
                    this.MediaType = parameter.Value.Span.Trim(TRIM_CHARS).ToString();
                    break;
                case ParameterKey.LABEL:
                    this.Label = parameter.Value.Span.Trim(TRIM_CHARS).UnMask(VCdVersion.V4_0);
                    break;
                case ParameterKey.CONTEXT:
                    this.Context = parameter.Value.Span.Trim(TRIM_CHARS).ToString();
                    break;
                case ParameterKey.INDEX:
                    {
                        if (_Int.TryParse(parameter.Value.Span.Trim(TRIM_CHARS), out int result))
                        {
                            this.Index = result;
                        }
                        break;
                    }
                case ParameterKey.LEVEL:
                    if (propertyKey == VCard.PropKeys.NonStandard.EXPERTISE)
                    {
                        Expertise? expertise =
                            ExpertiseConverter.Parse(parameter.Value.Span.Trim(TRIM_CHARS));

                        if (expertise.HasValue)
                        {
                            this.Expertise = expertise;
                        }
                        else
                        {
                            AddNonStandardParameter(in parameter);
                        }
                    }
                    else // HOBBY oder INTEREST
                    {
                        Interest? interest =
                            InterestConverter.Parse(parameter.Value.Span.Trim(TRIM_CHARS));

                        if (interest.HasValue)
                        {
                            this.Interest = interest;
                        }
                        else
                        {
                            AddNonStandardParameter(in parameter);
                        }
                    }//else
                    break;

                default:
                    {
                        AddNonStandardParameter(in parameter);
                        break;
                    }

            }//switch

        }//foreach

    }//ctor

    private static void GetParameters(in ReadOnlyMemory<char> parameterSection,
                                      List<KeyValuePair<string, ReadOnlyMemory<char>>> parameterTuples)
    {
        int splitIndex;
        ReadOnlyMemory<char> parameter;
        int parameterStartIndex = 0;

        parameterTuples.Clear();
        ReadOnlySpan<char> parameterSectionSpan = parameterSection.Span;

        // key=value;key="value,value,va;lue";key="val;ue" wird zu
        // key=value | key="value,value,va;lue" | key="val;ue"
        while (-1 != (splitIndex = GetNextParameterSplitIndex(parameterStartIndex, parameterSectionSpan)))
        {
            int paramLength = splitIndex - parameterStartIndex;

            if (paramLength != 0)
            {
                parameter = parameterSection.Slice(parameterStartIndex, paramLength);
                parameterStartIndex = splitIndex + 1;

                if (parameter.Span.IsWhiteSpace())
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

            if (!parameter.Span.IsWhiteSpace())
            {
                SplitParameterKeyAndValue(parameterTuples, in parameter);
            }
        }

        //return parameterTuples;

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

        static void SplitParameterKeyAndValue(List<KeyValuePair<string, ReadOnlyMemory<char>>> parameterTuples, in ReadOnlyMemory<char> parameter)
        {
            ReadOnlySpan<char> parameterSpan = parameter.Span;

            int splitIndex = parameterSpan.IndexOf('=');

            if (splitIndex == -1)
            {
                // in vCard 2.1. the Value can be specified directly, e.g., Note;Quoted-Printable;UTF-8:some text
                parameterTuples.Add(
                    new KeyValuePair<string, ReadOnlyMemory<char>>(ParseAttributeKeyFromValue(parameterSpan), parameter));
            }
            else
            {
                int valueStart = splitIndex + 1;
                int valueLength = parameter.Length - valueStart;

                if (valueLength != 0)
                {
                    parameterTuples.Add(
                            new KeyValuePair<string, ReadOnlyMemory<char>>(
                               ParameterKeyConverter.ParseParameterKey(parameterSpan.Slice(0, splitIndex)),
                               parameter.Slice(valueStart, valueLength)));
                }
            }
        }
    }

    [ExcludeFromCodeCoverage]
    [Conditional("DEBUG")]
    private static void Asserts(string propertyKey, IEnumerable<KeyValuePair<string, ReadOnlyMemory<char>>> propertyParameters)
    {
        Debug.Assert(propertyKey is not null);
        Debug.Assert(propertyParameters is not null);
        Debug.Assert(!propertyParameters.Any(x => string.IsNullOrWhiteSpace(x.Key)));
        Debug.Assert(StringComparer.Ordinal.Equals(propertyKey, propertyKey.ToUpperInvariant()));
    }

    private void AddNonStandardParameter(in KeyValuePair<string, ReadOnlyMemory<char>> parameter)
    {
        List<KeyValuePair<string, string>> userAttributes
                                        = (List<KeyValuePair<string, string>>?)this.NonStandard
                                          ?? [];

        this.NonStandard = userAttributes;
        userAttributes.Add(new KeyValuePair<string, string>(parameter.Key, parameter.Value.ToString()));
    }

    private bool ParseTypeParameter(string typeValue, string propertyKey)
    {
        typeValue = typeValue.Trim(TRIM_CHARS).ToUpperInvariant();
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
