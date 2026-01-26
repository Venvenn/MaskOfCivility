using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Escalon;
using UnityEngine;

public class PlatformManagerPC : PlatformManager
{
    private static string s_localSaveDirectory = $"{Application.persistentDataPath}/SaveFiles";
    
    protected Process _oskProcess;
    
    public override void Init()
    {
    }

    public override async Task SaveAsync(string json, string slotName, string fileName, string extension = "json")
    {
        string directoryPath = $"{s_localSaveDirectory}/{slotName}";

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        await File.WriteAllTextAsync($"{s_localSaveDirectory}/{slotName}/{fileName}.json", json);
    }

    public override async Task<string> LoadAsync(string slotName, string fileName, string extension = "json")
    {
        return await File.ReadAllTextAsync($"{s_localSaveDirectory}/{slotName}/{fileName}.json");
    }

    public override void Save(string json, string slotName, string fileName, string extension = "json")
    {
        string directoryPath = $"{s_localSaveDirectory}/{slotName}";
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    
        File.WriteAllText($"{directoryPath}/{fileName}.json", json);
    }
    
    public override string Load(string slotName, string fileName, string extension = "json")
    {
        return File.ReadAllText($"{s_localSaveDirectory}/{slotName}/{fileName}.json");
    }

    public override bool Delete(string slotName)
    {
        string directoryPath = $"{s_localSaveDirectory}/{slotName}";
        if (Directory.Exists(directoryPath))
        {
            Directory.Delete(directoryPath, true);
            return true;
        }

        return false;
    }

    public override bool CheckSaveExists(string slotName)
    {
        bool exists = Directory.Exists($"{s_localSaveDirectory}/{slotName}");
        return exists;
    }

    public override string GetSaveDirectoryPath()
    {
        return $"{s_localSaveDirectory}";
    }
    
    protected virtual void ShowOnScreenTextInput(object sender, object args)
    {
        _oskProcess = Process.Start("osk.exe");
    }
    
    protected virtual void CloseOnScreenTextInput(object sender, object args)
    {
        if (_oskProcess != null)
        {
            _oskProcess.Kill();
            _oskProcess = null;
        }
    }
}
