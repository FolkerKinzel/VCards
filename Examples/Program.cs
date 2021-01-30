using System;
using System.IO;
using FolkerKinzel.VCards.Models.Enums;

namespace Examples
{
    internal class Program
    {
        private static void Main()
        {
            string directoryPath = Path.GetFullPath("TestFiles");

            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            _ = Directory.CreateDirectory(directoryPath);


            VCardExample.ReadingAndWritingVCard(directoryPath);
            //VCard40Example.SaveSingleVCardAsVcf(directoryPath);
        }
    }
}
