using System.Threading.Tasks;

namespace Escalon
{
    /// <summary>
    /// Used to represent a view for something in the game world.
    /// </summary>
    public interface IView
    {
        Task Init(FlowState flowState, CoreManagers coreManagers);
        void Present();
        void UpdateView();
        void OnActive();
        void OnInactive();
        void Dismiss();
        bool IsTransitioning();
        void DisposeView();
    }
}

