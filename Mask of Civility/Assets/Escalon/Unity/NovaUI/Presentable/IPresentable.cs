
using Nova;

namespace Escalon.Nova
{
    public interface IPresentable
    {
        void Present(ref AnimationHandle animationHandle);
        void Dismiss(ref AnimationHandle animationHandle);
    }
}