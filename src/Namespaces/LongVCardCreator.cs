using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;

namespace Namespaces;

internal class Name
{
    internal string? LastName { get; set; }
    internal string? FirstName { get; set; }
    internal string? MiddleName { get; set; }

}

internal static class LongVCardCreator
{
    internal static void Create()
    {
        var faker = new Faker<Name>();
        faker
            .RuleFor(nameof(Name.LastName), (f, n) => n.LastName = f.Name.LastName())
            .RuleFor(nameof(Name.FirstName), (f, n) => n.FirstName = f.Name.FirstName())
            .RuleFor(nameof(Name.MiddleName), (f, n) => n.MiddleName = f.Name.FirstName().OrNull(in f));

        List<Name> name = faker.Generate(1000);
        var vCards = new List<VCard>(1000);

        for (int i = 0; i < name.Count; i++)
        {
            Name name2 = name[i];

            vCards.Add
                (
                VCardBuilder
                .Create(false)
                .NameViews.Add(familyName: name2.LastName,
                               givenName: name2.FirstName,
                               additionalName: name2.MiddleName,
                               displayName: (n, p) => n.Add(p.ToDisplayName()))
                .VCard
                );
        }

        Vcf.Save(vCards, "LargeFile.vcf", options: Opts.Default.Unset(Opts.UpdateTimeStamp));

    }
}
