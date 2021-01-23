using System;
using System.IO;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.Helpers;

namespace Examples
{
    internal class Program
    {
        //static void Main() => VCardExample.ReadingAndWritingVCard();

        private static void Main()
        {
            string directoryPath = Path.GetFullPath("TestFiles");

            if(Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            _ = Directory.CreateDirectory(directoryPath);

            VCard40Example.SaveSingleVCardAsVcf(directoryPath);
        }
    }
}
