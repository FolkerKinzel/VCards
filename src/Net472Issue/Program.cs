using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;

namespace Net472Issue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var nb = NameBuilder.Create();
            nb.AddGiven("Jürgen")
              .AddSurname("Müller");


            var builder = VCardBuilder.Create(true, true);

            builder.NameViews.Add(nb.Build());
            builder.NameViews.ToDisplayNames(NameFormatter.Default);

            Console.WriteLine(builder.VCard.DisplayNames.First().Value);

            //File.WriteAllText("vCard3_0.vcf", Vcf.AsString(builder.VCard), Encoding.GetEncoding(1252));
        }
    }
}
