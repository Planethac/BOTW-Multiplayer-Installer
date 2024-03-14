using System.Text.Json;
using System.Net;
using System.IO.Compression;
using System.Diagnostics;
using System.Runtime.InteropServices;
using botwm.AdnvancedMode;

namespace botwm
{

    public class Settings
    {
        public double installedVer { get; set; }
        public bool py { get; set; }
        public bool cemu { get; set; }
        public bool bcml { get; set; }
        public bool game { get; set; }
    }
    public class Setup
    {
        public static void Main(String[] args)
        {
            if (File.Exists("dwnBcml"))
            {
                String[] bcml = { "bcml" };
                InstallSoftware(bcml);
            }
            
            Logger.init();
            Console.WriteLine("BOTW Multiplayer Setup");
            if (File.Exists("settings.json"))
            {
                String configFile = "settings.json";
                String SearializedJson = File.ReadAllText(configFile);
                Settings settings = JsonSerializer.Deserialize<Settings>(SearializedJson);
                if (settings.installedVer == null) settings.installedVer = -1;
                if (!(settings.installedVer == -1)) Console.WriteLine($"Current Ver: {settings.installedVer}");
                Console.WriteLine();
                Console.WriteLine("Following Modules found");
                if (settings.py) Console.WriteLine("Module: Python 3.11");
                if (settings.bcml) Console.WriteLine("Module: bcml");
                if (settings.cemu) Console.WriteLine("Module: Cemu");
                if (settings.game) Console.WriteLine("Module: Mod Files");
            }
            else
            {
                Console.WriteLine("Current Ver: NOT FOUND");
            }

            String[] modules = { "py", "cemu", "bcml", "game" };
            Console.WriteLine();
            Console.WriteLine("Installing Following modules");
            Console.WriteLine("[X] Python 3.11");
            Console.WriteLine("[X] Cemu");
            Console.WriteLine("[X] Bcml");
            Console.WriteLine("[X] Mod Files");
            Console.WriteLine();
            Console.WriteLine("To Change the list of modules to install type [C]");
            Console.WriteLine("To Acces Advanced Mode type [A]");
            Console.WriteLine("To Load autorun.json type [L]");
            Console.WriteLine("To Proced with instalation type [Y] (recommended)");
            Console.Write(": ");
            String readline1 = Console.ReadLine();
            if (readline1.ToLower() == "y") InstallSoftware(modules);
            if (readline1.ToLower() == "c") ChangeModules();
            if (readline1.ToLower() == "g")
            {
                Console.WriteLine("Writing Test Json");
                makeJson.settings(modules);
            }
            if (readline1.ToLower() == "l") AdvancedMode.LoadFile("autorun.json");
            if (readline1.ToLower() == "a") /*TODO: Advanced Mode */ AdvancedMode.Init();
        }

        public static void InstallSoftware(String[] modules)
        {
            if (modules.Contains("game"))
            {
                Console.Clear();
                Console.WriteLine("[ ] Module: Mod Files");
                WebClient webClient = new WebClient();
                Directory.CreateDirectory("BOTW-COOP");
                Directory.CreateDirectory(@"BOTW-COOP\tmp");
                webClient.DownloadFile("https://cdn.planethac.me/botw/Downloads/Data/BOTW.multiplayer_1.0.4_files__setup_tutorial.zip", @"BOTW-COOP\tmp\mod-data.zip");
                try
                {
                    ZipFile.ExtractToDirectory(@"BOTW-COOP\tmp\mod-data.zip", "BOTW-COOP");
                }
                catch(Exception ex)
                {

                }
                List<String> files = Directory.GetFiles(@"BOTW-COOP\BOTW.multiplayer 1.0.4 files + setup tutorial\Breath of the Wild Multiplayer files", "*.*", SearchOption.TopDirectoryOnly).ToList();
                List<String> backs = Directory.GetFiles(@"BOTW-COOP\BOTW.multiplayer 1.0.4 files + setup tutorial\Breath of the Wild Multiplayer files\Backgrounds", "*.*", SearchOption.TopDirectoryOnly).ToList();
                List<String> bnps = Directory.GetFiles(@"BOTW-COOP\BOTW.multiplayer 1.0.4 files + setup tutorial\Breath of the Wild Multiplayer files\BNPs", "*.*", SearchOption.TopDirectoryOnly).ToList();
                List<String> serv = Directory.GetFiles(@"BOTW-COOP\BOTW.multiplayer 1.0.4 files + setup tutorial\Breath of the Wild Multiplayer files\DedicatedServer", "*.*", SearchOption.TopDirectoryOnly).ToList();
                List<String> rcs = Directory.GetFiles(@"BOTW-COOP\BOTW.multiplayer 1.0.4 files + setup tutorial\Breath of the Wild Multiplayer files\Resources", "*.*", SearchOption.TopDirectoryOnly).ToList();
                List<String> tmp = Directory.GetFiles(@"BOTW-COOP\tmp", "*.*", SearchOption.TopDirectoryOnly).ToList();
                DirectoryInfo directoryInfo = new DirectoryInfo(@"BOTW-COOP");
                Directory.CreateDirectory(@"BOTW-COOP\Backgrounds");
                Directory.CreateDirectory(@"BOTW-COOP\BNPs");
                Directory.CreateDirectory(@"BOTW-COOP\DedicatedServer");
                Directory.CreateDirectory(@"BOTW-COOP\Resources");
                foreach (String file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (new FileInfo(directoryInfo + "\\" + fileInfo.Name).Exists == false)
                    {
                        fileInfo.MoveTo(@"BOTW-COOP\" + fileInfo.Name);
                    }
                }
                foreach (String file in backs)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (new FileInfo(directoryInfo + "\\Backgrounds" + fileInfo.Name).Exists == false)
                    {
                        fileInfo.MoveTo("BOTW-COOP\\Backgrounds\\" + fileInfo.Name);
                    }
                }
                foreach (String file in bnps)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (new FileInfo(directoryInfo + "\\BNPs" + fileInfo.Name).Exists == false)
                    {
                        fileInfo.MoveTo(@"BOTW-COOP\BNPs\" + fileInfo.Name);
                    }
                }
                foreach (String file in serv)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (new FileInfo(directoryInfo + "\\DedicatedServer" + fileInfo.Name).Exists == false)
                    {
                        fileInfo.MoveTo(@"BOTW-COOP\DedicatedServer\" + fileInfo.Name);
                    }
                }
                foreach (String file in rcs)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (new FileInfo(directoryInfo + "\\Resources" + fileInfo.Name).Exists == false)
                    {
                        fileInfo.MoveTo(@"BOTW-COOP\Resources\" + fileInfo.Name);
                    }
                }
                foreach (String file in tmp)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    fileInfo.Delete();
                }
                Directory.Delete(@"BOTW-COOP\tmp");
                Console.Clear();
                Console.WriteLine("[X] Module: Mod Files");
                Thread.Sleep(1000);
            }

            if (modules.Contains("py"))
            {
                Console.Clear();
                if (modules.Contains("game"))
                {
                    Console.WriteLine("[X] Module: Mod Files");
                }

                Console.WriteLine("[ ] Module: Python 3.11");
                Directory.CreateDirectory(@"BOTW-COOP\py-tmp");
                WebClient webClient = new WebClient();
                webClient.DownloadFile("https://www.python.org/ftp/python/3.11.2/python-3.11.2-amd64.exe", @"BOTW-COOP\py-tmp\python-setup.exe");
                Process py = new Process();
                py.StartInfo.FileName = @"BOTW-COOP\py-tmp\python-setup.exe";
                py.Start();
                py.WaitForExit();
                File.Delete(@"BOTW-COOP\py-tmp\python-setup.exe");
                Directory.Delete(@"BOTW-COOP\py-tmp");
                Process clitools = new Process();
                clitools.StartInfo.FileName = "pip";
                clitools.StartInfo.Arguments = "install pathlib";
                clitools.Start();
                clitools.WaitForExit();
                clitools.StartInfo.FileName = "pip";
                clitools.StartInfo.Arguments = "install argparse";
                clitools.Start();
                clitools.WaitForExit();
                Console.Clear();
                if (modules.Contains("game"))
                {
                    Console.WriteLine("[X] Module: Mod Files");
                }
                Console.WriteLine("[X] Module: Python 3.11");
            }
            
            if (modules.Contains("cemu"))
            {
                Console.Clear();
                if (modules.Contains("game"))
                {
                    Console.WriteLine("[X] Module: Mod Files");
                }

                if (modules.Contains("py"))
                {
                    Console.WriteLine("[X] Module: Python 3.11");
                }

                Console.WriteLine("[ ] Module: Cemu");
                WebClient webClient = new WebClient();
                webClient.DownloadFile("https://cemu.info/releases/cemu_1.26.2.zip", "cemu.zip");
                Directory.CreateDirectory(@"BOTW-COOP\cemu");
                try
                {
                    ZipFile.ExtractToDirectory("cemu.zip", @"BOTW-COOP\cemu");
                }
                catch (Exception e)
                {
                    
                }
                File.Delete("cemu.zip");
                Console.Clear();
                if (modules.Contains("game"))
                {
                    Console.WriteLine("[X] Module: Mod Files");
                }

                if (modules.Contains("py"))
                {
                    Console.WriteLine("[X] Module: Python 3.11");
                }

                Console.WriteLine("[X] Module: Cemu");
            }

            if (modules.Contains("bcml"))
            {
                Console.Clear();
                if (modules.Contains("game"))
                {
                    Console.WriteLine("[X] Module: Mod Files");
                }

                if (modules.Contains("py"))
                {
                    Console.WriteLine("[X] Module: Python 3.11");
                }

                if (modules.Contains("cemu"))
                {
                    Console.WriteLine("[X] Module: Cemu");
                }

                Console.WriteLine("[ ] Module: bcml");

                if (modules.Contains("py"))
                {
                    Console.WriteLine("As you have installed Python PIP is not in path");
                    Console.WriteLine("Please rerun the exe an the install will continue");
                    Console.WriteLine("Closing in 5 seconds");
                    File.Create("dwnBcml");
                    Thread.Sleep(5000);
                    Environment.Exit(0);
                    
                }
                else
                {
                    Process bcml = new Process();
                    bcml.StartInfo.FileName = "pip";
                    bcml.StartInfo.Arguments = "install bcml";
                    bcml.Start();
                    bcml.WaitForExit();
                }
                
                if (File.Exists("dwnBcml"))
                {
                    File.Delete("dwnBcml");
                    Environment.Exit(0);
                }
                
                Console.Clear();
                if (modules.Contains("game"))
                {
                    Console.WriteLine("[X] Module: Mod Files");
                }

                if (modules.Contains("py"))
                {
                    Console.WriteLine("[X] Module: Python 3.11");
                }

                if (modules.Contains("cemu"))
                {
                    Console.WriteLine("[X] Module: Cemu");
                }
                Console.WriteLine("[X] Module: bcml");
            }
            makeJson.settings(modules);
        
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start("cmd", "/c start https://cdn.planethac.me/botw/SetupPart2.html");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", "https://cdn.planethac.me/botw/SetupPart2.html");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", "https://cdn.planethac.me/botw/SetupPart2.html");
            }
            
            Process.Start("cmd", "/c start https://cdn.planethac.me/botw/SetupPart2.html");
        }

        public static String[] ChangeModules()
        {
            int setupmode = 0;
            
            // Module Selection Ints
            int py = 1;
            int cemu = 1;
            int bcml = 1;
            int game = 1;
            
            String[] modules = { "py", "cemu", "bcml", "game" };
            while (setupmode <= 0)
            {
                Console.Clear();
                Console.WriteLine("Select modules by typing thier number (one at a time)");
                Console.WriteLine("Finisch by typing [Y]");
                if (py == 1) Console.WriteLine("1 | [X] Python 3.11");
                else Console.WriteLine("1 | [ ] Python 3.11");
                if (cemu == 1) Console.WriteLine("2 | [X] Cemu");
                else Console.WriteLine("2 | [ ] Cemu");
                if (bcml == 1) Console.WriteLine("3 | [X] Bcml");
                else Console.WriteLine("3 | [ ] Bcml");
                if (game == 1) Console.WriteLine("4 | [X] Game Files");
                else Console.WriteLine("4 | [ ] Game Files");
                Console.Write(": ");
                String arg = Console.ReadLine();
                if (arg.ToLower() == "y") setupmode = 1;
                if (arg == "1") py = flipBitI(py);
                if (arg == "2") cemu = flipBitI(cemu);
                if (arg == "3") bcml = flipBitI(bcml);
                if (arg == "4") game = flipBitI(game);
            }

            int pos = 0;
            String[] rm = { "", "", "", "" };
            if (py == 1)
            {
                rm[pos] = "py";
                pos++;
            }
            if (cemu == 1)
            {
                rm[pos] = "cemu";
                pos++;
            }

            if (bcml == 1)
            {
                rm[pos] = "bcml";
                pos++;
            }
            if (game == 1)
            {
                rm[pos] = "game";
                pos++;
            }
            
            InstallSoftware(rm);
            return rm;
        }

        public static int flipBitI(int bit)
        {
            if (bit == 1)
            {
                return 0;
            }
            else if (bit == 0)
            {
                return 1;
            }

            return 2;
        }
    }
    
    public class makeJson
    {
        public static void settings(String[] modules)
        {
            var Jsettings = new Settings
            {
                installedVer = 1.1,
                py = false,
                cemu = false,
                bcml = false,
                game = false,
            };

            if (modules.Contains("py")) Jsettings.py = true;
            if (modules.Contains("cemu")) Jsettings.cemu = true;
            if (modules.Contains("bcml")) Jsettings.bcml = true;
            if (modules.Contains("game")) Jsettings.game = true;
            
            String fName = "settings.json";
            var options = new JsonSerializerOptions { WriteIndented = true};
            String json = JsonSerializer.Serialize<Settings>(Jsettings, options);
            File.WriteAllText(fName, json);
        }
    }
}