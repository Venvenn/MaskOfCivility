using System;

namespace Escalon
{
    /// <summary>
    /// State to be used within flow state machine, should contain contiguous game logic  
    /// </summary>
    [Serializable]
    public abstract partial class FlowState
    {
        public enum StateStage
        {
            Presenting,
            Active,
            Inactive,
            Dismissing,
        }

        public enum TransitionState
        {
            InProgress,
            Completed
        }

        /// <summary>
        /// the flow state machine that owns this state
        /// </summary>
        public FlowStateMachine FlowStateMachine = null;
        
        /// <summary>
        /// The current stage this state is on
        /// </summary>
        public StateStage Stage = StateStage.Inactive;

        public string StateId => GetType().Name;

        /// <summary>
        /// the flow state machine that owns this state
        /// </summary>
        public Container Container => FlowStateMachine.Container;

        /// <summary>
        /// View that represents this state
        /// </summary>
        public IView View { get; private set; }

        /// <summary>
        /// Called when the state is first added to the stack
        /// </summary>
        public virtual void OnStartInitialise()
        {
        }

        /// <summary>
        /// called every tick until TransitionState.COMPLETED is returned, things like loading and transitions can be done here 
        /// </summary>
        public virtual TransitionState UpdateInitialise()
        {
            return TransitionState.Completed;
        }

        /// <summary>
        /// called when the initialising has been completed
        /// </summary>
        public virtual void OnFinishInitialise()
        {
        }

        /// <summary>
        /// called either when the full initialising process has been completed or this state becomes active again after being inactive 
        /// </summary>
        public virtual void OnActive()
        {
            this.PostNotification($"Notification.{GetType()}.Active");
        }

        /// <summary>
        /// called when another state is pushed onto the stack and this state becomes no longer active
        /// </summary>
        public virtual void OnInactive()
        {
            this.PostNotification($"Notification.{GetType()}.Inactive");
        }

        /// <summary>
        /// called every tick when this state is active
        /// </summary>
        public virtual void ActiveUpdate()
        {
        }

        /// <summary>
        /// called at unity's fixed time step which by default is 0.02 seconds, should be used for physics 
        /// </summary>
        public virtual void ActiveFixedUpdate()
        {
        }

        /// <summary>
        /// called when this state is popped 
        /// </summary>
        public virtual void OnStartDismiss()
        {
        }

        /// <summary>
        /// called every tick while in the dismissing stage until TransitionState.COMPLETED is returned 
        /// </summary>
        /// <returns></returns>
        public virtual TransitionState UpdateDismiss()
        {
            return TransitionState.Completed;
        }

        /// <summary>
        /// called when the dismissing of the state has been completed
        /// </summary>
        public virtual void OnFinishDismiss()
        {
        }

        /// <summary>
        /// Sets this states view that visualises the state
        /// </summary>
        public void SetView(IView view)
        {
            View = view;
        }

        public static string GetActiveNotification<T>() where T : FlowState
        {
            return $"Notification.{typeof(T)}.Active";
        }
        
        public static string GetInactiveNotification<T>() where T : FlowState
        {
            return $"Notification.{typeof(T)}.Inactive";
        }
    }
}
