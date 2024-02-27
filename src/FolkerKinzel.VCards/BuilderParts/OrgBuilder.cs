﻿using System.ComponentModel;
using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals", Justification = "<Pending>")]
public readonly struct OrgBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))] 
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal OrgBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(string? orgName,
                            IEnumerable<string?>? orgUnits = null,
                            bool pref = false,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.Organizations,
                           VCardBuilder.Add(new OrgProperty(orgName, orgUnits, group?.Invoke(_builder.VCard)),
                                            _builder.VCard.Get<IEnumerable<OrgProperty?>?>(Prop.Organizations),
                                            parameters,
                                            pref));
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.Organizations"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="OrgBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.Organizations, null);
        return _builder;
    }

    /// <summary>
    /// Removes <see cref="OrgProperty"/> objects that match a specified predicate from the <see cref="VCard.Organizations"/> property.
    /// </summary>
    /// <param name="predicate">A function that returns <c>true</c> for <see cref="OrgProperty"/> objects that shall be removed.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="OrgBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder Remove(Func<OrgProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.Organizations,
                           _builder.VCard.Get<IEnumerable<OrgProperty?>?>(Prop.Organizations).Remove(predicate));
        return _builder;
    }

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString()!;

}

