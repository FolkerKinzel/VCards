using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

#if !NET40
using FolkerKinzel.Strings;
#endif

namespace FolkerKinzel.VCards.Models.PropertyParts
{
    public sealed partial class ParameterSection
    {
        internal ParameterSection() { }

        /// <summary>
        /// Copy ctor
        /// </summary>
        /// <param name="paraSec">The <see cref="ParameterSection"/> object to clone.</param>
        private ParameterSection(ParameterSection paraSec)
        {
            foreach (var kvp in paraSec._propDic)
            {
                Set(kvp.Key, kvp.Value switch
                {
                    PropertyID pid => pid,
                    IEnumerable<PropertyID?> pidEnumerable => pidEnumerable.ToArray(),
                    IEnumerable<string?> stringEnumerable => stringEnumerable.ToArray(),
                    IEnumerable<KeyValuePair<string, string>> kvpEnumerable => kvpEnumerable.ToArray(),

                    _ => kvp.Value
                });
            }
        }


        internal ParameterSection(string propertyKey, IEnumerable<KeyValuePair<string, string>> propertyParameters, VcfDeserializationInfo info)
        {
            #region DebugAssert

            Debug.Assert(propertyKey != null);
            Debug.Assert(propertyParameters != null);
            Debug.Assert(!propertyParameters.Any(
                x => string.IsNullOrWhiteSpace(x.Key) || string.IsNullOrWhiteSpace(x.Value)
                ));
            Debug.Assert(StringComparer.Ordinal.Equals(propertyKey, propertyKey.ToUpperInvariant()));
            Debug.Assert(propertyParameters.All(x => StringComparer.Ordinal.Equals(x.Key, x.Key.ToUpperInvariant())));

            #endregion

            StringBuilder builder = info.Builder;

            foreach (KeyValuePair<string, string> parameter in propertyParameters)
            {
                switch (parameter.Key)
                {
                    case ParameterKey.LANGUAGE:
                        this.Language = parameter.Value;
                        break;
                    case ParameterKey.VALUE:
                        {
                            string valValue = CleanParameterValue(parameter.Value, builder);
                            VCdDataType? dataType = VCdDataTypeConverter.Parse(valValue);
                            this.DataType = dataType;

                            if (!dataType.HasValue)
                            {
                                VCdContentLocation contentLocation = VCdContentLocationConverter.Parse(valValue);
                                this.ContentLocation = contentLocation;

                                if (contentLocation == VCdContentLocation.Url)
                                {
                                    this.DataType = VCdDataType.Uri;
                                }
                            }

                            break;
                        }
                    case ParameterKey.PREF:
                        {
                            if (int.TryParse(parameter.Value.Trim().Trim(info.AllQuotes), out int intVal))
                            {
                                this.Preference = intVal;
                            }
                            break;
                        }
                    case ParameterKey.PID:
                        {
                            List<PropertyID> list = (List<PropertyID>?)this.PropertyIDs ?? new List<PropertyID>();
                            this.PropertyIDs = list;

                            PropertyID.ParseInto(list, parameter.Value);

                            break;
                        }
                    case ParameterKey.TYPE:
                        {
                            ValueSplitter commaSplitter = info.CommaSplitter;

                            commaSplitter.ValueString = parameter.Value;
                            foreach (var s in commaSplitter)
                            {
                                string typeValue = CleanParameterValue(s, builder);

                                Debug.Assert(typeValue.Length != 0);
                                if (!ParseTypeParameter(typeValue, propertyKey))
                                {
                                    List<KeyValuePair<string, string>> nonStandardList = (List<KeyValuePair<string, string>>?)this.NonStandardParameters ?? new List<KeyValuePair<string, string>>();
                                    this.NonStandardParameters = nonStandardList;

                                    nonStandardList.Add(new KeyValuePair<string, string>(parameter.Key, s));
                                }
                            }
                            break;
                        }
                    case ParameterKey.GEO:
                        if (FolkerKinzel.VCards.Models.GeoCoordinate.TryParse(parameter.Value.Trim().Trim(info.AllQuotes), out FolkerKinzel.VCards.Models.GeoCoordinate? geo))
                        {
                            this.GeoPosition = geo;
                        }
                        break;
                    case ParameterKey.TZ:
                        try
                        {
                            this.TimeZone = new TimeZoneID(parameter.Value.Trim().Trim(info.AllQuotes));
                        }
                        catch { }
                        break;
                    case ParameterKey.SORT_AS:
                        {
                            List<string> list = (List<string>?)this.SortAs ?? new List<string>();
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
                        this.Calendar = parameter.Value.Trim().Trim(info.AllQuotes);
                        break;
                    case ParameterKey.ENCODING:
                        this.Encoding = VCdEncodingConverter.Parse(parameter.Value);
                        break;
                    case ParameterKey.CHARSET:
                        this.Charset = parameter.Value.Trim().Trim(info.AllQuotes);
                        break;
                    case ParameterKey.ALTID:
                        this.AltID = parameter.Value.Trim().Trim(info.AllQuotes);
                        break;
                    case ParameterKey.MEDIATYPE:
                        this.MediaType = parameter.Value.Trim().Trim(info.AllQuotes);
                        break;
                    case ParameterKey.LABEL:
                        this.Label = builder
                            .Clear()
                            .Append(parameter.Value)
                            .Trim().RemoveQuotes()
                            .Replace(@"\n", Environment.NewLine)
                            .Replace(@"\N", Environment.NewLine)
                            .ToString();
                        break;
                    case ParameterKey.CONTEXT:
                        this.Context = parameter.Value.Trim().Trim(info.AllQuotes);
                        break;
                    case ParameterKey.INDEX:
                        {
                            if (int.TryParse(parameter.Value.Trim().Trim(info.AllQuotes), out int result))
                            {
                                this.Index = result;
                            }
                            break;
                        }
                    case ParameterKey.LEVEL:
                        if (propertyKey == VCard.PropKeys.NonStandard.EXPERTISE)
                        {
                            ExpertiseLevel? expertise = ExpertiseLevelConverter.Parse(CleanLevelValue(parameter.Value, builder));

                            if (expertise.HasValue)
                            {
                                this.ExpertiseLevel = expertise;
                            }
                            else
                            {
                                AddNonStandardParameter(parameter);
                            }
                        }
                        else // HOBBY oder INTEREST
                        {
                            InterestLevel? interest = InterestLevelConverter.Parse(CleanLevelValue(parameter.Value, builder));

                            if (interest.HasValue)
                            {
                                this.InterestLevel = interest;
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

        private void AddNonStandardParameter(KeyValuePair<string, string> parameter)
        {
            List<KeyValuePair<string, string>> userAttributes
                                            = (List<KeyValuePair<string, string>>?)this.NonStandardParameters ?? new List<KeyValuePair<string, string>>();
            this.NonStandardParameters = userAttributes;

            userAttributes.Add(parameter);
        }

        private static string CleanParameterValue(string parameterValue, StringBuilder builder)
        {
            bool clean = false;

            for (int i = 0; i < parameterValue.Length; i++)
            {
                char c = parameterValue[i];

                if (char.IsLower(c) || char.IsWhiteSpace(c) || c == '\'' || c == '\"')
                {
                    clean = true;
                    break;
                }
            }

            if (!clean)
            {
                return parameterValue;
            }


            _ = builder.Clear();

            for (int i = 0; i < parameterValue.Length; i++)
            {
                char c = parameterValue[i];

                if (char.IsWhiteSpace(c) || c == '\'' || c == '\"')
                {
                    continue;
                }

                _ = builder.Append(char.ToUpperInvariant(c));
            }

            return builder.ToString();
        }


        private static string CleanLevelValue(string parameterValue, StringBuilder builder)
        {
            bool clean = false;

            for (int i = 0; i < parameterValue.Length; i++)
            {
                char c = parameterValue[i];

                if (char.IsUpper(c) || char.IsWhiteSpace(c) || c == '\'' || c == '\"')
                {
                    clean = true;
                    break;
                }
            }

            if (!clean)
            {
                return parameterValue;
            }


            _ = builder.Clear();

            for (int i = 0; i < parameterValue.Length; i++)
            {
                char c = parameterValue[i];

                if (char.IsWhiteSpace(c) || c == '\'' || c == '\"')
                {
                    continue;
                }

                _ = builder.Append(char.ToLowerInvariant(c));
            }

            return builder.ToString();
        }



        private bool ParseTypeParameter(string typeValue, string propertyKey)
        {
            switch (typeValue)
            {
                case TypeValue.PREF:
                    this.Preference = 1;
                    return true;
                case TypeValue.HOME:
                    this.PropertyClass = this.PropertyClass.Set(Enums.PropertyClassTypes.Home);
                    return true;
                case TypeValue.WORK:
                    this.PropertyClass = this.PropertyClass.Set(Enums.PropertyClassTypes.Work);
                    return true;
                default:
                    break;
            }


            switch (propertyKey)
            {
                case VCard.PropKeys.ADR:
                    {
                        AddressTypes? addressType = AddressTypesConverter.Parse(typeValue);

                        if (addressType.HasValue)
                        {
                            this.AddressType = this.AddressType.Set(addressType.Value);
                            return true;
                        }

                        return false;
                    }
                case VCard.PropKeys.TEL:
                    {
                        TelTypes? telType = TelTypesConverter.Parse(typeValue);

                        if (telType.HasValue)
                        {
                            this.TelephoneType = this.TelephoneType.Set(telType.Value);
                            return true;
                        }

                        return false;
                    }
                case VCard.PropKeys.RELATED:
                    {
                        RelationTypes? relType = RelationTypesConverter.Parse(typeValue);

                        if (relType.HasValue)
                        {
                            this.RelationType = this.RelationType.Set(relType.Value);
                            return true;
                        }

                        return false;
                    }
                case VCard.PropKeys.EMAIL:
                    this.EmailType = propertyKey;
                    break;
                case VCard.PropKeys.KEY:
                    this.MediaType = MimeTypeConverter.MimeTypeFromEncryptionTypeValue(typeValue);
                    break;
                case VCard.PropKeys.SOUND:
                    this.MediaType = MimeTypeConverter.MimeTypeFromSoundTypeValue(typeValue);
                    break;
                case VCard.PropKeys.PHOTO:
                case VCard.PropKeys.LOGO:
                    this.MediaType = MimeTypeConverter.MimeTypeFromImageTypeValue(typeValue);
                    break;
                case VCard.PropKeys.IMPP:
                    {
                        ImppTypes? imppType = ImppTypesConverter.Parse(typeValue);

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
}
