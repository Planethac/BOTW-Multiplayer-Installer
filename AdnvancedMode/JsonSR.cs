using System.Text.Json;

namespace botwm.AdnvancedMode;

public class AdvancedSetup
{
    public double version { get; set; }
    public String[] modules { get; set; }
    public String pyVnum { get; set; }
    public String cemuVnum { get; set; }
    public bool useCoustumPy { get; set; }
    public String pyCoustumUri { get; set; }
    public bool useCoustumCemu { get; set; }
    public String cemuCoustumUri { get; set; }
    public bool useCoustumModFiles { get; set; }
    public String modFileUri { get; set; }
    public bool autoDumpGameFiles { get; set; }
    public String gameFileUri { get; set; }
}

public class JsonSR
{
    public static void SRAdvancedSetup(AdvancedSetup setup)
    {
        var options = new JsonSerializerOptions { WriteIndented = true};
        string SerializedJson = JsonSerializer.Serialize<AdvancedSetup>(setup, options);
        File.WriteAllText("autorun.json", SerializedJson);
    }

    public static AdvancedSetup DSAdvancedSetup(FileInfo fileInfo)
    {
        String SearializedJson = File.ReadAllText(fileInfo.FullName);
        AdvancedSetup setup = JsonSerializer.Deserialize<AdvancedSetup>(SearializedJson);
        return setup;
    }
}