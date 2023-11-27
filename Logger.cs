using System.IO;
namespace botwm;

public class Logger
{
    
    public static void init()
    {
        if (File.Exists("log.txt")) File.Delete("log.txt");
        File.WriteAllText("log.txt", "BOTWM AutoInstall by Planethac\n");
    }
    
    public static void info(String text)
    {
        StreamWriter sw = File.AppendText("log.txt");
        Console.WriteLine(" [INFO] {0}", text);
        sw.WriteLine(" [INFO] " + text);
        sw.Close();
    }
    public static void error(String text)
    {
        StreamWriter sw = File.AppendText("log.txt");
        Console.WriteLine("[ERROR] {0}", text);
        sw.WriteLine("[ERROR] " + text);
        sw.Close();
    }
}