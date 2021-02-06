using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLA_Video_Randomizer
{
    class Program
    {

        public static FileStream config = new FileStream(@".\config.json", FileMode.OpenOrCreate);
        public static string restOfPath = @"\game\hlvr\panorama\videos";
        public const string file1 = "advisor_screen.webm";
        public const string file2 = "citadel_building_recruitment.webm";
        public const string file3 = "combine_alarm000.webm";
        public const string file4 = "combinescreen.webm";
        public const string file5 = "intro_world_heist.webm";
        public const string file6 = "wupgrade_boot.webm";
        public const string file7 = "wupgrade_frabrication.webm";
        public const string file8 = "wupgrade_loop.webm";
        public static bool hasWebMs = true, hasErrored = false;
        static configs defaultConfig = new configs();
        static void Main(string[] args)
        {
            try
            {
                if (config.Length == 0)
                {
                    writeConfig();
                }
                readConfig();
                if (defaultConfig.HLAPath.EndsWith(@"\"))
                {
                    restOfPath = @"game\hlvr\panorama\videos";
                }
            }
            catch
            {
                hasErrored = true;
                Console.WriteLine("There was an error with your config. Try editing the config again, or delete it and let it re-generate.");
            }
            if (!hasErrored)
            {
                System.IO.Directory.CreateDirectory(@".\put WebMs Here");
                string[] files = Directory.GetFiles(@".\put WebMs Here", @"*.webm");
                string fullPath = defaultConfig.HLAPath + restOfPath;
                if (files.Length == 0)
                {
                    Console.WriteLine("Put your webms in the folder called \"put WebMs Here\" then run the program again.");
                    hasWebMs = false;
                }
                if (!Directory.Exists(fullPath))
                {
                    Console.WriteLine("Check the HLAPath, the path doesn't seem to exist.");
                    hasWebMs = false;
                }

                System.IO.Directory.CreateDirectory(@".\Backup Files");

                string[] files2 = Directory.GetFiles(fullPath, "*.webm");
                string[] files3 = Directory.GetFiles(@".\Backup Files");


                if (hasWebMs)
                {
                    if (files3.Length == 0)
                    {
                        foreach (var file in files2)
                        {
                            string fName = file.Substring(fullPath.Length + 1);

                            File.Copy(Path.Combine(fullPath, fName), @".\Backup Files\" + fName);
                        }
                    }
                    Random rand = new Random();

                    File.Copy(files[rand.Next(files.Length)], Path.Combine(fullPath, file1), true);
                    File.Copy(files[rand.Next(files.Length)], Path.Combine(fullPath, file2), true);
                    File.Copy(files[rand.Next(files.Length)], Path.Combine(fullPath, file3), true);
                    File.Copy(files[rand.Next(files.Length)], Path.Combine(fullPath, file4), true);
                    File.Copy(files[rand.Next(files.Length)], Path.Combine(fullPath, file5), true);
                    File.Copy(files[rand.Next(files.Length)], Path.Combine(fullPath, file6), true);
                    File.Copy(files[rand.Next(files.Length)], Path.Combine(fullPath, file7), true);
                    File.Copy(files[rand.Next(files.Length)], Path.Combine(fullPath, file8), true);
                    Console.WriteLine("Operation Successful");
                }
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey(); 
            
        }

        public static void readConfig()
        {
            config.Position = 0;
            byte[] cResult = new byte[config.Length];
            config.Read(cResult, 0, (int)config.Length);
            string result = System.Text.Encoding.ASCII.GetString(cResult);
            StringBuilder builder = new StringBuilder();
            char[] splitted = result.ToCharArray();
            List<char> combined = new List<char>();
            for (int i = 0; i < splitted.Count(); i++)
            {
                combined.Add(splitted[i]);
                if (splitted[i] == '\\')
                {
                    if (combined[i - 1] != '\\'&&splitted[i+1]!='\\') combined.Add('\\');
                }
            }
            string full = new string(combined.ToArray());
            var jsonResult = JObject.Parse(full);
            defaultConfig = jsonResult.ToObject<configs>();
        }

        public static void writeConfig()
        {
            byte[] array = Encoding.ASCII.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(defaultConfig));
            config.Position = 0;
            config.WriteAsync(array, 0, array.Length);
            config.FlushAsync();
        }
    }
    class configs
    {
        public string HLAPath = @"C:\Program Files (x86)\Steam\steamapps\common\Half-Life Alyx";
    }
}
