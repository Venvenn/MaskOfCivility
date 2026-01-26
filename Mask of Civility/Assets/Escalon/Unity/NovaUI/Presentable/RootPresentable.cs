using System;
using Nova;
using UnityEngine;

namespace Escalon.Nova
{
    public abstract class RootPresentable : AnimatedPresentable
    {
        [SerializeField] protected ScreenSpace _screenSpace;

        protected Presentable[] _presentables;

        public void Start()
        {
            SetCamera(Camera.main);
        }

        public void SetCamera(Camera camera)
        {
            if (_screenSpace != null)
            {
                _screenSpace.TargetCamera = camera;
            }
        }

        protected void FindPresentables()
        {
            _presentables = transform.GetComponentsInChildren<Presentable>(true);
        }

        public void Present()
        {
            if (_presentables != null)
            {
                foreach (Presentable presentable in _presentables)
                {
                    if (presentable != this)
                    {
                        presentable.Present(ref _animationHandle);
                    }
                }

            }
        }

        public void Dismiss()
        {
            if (_presentables != null)
            {
                foreach (Presentable presentable in _presentables)
                {
                    if (presentable != this)
                    {
                        presentable.Dismiss(ref _animationHandle);
                    }
                }
            }
        }
    }
}