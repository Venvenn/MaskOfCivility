using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Escalon.Unity
{
    /// <summary>
    /// A director should hold the main flow state machine and is the flow states way of accessing unity update methods
    /// </summary>
    public abstract class DirectorUnity : MonoBehaviour, IDirector
    {
        public FlowStateMachine FlowStateMachine { get; set; }
        public NotificationReceiver NotificationReceiver { get; set; }

        public abstract void OnStart();

        public abstract void OnUpdate();

        public abstract void OnFixedUpdate();
        
        public void Start()
        {
            Debug.Init(new DebugLoggerUnity());
            NotificationManager.Instance.SetDirector(this);
            NotificationReceiver = new NotificationReceiver();
            OnStart();
        }

        public void Update()
        {
            OnUpdate();
            FlowStateMachine.Update();
        }

        public void FixedUpdate()
        {
            OnFixedUpdate();
            FlowStateMachine.FixedUpdate();
        }
        
        public PlatformManager GetPlatformManager()
        {
#if UNITY_STEAM
        return new PlatformManagerSteam();
#elif UNITY_MICROSOFT_STORE
        return new PlatformManagerPC();
#elif UNITY_STANDALONE_WIN
            return new PlatformManagerPC();
#else
        Debug.Log("No Implementation of PlatformManager for Target Platform");
        return null;
#endif
        }
        
        public void AddObserver(Type receiverType, IHandlerWrapper handler, string notificationName, object sender = null)
        {
            NotificationReceiver.AddObserver(receiverType, handler, notificationName, sender);
        }

        public void RemoveObserver(Type receiverType, IHandlerWrapper handler, string notificationName, object sender = null)
        {
            NotificationReceiver.RemoveObserver(receiverType, handler, notificationName, sender);
        }

        public void RemoveReceiver(Type receiverType)
        {
            NotificationReceiver.RemoveReceiver(receiverType);
        }

        public async Task PostNotification(string notificationName, object sender, object args)
        {
            List<FlowState> activeStates = new List<FlowState>();
            FlowStateMachine.GetAllActiveChildStates(ref activeStates);

            foreach (FlowState flowState in activeStates)
            {
                await NotificationReceiver.PostNotification(flowState.GetType(), notificationName, sender, args);
            }
        }
    }
}