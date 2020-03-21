using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace FolkerKinzel.VCards_Tests
{
    static class VcfPaths
    {
        private const string BASE_DIRECTORY = @"C:\Users\fkinz\source\repos\FolkerKinzel.VCards\FolkerKinzel.VCardsTests\TestVcf";
        //private const string BASE_DIRECTORY = "$(ProjectDir)TestVcf";

        public static readonly string Vcard_2_1_Path;
        public static readonly string Vcard_3_0_Path;
        public static readonly string Vcard_4_0_Path;

        public static readonly string V2_1_TEST_DIRECTORY;
        public static readonly string V3_0_TEST_DIRECTORY;
        public static readonly string V4_0_TEST_DIRECTORY;

        static VcfPaths()
        {
            const string CreatedDirectoryName = "Created";

            Vcard_2_1_Path = Path.Combine(BASE_DIRECTORY, @"v2_1.vcf");
            Vcard_3_0_Path = Path.Combine(BASE_DIRECTORY, @"v3.vcf");
            Vcard_4_0_Path = Path.Combine(BASE_DIRECTORY, @"v4.vcf");

            V2_1_TEST_DIRECTORY = Path.Combine(BASE_DIRECTORY, CreatedDirectoryName, "2.1");
            V3_0_TEST_DIRECTORY = Path.Combine(BASE_DIRECTORY, CreatedDirectoryName, "3.0");
            V4_0_TEST_DIRECTORY = Path.Combine(BASE_DIRECTORY, CreatedDirectoryName, "4.0");
        }

        public static bool VerifyPaths(out string error)
        {
            Directory.CreateDirectory(V2_1_TEST_DIRECTORY);
            Directory.CreateDirectory(V3_0_TEST_DIRECTORY);
            Directory.CreateDirectory(V4_0_TEST_DIRECTORY);

            try
            {
                if(!File.Exists(Vcard_2_1_Path))
                {
                    error = "Vcard_2_1_Path: " + "Der Pfad " + Vcard_2_1_Path + "existiert nicht.";
                    return false;
                }

                if (!File.Exists(Vcard_3_0_Path))
                {
                    error = "Vcard_3_0_Path: " + "Der Pfad " + Vcard_3_0_Path + "existiert nicht.";
                    return false;
                }

                if (!File.Exists(Vcard_4_0_Path))
                {
                    error = "Vcard_4_0_Path: " + "Der Pfad " + Vcard_4_0_Path + "existiert nicht.";
                    return false;
                }
            }
            catch(Exception e)
            {
                error = e.ToString();
                return false;
            }

            error = string.Empty;
            return true;
        }
    }
}
