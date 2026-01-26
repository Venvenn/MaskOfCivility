using System.IO;
using UnityEditor;
using UnityEngine;

namespace Escalon
{
    public static class SaveUtils
    {
        [MenuItem("Utils/Clear Save Data")]
        public static void ClearSaveData()
        {
            Directory.Delete($"{Application.persistentDataPath}/SaveFiles/", true);
        }
    
        [MenuItem("Utils/Recompile Scripts")]
        public static void RecompileScripts()
        {
            UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
        }
    
        [MenuItem("Utils/Refresh Asset Database")]
        public static void RefreshAssetDatabase()
        {
            AssetDatabase.Refresh();
        }
    }
}

