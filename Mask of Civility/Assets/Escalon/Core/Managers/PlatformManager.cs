
using System.Threading.Tasks;

namespace Escalon
{
    /// <summary>
    /// Responsible for platform specific functionality and requirements
    /// </summary>
    public abstract class PlatformManager : Aspect
    {
        public const string k_openOSKeyboard = "PlatformManager.OpenKeybaord";
        public const string k_closeOSKeyboard = "PlatformManager.CloseKeybaord";
        
        public abstract void Init();
        public abstract Task SaveAsync(string json, string slotName, string fileName, string extension = "json");
        public abstract Task<string> LoadAsync(string slotName, string fileName, string extension = "json");
        public abstract void Save(string json, string slotName, string fileName, string extension = "json");
        public abstract string Load(string slotName, string fileName, string extension = "json");
        public abstract bool Delete(string slotName);
        public abstract bool CheckSaveExists(string slotName);
        public abstract string GetSaveDirectoryPath();
        
        
    }
}
