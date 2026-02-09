using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards;

namespace Net472Issue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var nb = NameBuilder.Create();
            nb.AddGiven("Susi")
              .AddSurname("Sonntag");

            var builder = VCardBuilder.Create(true, true);

            builder.NameViews.Add(nb.Build());

            Console.WriteLine(builder.VCard.NameViews.First().Value.Given[0]);
        }
    }
}
