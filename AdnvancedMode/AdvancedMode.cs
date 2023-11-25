using System.Diagnostics;
using System.Net;

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
    }

    public static void MakeDummy()
    {
        String[] mod = { "py", "game", "cemu", "bcml" };
        var dummy = new AdvancedSetup()
        {
            version = 1.0,
            modules = mod,
            pyVnum = "3.11",
            cemuVnum = 1.26,
            useCoustumCemu = false,
            pyCoustumUri = "NONE",
            useCoustumPy = false,
            cemuCoustumUri = "NONE",
            useCoustumModFiles = false,
            modFileUri = "NONE",
            autoDumpGameFiles = false,
            gameFileUri = "NONE"
        };
        JsonSR.SRAdvancedSetup(dummy);
    }

    public static void LoadFile(string filepath)
    {
        if (filepath == null) Console.WriteLine("Cannt be null");
        try
        {
            FileInfo file = new FileInfo(filepath);
            AdvancedSetup setup = JsonSR.DSAdvancedSetup(file);

            if (setup.modules.Contains("py"))
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
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}