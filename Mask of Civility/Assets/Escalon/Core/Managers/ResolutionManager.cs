using System.Collections.Generic;

namespace Escalon
{
    public abstract class ResolutionManager<T> : IResolutionManager
    {
        public IContainer Container { get; set; }
        public abstract void Init();
        public abstract List<T> GetSupportedResolutions();
        public abstract List<string> GetSupportedResolutionsByString();
        public abstract void SetResolution(T resolution);

        public abstract void SetResolution(string resolution);
    }

    public interface IResolutionManager : IAspect
    {
        void Init();
    }
}