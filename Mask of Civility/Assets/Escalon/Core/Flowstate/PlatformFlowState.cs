
namespace Escalon
{
    /// <summary>
    /// Platform specific logic linked to a State to be used within flow state machine
    /// </summary>
    public abstract partial class FlowState
    {
        /// <summary>
        /// Called when the state is first added to the stack
        /// </summary>
        public virtual void OnStartInitialisePlatform()
        {
        }

        /// <summary>
        /// called every tick until TransitionState.COMPLETED is returned, things like loading and transitions can be done here 
        /// </summary>
        public virtual TransitionState UpdateInitialisePlatform()
        {
            return TransitionState.Completed;
        }

        /// <summary>
        /// called when the initialising has been completed
        /// </summary>
        public virtual void OnFinishInitialisePlatform()
        {
        }

        /// <summary>
        /// called either when the full initialising process has been completed or this state becomes active again after being inactive 
        /// </summary>
        public virtual void OnActivePlatform()
        {
        }

        /// <summary>
        /// called when another state is pushed onto the stack and this state becomes no longer active
        /// </summary>
        public virtual void OnInactivePlatform()
        {
        }

        /// <summary>
        /// called every tick when this state is active
        /// </summary>
        public virtual void ActiveUpdatePlatform()
        {
        }

        /// <summary>
        /// called when this state is popped 
        /// </summary>
        public virtual void OnStartDismissPlatform()
        {
        }

        /// <summary>
        /// called every tick while in the dismissing stage until TransitionState.COMPLETED is returned 
        /// </summary>
        /// <returns></returns>
        public virtual TransitionState UpdateDismissPlatform()
        {
            return TransitionState.Completed;
        }

        /// <summary>
        /// called when the dismissing of the state has been completed
        /// </summary>
        public virtual void OnFinishDismissPlatform()
        {
        }
    }
}
