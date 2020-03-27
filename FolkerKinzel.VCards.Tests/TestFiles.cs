using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace FolkerKinzel.VCards.Tests
{
    internal static class TestFiles
    {
        private const string TEST_FILE_DIRECTORY_NAME = "TestFiles";

        static readonly string _testFileDirectory;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1810:Statische Felder für Referenztyp inline initialisieren", Justification = "<Ausstehend>")]
        static TestFiles()
        {
            ProjectDirectory = Properties.Resources.ProjDir.Trim();
            _testFileDirectory = Path.Combine(ProjectDirectory, TEST_FILE_DIRECTORY_NAME);
        }


        internal static string[] GetAll()
        {
            return Directory.GetFiles(_testFileDirectory);
        }


        internal static string ProjectDirectory { get; }


        

        internal static string V2vcf => Path.Combine(_testFileDirectory, "v2_1.vcf");
        internal static string V3vcf => Path.Combine(_testFileDirectory, "v3.vcf");
        internal static string V4vcf => Path.Combine(_testFileDirectory, "v4.vcf");




    }
}
