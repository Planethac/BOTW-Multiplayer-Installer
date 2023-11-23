using System;
using System.IO;
using System.Text.Json;

namespace botwm
{

    public class Settings
    {
        public double installedVer { get; set; }
    }
    public class Setup
    {
        public static void Main(String[] args)
        {
            makeJson.settings();
            Console.WriteLine("BOTW Multiplayer Setup");
            if (File.Exists("settings.json"))
            {
                String configFile = "settings.json";
                String SearializedJson = File.ReadAllText(configFile);
                Settings settings = JsonSerializer.Deserialize<Settings>(SearializedJson);
                if (settings.installedVer == null) settings.installedVer = -1;
                if (!(settings.installedVer == -1)) Console.WriteLine($"Current Ver: {settings.installedVer}");
            }
            else
            {
                Console.WriteLine("Current Ver: NOT FOUND");
            }
        }
    }
    
    public class makeJson
    {
        public static void settings()
        {
            var Jsettings = new Settings
            {
                installedVer = 1.1
            };
            String fName = "settings.json";
            String json = JsonSerializer.Serialize<Settings>(Jsettings);
            File.WriteAllText(fName, json);
        }
    }
}