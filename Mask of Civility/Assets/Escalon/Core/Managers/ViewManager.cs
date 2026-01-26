using System;
using System.Collections.Generic;

namespace Escalon
{
    /// <summary>
    /// Responsible for the visualisation of the game 
    /// </summary>
    public class ViewManager : Aspect, IDisposable
    {
        protected readonly Dictionary<string, IView> _views = new Dictionary<string, IView>();
        protected readonly List<string> _initialising = new List<string>();

        public virtual void AddView(string viewId, IView view)
        {
            _views.Add(viewId, view);
        }
        
        public virtual void RemoveView(string viewId)
        {
            if (CheckValid(viewId))
            {
                _views[viewId].DisposeView();
                _views.Remove(viewId);
            }
        }

        public virtual async void PresentView(FlowState flowState)
        {
            string viewId = flowState.StateId;
            if (CheckValid(viewId))
            {
                IView view = _views[viewId];
                CoreManagers coreManagers = Container.GetAspect<CoreManagers>();
                
                _initialising.Add(viewId);
                await view.Init(flowState, coreManagers);
                _initialising.Remove(viewId);
                view.Present();
            }
        }

        public virtual void ActivateView(string viewId)
        {
            if (CheckValid(viewId))
            {
                _views[viewId].OnActive();
            }
        }
        
        public virtual void DeactivateView(string viewId)
        {
            if (CheckValid(viewId))
            {
                _views[viewId].OnInactive();
            }
        }

        public virtual void UpdateView(string viewId)
        {
            if (CheckValid(viewId))
            {
                _views[viewId].UpdateView();
            }
        }
        
        public virtual void DismissView(string viewId)
        {
            if (CheckValid(viewId))
            {
                _views[viewId].Dismiss();
            }
        }
    
        public virtual void DisposeView(string viewId)
        {
            if (CheckValid(viewId))
            {
                _views[viewId].DisposeView();
            }
        }
    
        public virtual bool IsViewTransitioning(string viewId)
        {
            if (CheckValid(viewId))
            {
                return _views[viewId].IsTransitioning();
            }
            return false;
        }
        
        public virtual bool IsViewInitialising(string viewId)
        {
            if (CheckValid(viewId))
            {
                return _initialising.Contains(viewId);
            }
            return false;
        }
    
        protected bool CheckValid(string viewId)
        {
            return _views.ContainsKey(viewId);;
        }

        public void Dispose()
        {
        }
    }
}
