﻿using System.IO;


namespace FolkerKinzel.VCards.Tests
{
    internal static class TestFiles
    {
        private const string TEST_FILE_DIRECTORY_NAME = "TestFiles";
        private static readonly string _testFileDirectory;

        static TestFiles()
        {
            ProjectDirectory = Properties.Resources.ProjDir.Trim();
            _testFileDirectory = Path.Combine(ProjectDirectory, TEST_FILE_DIRECTORY_NAME);
        }


        internal static string[] GetAll() => Directory.GetFiles(_testFileDirectory);


        internal static string ProjectDirectory { get; }

        internal static string LibraryV2vcf => Path.Combine(_testFileDirectory, "Library v2.vcf");
        internal static string PhotoOutlookV2vcf => Path.Combine(_testFileDirectory, "Photo Outlook v2.vcf");
        internal static string OutlookV2vcf => Path.Combine(_testFileDirectory, "Outlook v2.vcf");
        internal static string V2vcf => Path.Combine(_testFileDirectory, "v2_1.vcf");
        internal static string V3vcf => Path.Combine(_testFileDirectory, "v3.vcf");
        internal static string V4vcf => Path.Combine(_testFileDirectory, "v4.vcf");
        internal static string PhotoV3vcf => Path.Combine(_testFileDirectory, "Photo v3.vcf");
        internal static string WhatsAppIssueVcf => Path.Combine(_testFileDirectory, "WhatsAppIssue.vcf");





    }
}
