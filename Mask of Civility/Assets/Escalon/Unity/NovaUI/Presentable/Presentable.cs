using Nova;
using UnityEngine;

namespace Escalon.Nova
{
    public abstract class Presentable : MonoBehaviour, IPresentable
    {
        public abstract void Present(ref AnimationHandle animationHandle);
        public abstract void Dismiss(ref AnimationHandle animationHandle);
        public abstract bool IsTransitioning();
    }
}