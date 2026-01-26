using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public static class LocalBuildPipeline
{
    private enum BuildPlatform
    {
        Windows,
        Steam
    }
    
    private enum BuildType
    {
        Production,
        Demo,
        Playtest
    }
    
#region EditorSymbols
    [MenuItem("Build/SetEditorSymbols/ReleaseType/Demo")]
    public static void SetDemo()
    {
        SetBuildType(BuildType.Demo, NamedBuildTarget.Standalone);
        ApplyLocalDefines("DEMO");
    }
    [MenuItem("Build/SetEditorSymbols/ReleaseType/Playtest")]
    public static void SetPlaytest()
    {
        SetBuildType(BuildType.Playtest, NamedBuildTarget.Standalone);
        ApplyLocalDefines("PLAYTEST");
    }
    [MenuItem("Build/SetEditorSymbols/ReleaseType/Production")]
    public static void SetProduction()
    {
        SetBuildType(BuildType.Production, NamedBuildTarget.Standalone);
        ApplyLocalDefines("PRODUCTION");
    }
    
#if UNITY_STEAM
    [MenuItem("Build/SetEditorSymbols/Platform/Steam")]
    public static void SetSteam()
    {
        EditScriptingDefines("UNITY_STEAM", true, NamedBuildTarget.Standalone);
        ApplyLocalDefines();
    }
#endif
    
    [MenuItem("Build/SetEditorSymbols/Platform/Standalone")]
    public static void SetStandalone()
    {
        EditScriptingDefines("UNITY_STEAM", false, NamedBuildTarget.Standalone);
        ApplyLocalDefines();
    }
#endregion 
    
#region Platforms
    [MenuItem("Build/Local/Windows/Release/Production")]
    public static void BuildWindowsRelease()
    {
        BuildOptions options = SetDevelopment(false);
        SetBuildType(BuildType.Production, NamedBuildTarget.Standalone);
        Build(BuildPlatform.Windows, options);
    }
    [MenuItem("Build/Local/Windows/Release/Demo")]
    public static void BuildWindowsDemoRelease()
    {
        BuildOptions options = SetDevelopment(false);
        SetBuildType(BuildType.Demo, NamedBuildTarget.Standalone);
        Build(BuildPlatform.Windows, options);
    }
    
    [MenuItem("Build/Local/Windows/Development/Production")]
    public static void BuildWindowsDevelopment()
    {
        BuildOptions options = SetDevelopment(true);
        SetBuildType(BuildType.Production, NamedBuildTarget.Standalone);
        Build(BuildPlatform.Windows, options);
    }
    [MenuItem("Build/Local/Windows/Development/Demo")]
    public static void BuildWindowsDemoDevelopment()
    {
        BuildOptions options = SetDevelopment(true);
        SetBuildType(BuildType.Demo, NamedBuildTarget.Standalone);
        Build(BuildPlatform.Windows, options);
    }
    
    [MenuItem("Build/Local/Steam/Release/Production")]
    public static void BuildSteamRelease()
    {
        BuildOptions options = SetDevelopment(false);
        SetBuildType(BuildType.Production, NamedBuildTarget.Standalone);
        Build(BuildPlatform.Steam, options);
    }
    [MenuItem("Build/Local/Steam/Release/Demo")]
    public static void BuildSteamDemoRelease()
    {
        BuildOptions options = SetDevelopment(false);
        SetBuildType(BuildType.Demo, NamedBuildTarget.Standalone);
        Build(BuildPlatform.Steam, options);
    }
    [MenuItem("Build/Local/Steam/Release/Playtest")]
    public static void BuildSteamPlaytestRelease()
    {
        BuildOptions options = SetDevelopment(false);
        SetBuildType(BuildType.Playtest, NamedBuildTarget.Standalone);
        Build(BuildPlatform.Steam, options);
    }
    
    [MenuItem("Build/Local/Steam/Development/Production")]
    public static void BuildSteamDevelopment()
    {
        BuildOptions options = SetDevelopment(true);
        SetBuildType(BuildType.Production, NamedBuildTarget.Standalone);
        Build(BuildPlatform.Steam, options);
    }
    [MenuItem("Build/Local/Steam/Development/Demo")]
    public static void BuildSteamDemoDevelopment()
    {
        BuildOptions options = SetDevelopment(true);
        SetBuildType(BuildType.Demo, NamedBuildTarget.Standalone);
        Build(BuildPlatform.Steam, options);
    }
    [MenuItem("Build/Local/Steam/Development/Playtest")]
    public static void BuildSteamPlaytestDevelopment()
    {
        BuildOptions options = SetDevelopment(true);
        SetBuildType(BuildType.Playtest, NamedBuildTarget.Standalone);
        Build(BuildPlatform.Steam, options);
    }
#endregion   

    private static void Build(BuildPlatform platform, BuildOptions options)
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).ToArray();

        switch (platform)
        {
            case BuildPlatform.Windows:
                EditScriptingDefines("UNITY_STEAM", false, NamedBuildTarget.Standalone);
                BuildWindows(scenes, options);
                break;
            case BuildPlatform.Steam:
                BuildSteam(scenes, options);
                break;
        }
    }
    
    private static void BuildSteam(EditorBuildSettingsScene[] scenes, BuildOptions buildOptions)
    {
        EditScriptingDefines("UNITY_STEAM", true, NamedBuildTarget.Standalone);
        BuildWindows(scenes, buildOptions);
    }
    
    private static void BuildWindows(EditorBuildSettingsScene[] scenes, BuildOptions buildOptions)
    {
        Debug.Log("Starting Windows Build!");
        BuildPipeline.BuildPlayer(scenes, $"Build/Windows/{Application.productName}.exe", BuildTarget.StandaloneWindows64, buildOptions);
        
    }

    private static void SetBuildType(BuildType buildType, NamedBuildTarget buildTarget)
    {
        switch (buildType)
        {
            case BuildType.Production:
                EditScriptingDefines("DEMO", false, buildTarget);
                EditScriptingDefines("PLAYTEST", false, buildTarget);
                break;
            case BuildType.Demo:
                EditScriptingDefines("DEMO", true, buildTarget);
                EditScriptingDefines("PLAYTEST", false, buildTarget);
                break;
            case BuildType.Playtest:
                EditScriptingDefines("DEMO", false, buildTarget);
                EditScriptingDefines("PLAYTEST", true, buildTarget);
                break;
        }
    }

    private static void ApplyLocalDefines(string appType = "")
    {
        
#if UNITY_STEAM
        string steamAppId;

        if (string.IsNullOrEmpty(appType))
        {
            steamAppId = SteamId.s_appId.ToString();
        }
        else
        {
            switch (appType)
            {
                case "DEMO":
                {
                    steamAppId = SteamId.s_demoId.ToString();
                    break;
                }
                default:
                {
                    steamAppId = SteamId.s_productionId.ToString();
                    break;
                }
            }
        }
        
        File.WriteAllText("steam_appId.txt", steamAppId);
#endif

        var defines = PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.Standalone);
        Debug.Log($"Define Symbols Set: {defines}");
    }

    private static void EditScriptingDefines(string symbol, bool add, NamedBuildTarget buildTarget)
    {
        var defines = PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.Standalone);
        if (add && !defines.Contains(symbol))
        {
            defines += $";{symbol}";
            PlayerSettings.SetScriptingDefineSymbols(buildTarget, defines);
        }
        else if (!add && defines.Contains(symbol))
        {
            defines = defines.Replace($";{symbol}", string.Empty);
            PlayerSettings.SetScriptingDefineSymbols(buildTarget, defines);
        }
    }
    
    private static BuildOptions SetDevelopment(bool development)
    {
        return development ? BuildOptions.Development | BuildOptions.EnableDeepProfilingSupport : BuildOptions.None;
    }
}
