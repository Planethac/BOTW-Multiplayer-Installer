using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.IO;

namespace botwm.AdnvancedMode;

public class AdvancedMode
{
    public static void Init()
    {
        Console.Clear();
        Console.WriteLine("Advnaved Mode");
        Console.WriteLine("Type [D] to create a dummy file (will be stored as autorun.json)");
        Console.WriteLine("Type [C] to create a coustum auto install file");
        Console.WriteLine("Type [L] to load an autorun file");
        Console.Write(": ");
        String option = Console.ReadLine();
        if (option.ToLower() == "d") MakeDummy();
        if(option.ToLower() == "l")
        {
            Console.WriteLine("Enter filename / path");
            Console.WriteLine(": ");
            String fp = Console.ReadLine();
            LoadFile(fp);
        }
        if (option.ToLower() == "c") CreateCoustum();
    }

    public static void CreateCoustum()
    {
        var csData = new AdvancedSetup()
        {
            version = 1.2,
            pyVnum = "3.11",
            cemuVnum = "1.26.2f"
        };
        string rl = "";
        
        Console.Write("List modules to install (space separated): ");
        rl = Console.ReadLine();
        csData.modules = rl.Split(" ");
        Console.WriteLine();
        Console.Write("Use coustum Python? (y/n): ");
        rl = Console.ReadLine();
        if (rl.ToLower() == "y")
        {
            csData.useCoustumPy = true;
            Console.WriteLine();
            Console.Write("Enter download link: ");
            csData.pyCoustumUri = Console.ReadLine();
        }
        Console.Write("Use coustum Cemu? (y/n): ");
        rl = Console.ReadLine();
        if (rl.ToLower() == "y")
        {
            csData.useCoustumCemu = true;
            Console.WriteLine();
            Console.Write("Enter download link: ");
            csData.cemuCoustumUri = Console.ReadLine();
        }
        Console.Write("Use coustum Mod Files? (y/n): ");
        rl = Console.ReadLine();
        if (rl.ToLower() == "y")
        {
            csData.useCoustumModFiles = true;
            Console.WriteLine();
            Console.Write("Enter download link: ");
            csData.modFileUri = Console.ReadLine();
        }
        Console.Write("Auto dump game files? (y/n): ");
        rl = Console.ReadLine();
        if (rl.ToLower() == "y")
        {
            csData.autoDumpGameFiles = true;
            Console.WriteLine();
            Console.Write("Enter download link: ");
            csData.gameFileUri = Console.ReadLine();
        }
        
        JsonSR.SRAdvancedSetup(csData, "cstm");
    }

    public static void MakeDummy()
    {
        String[] mod = { "py", "game", "cemu", "bcml", "fs" };
        var dummy = new AdvancedSetup()
        {
            version = 1.2,
            modules = mod,
            pyVnum = "3.11",
            cemuVnum = "1.26.2f",
            useCoustumCemu = false,
            pyCoustumUri = "NONE",
            useCoustumPy = false,
            cemuCoustumUri = "NONE",
            useCoustumModFiles = false,
            modFileUri = "NONE",
            autoDumpGameFiles = false,
            gameFileUri = "NONE"
        };
        JsonSR.SRAdvancedSetup(dummy, "dummy");
    }

    public static void LoadFile(string filepath)
    {
        if (filepath == null) Console.WriteLine("Cannt be null");
        try
        {
            FileInfo fs = new FileInfo(filepath);
            AdvancedSetup setup = JsonSR.DSAdvancedSetup(fs);

            if (setup.modules.Contains("py"))
            {
                if (setup.useCoustumPy)
                {
                    Logger.info("Using custom py");
                    Logger.info("Downloading custom Python from " + setup.pyCoustumUri);
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(setup.pyCoustumUri, "python3.exe");
                    Logger.info("Running Python Setup");
                    Process py = new Process();
                    py.StartInfo.FileName = @"python3.exe";
                    py.Start();
                    py.WaitForExit();
                    File.Delete("python3.exe");
                    Logger.info("Python Successfully Installed!");
                }
                else
                {
                    String[] supportedPyVersions = { "3.11", "3.10" };
                    string[] downloadUrl = { "https://www.python.org/ftp/python/3.11.2/python-3.11.2-amd64.exe", "https://www.python.org/ftp/python/3.10.9/python-3.10.9-amd64.exe" };
                    if (supportedPyVersions.Contains(setup.pyVnum))
                    {
                        int pyDwn = Array.IndexOf(supportedPyVersions, setup.pyVnum);
                        Logger.info("Downloading Python " + (setup.pyVnum));
                        WebClient webClient = new WebClient();
                        webClient.DownloadFile(downloadUrl[pyDwn], "python3.exe");
                        Logger.info("Running Python Installer");
                        Process py = new Process();
                        py.StartInfo.FileName = @"python3.exe";
                        py.Start();
                        py.WaitForExit();
                        File.Delete("python3.exe");
                        Logger.info("Python " + (setup.pyVnum) +" Successfully installed!");
                    }
                    else
                    {
                        Logger.error("Unsupported Python Version");
                    }   
                }
            }

            if (setup.modules.Contains("cemu"))
            {
                if (setup.useCoustumCemu)
                {
                    Logger.info("Using custom Cemu");
                    Logger.info("Download custom cemu from " + setup.cemuCoustumUri);
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(setup.cemuCoustumUri, "cemu.zip");
                    Logger.info("Extracting cemu");
                    try
                    {
                        ZipFile.ExtractToDirectory("cemu.zip", @"BOTW-COOP\cemu");
                    }
                    catch (Exception e)
                    {
                        
                    }
                    Logger.info("Cemu successfully installed!");
                }
                else
                {
                    String[] supportedCemuVersion = { "1.26.2f" };
                    String[] downloadUrl = { "https://cemu.info/releases/cemu_1.26.2.zip" };
                    if (supportedCemuVersion.Contains(setup.cemuVnum))
                    {
                        int cemuDwn = Array.IndexOf(supportedCemuVersion, setup.cemuVnum);
                        if (!(Directory.Exists(@"BOTW-COOP\cemu"))) Directory.CreateDirectory(@"BOTW-COOP\cemu");
                        Logger.info("Downloading Cemu Version " + setup.cemuVnum);
                        WebClient webClient = new WebClient();
                        webClient.DownloadFile(downloadUrl[cemuDwn], "cemu.zip");
                        Logger.info("Extracting Cemu " + setup.cemuVnum);
                        try
                        {
                            ZipFile.ExtractToDirectory("cemu.zip", @"BOTW-COOP\cemu");
                        }
                        catch (Exception e)
                        {
                        
                        }
                        Logger.info("Cemu " + (setup.cemuVnum) +" Successfully installed!");
                        File.Delete("cemu.zip");
                    }
                    else
                    {
                        Logger.error("Unsupported Cemu Version");
                    }   
                }
            }

            if (setup.modules.Contains("bcml"))
            {
                Logger.info("Installing BCML");
                Process bcml = new Process();
                bcml.StartInfo.FileName = "Scripts\\BCML.exe";
                bcml.StartInfo.RedirectStandardOutput = false;
                bcml.Start();
                bcml.WaitForExit();
                Logger.info("BCML Successfully installed!");
            }

            if (setup.modules.Contains("game")) 
            {
                if (setup.useCoustumModFiles)
                {
                    Logger.info("Using Coustum Mod Files");
                    Logger.info("Downloading mod files from " + setup.modFileUri);
                    WebClient webClient = new WebClient();
                    Directory.CreateDirectory("BOTW-COOP");
                    Directory.CreateDirectory(@"BOTW-COOP\tmp");
                    webClient.DownloadFile(setup.modFileUri, @"BOTW-COOP\tmp\mod-data.zip");
                    Logger.info("Uncopressing Files");
                    try
                    {
                        ZipFile.ExtractToDirectory(@"BOTW-COOP\tmp\mod-data.zip", "BOTW-COOP");
                    }
                    catch(Exception ex)
                    {

                    }
                    Logger.info("Reorcanizing Files");
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
                    Logger.info("Installed Mod files");
                    Thread.Sleep(1000);   
                }
                else
                {
                    Logger.info("Installing mod files");
                    Logger.info("Downloading Files");
                    WebClient webClient = new WebClient();
                    Directory.CreateDirectory("BOTW-COOP");
                    Directory.CreateDirectory(@"BOTW-COOP\tmp");
                    webClient.DownloadFile("https://cdn.discordapp.com/attachments/1113599857630908540/1124419427400683581/BOTW.multiplayer_1.0.4_files__setup_tutorial.zip", @"BOTW-COOP\tmp\mod-data.zip");
                    Logger.info("Uncopressing Files");
                    try
                    {
                        ZipFile.ExtractToDirectory(@"BOTW-COOP\tmp\mod-data.zip", "BOTW-COOP");
                    }
                    catch(Exception ex)
                    {

                    }
                    Logger.info("Reorcanizing Files");
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
                    Logger.info("Installed Mod files");
                    Thread.Sleep(1000);   
                }
            }

            if (setup.modules.Contains("fs"))
            {
                
                // IMPORTANT
                // the zip file should contain the folowing folders:
                // 000500c
                // 000500e
                // 0005000
                
                if (setup.autoDumpGameFiles)
                {
                    Logger.info("Downloading game data");
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(setup.gameFileUri, "gamedata.zip");
                    try
                    {
                        ZipFile.ExtractToDirectory("gamedata.zip", @"BOTW-COOP\cemu\mlc01\usr\title");
                    }
                    catch (Exception e)
                    {
                        
                    }
                    
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}