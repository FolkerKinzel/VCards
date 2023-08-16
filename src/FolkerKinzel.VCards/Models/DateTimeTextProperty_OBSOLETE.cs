﻿using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Spezialisierung der <see cref="DateTimeProperty"/>-Klasse, die es erlaubt eine Zeit- und/oder Datumsangabe
/// als freien Text zu speichern.
/// </summary>
public sealed class DateTimeTextProperty : DateTimeProperty, IEnumerable<DateTimeTextProperty>
{
    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="prop"></param>
    private DateTimeTextProperty(DateTimeTextProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>
    /// Initialisiert ein neues <see cref="DateTimeTextProperty"/>-Objekt, bei dem der 
    /// <see cref="ParameterSection.DataType"/>-Parameter
    /// auf <see cref="VCdDataType.Text"/> gesetzt ist.
    /// </summary>
    /// <param name="value">Ein beliebiger <see cref="string"/>, der eine Zeit oder ein Datum beschreibt, oder <c>null</c>.</param>
    /// <param name="propertyGroup">Bezeichner der Gruppe,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    public DateTimeTextProperty(string? value, string? propertyGroup = null) : base(new ParameterSection(), propertyGroup)
    {
        this.Value = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        Parameters.DataType = VCdDataType.Text;
    }


    internal DateTimeTextProperty(VcfRow vcfRow, VCdVersion version) : base(vcfRow.Parameters, vcfRow.Group)
    {
        vcfRow.UnMask(version);

        if (vcfRow.Value != null)
        {
            Value = vcfRow.Value;
        }
    }

    /// <summary>
    /// Die von der <see cref="DateTimeTextProperty"/> zur Verfügung gestellten Daten.
    /// </summary>
    public new string? Value
    {
        get;
    }


    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;



    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        base.PrepareForVcfSerialization(serializer);

        this.Parameters.DataType = VCdDataType.Text;
    }


    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        StringBuilder builder = serializer.Builder;
        StringBuilder worker = serializer.Worker;

        _ = worker.Clear().Append(this.Value).Mask(serializer.Version);
        _ = builder.Append(worker);
    }

    IEnumerator<DateTimeTextProperty> IEnumerable<DateTimeTextProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc/>
    public override object Clone() => new DateTimeTextProperty(this);

}