using System;
using System.Threading.Tasks;

namespace Escalon
{
    /// <summary>
    /// A director should hold the main flow state machine and is the flow states way of accessing unity update methods
    /// </summary>
    public interface IDirector 
    {
        /// <summary>
        /// a Director's root FlowState machine, all states and other machines should stem from this one.
        /// </summary>
        FlowStateMachine FlowStateMachine { get; set; }
        
        /// <summary>
        /// a Director's Notification receiver, used to manage the receiving of messages and the invoking of responses
        /// </summary>
        NotificationReceiver NotificationReceiver { get; set; }
        
        void OnStart();
        void OnUpdate();
        void OnFixedUpdate();
        PlatformManager GetPlatformManager();
        void AddObserver(Type receiverType, IHandlerWrapper handler, string notificationName, Object sender = null);
        void RemoveObserver(Type receiverType, IHandlerWrapper handler, string notificationName, Object sender = null);
        void RemoveReceiver(Type receiverType);
        Task PostNotification(string notificationName, Object sender, Object args);
    }
}