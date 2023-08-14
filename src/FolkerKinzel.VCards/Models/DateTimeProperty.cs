using System.Collections.ObjectModel;
using FolkerKinzel.VCards.Models.PropertyParts;
using OneOf;

namespace FolkerKinzel.VCards.Models;

[GenerateOneOf]
public partial class DateAndOrTime : OneOfBase<DateOnly, DateTime, DateTimeOffset, string> { }

public abstract class DateTimeProperty : VCardProperty
{
    private DateAndOrTime? _value;
    private bool _isValueInitialized;

    protected DateTimeProperty(VCardProperty prop) : base(prop)
    {
    }

    protected DateTimeProperty(ParameterSection parameters, string? propertyGroup) : base(parameters, propertyGroup)
    {
    }


    public new DateAndOrTime? Value
    {
        get
        {
            if (!_isValueInitialized)
            {
                InitializeValue();
            }

            return _value;
        }
    }


    private void InitializeValue()
    {
        _isValueInitialized = true;
        _value = GetVCardPropertyValue() switch
        {

            _ => throw new NotImplementedException()
        };
    }
}
