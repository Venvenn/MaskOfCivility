using System.Threading.Tasks;

namespace Escalon
{
    /// <summary>
    /// Manager in charge of application and engine specific functionality
    /// </summary>
    public abstract class ApplicationManager : Aspect
    {
        public abstract void Init();
        public abstract float GetDeltaTime();
        public abstract float GetTimeSinceStartup();
        public abstract void AddViews();
        public abstract void Quit();
        public abstract void Pause(bool pause);
        public abstract string GetDataPath(string additionalPath = "");
        public abstract Task LoadAssets();
        public abstract void AddOnQuitAction(System.Action onQuit);
    }
}
