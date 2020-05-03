using System;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.Helpers;

namespace ExampleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            TelTypes? tel = null;

            tel = tel.Set(TelTypes.Fax);

            Console.WriteLine(tel);



        }
    }
}
