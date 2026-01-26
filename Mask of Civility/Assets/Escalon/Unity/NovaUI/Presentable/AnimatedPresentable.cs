using System.Collections.Generic;
using Nova;
using UnityEngine;

namespace Escalon.Nova
{
    public class AnimatedPresentable : Presentable
    {
        protected AnimationHandle _animationHandle;
        
        [SerializeField]
        private List<IAnimation> _presentAnimations;
        [SerializeField]
        private List<IAnimation> _dismissAnimations;

        private void Animate(List<IAnimation> animations, ref AnimationHandle handle)
        {
            //AnimationChainType nextAnimationChainType = AnimationChainType.Sequential;
        
            // foreach (IAnimation animation in animations)
            // {
            //     // AnimationSystem.AddAnimation(animation, );
            //     // nextAnimationChainType = animation.AnimationAnimationChainType;
            // }
        }

        public override void Present(ref AnimationHandle animationHandle)
        {
            Animate(_presentAnimations, ref _animationHandle);
        }

        public override void Dismiss(ref AnimationHandle animationHandle)
        {
            Animate(_dismissAnimations, ref _animationHandle);
        }

        public override bool IsTransitioning()
        {
            return !_animationHandle.IsComplete();
        }
    }
}


