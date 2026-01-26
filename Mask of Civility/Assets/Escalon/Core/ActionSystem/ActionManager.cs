using System.Collections.Generic;
using System.Threading.Tasks;
using Arch.Core;
using Arch.Core.Extensions;

namespace Escalon.ActionSystem
{
    /// <summary>
    /// Manager responsible for handling the processing of actions
    /// </summary>
    public class ActionManager : BaseManager
    {
        public const string k_eventExecuted = "Notification.ActionPerformed";
        public const string k_eventRun = "Notification.EventStarted";
        
        /// <summary>
        /// System for processing events, sequences and actions
        /// </summary>
        private readonly ActionProcessor _actionProcessor = new ActionProcessor();
        
        /// <summary>
        /// System for viewing actions
        /// </summary>
        private readonly ActionViewer _actionViewer;
        
        public ActionSystemState ActionSystemState => _actionProcessor.ActionSystemState;
        public bool ProcessorRunning => (_actionProcessor.ActionSystemState & ActionSystemState.Active) != 0;
        public bool ViewerRunning => _actionViewer.Running;
        public bool Running => ProcessorRunning || ViewerRunning;

        public ActionManager(DataManager dataManager, FlowState scope) 
        {
            _actionViewer = new ActionViewer(dataManager, scope);
        }
        
        public void AddSequence<T>(T eventTrigger, Sequence actionSequence, Entity source, ActionExecutionPhase executionPhase = ActionExecutionPhase.Execute, int useCount = -1) where T : IEventTrigger
        {
            _actionProcessor.AddSequence(eventTrigger, actionSequence, source, executionPhase);
        }
        
        public void AddSequence<T>(Sequence actionSequence, Entity source, ActionExecutionPhase executionPhase = ActionExecutionPhase.Execute, int useCount = -1) where T : IEventTrigger
        {
            AddSequence<T>(default, actionSequence, source, executionPhase);
        }

        public void AddSequence<T>(T eventTrigger, Sequence actionSequence, ActionExecutionPhase executionPhase = ActionExecutionPhase.Execute, int useCount = -1) where T : IEventTrigger
        {
            AddSequence(eventTrigger, actionSequence, Entity.Null, executionPhase);
        }

        public void AddSequence<T>(Sequence actionSequence, ActionExecutionPhase executionPhase = ActionExecutionPhase.Execute, int useCount = -1) where T : IEventTrigger
        {
            AddSequence<T>(default, actionSequence, Entity.Null, executionPhase);
        }
        
        public void RemoveSequence<T>(Sequence actionSequence) where T : IEventTrigger
        {
            RemoveSequence<T>(default, actionSequence);
        }

        public void RemoveSequence<T>(T eventTrigger, Sequence actionSequence) where T : IEventTrigger
        {
            _actionProcessor.RemoveSequence(eventTrigger, actionSequence);
        }

        public void RemoveAllSequence<T>(T eventTrigger, Sequence actionSequence) where T : IEventTrigger
        {
            _actionProcessor.RemoveAllSequence(eventTrigger, actionSequence);
        }

        public void RemoveAllSequences(Entity source)
        {
            _actionProcessor.RemoveAllSequences(source);
        }

        public List<Sequence> GetSequences<T>(Entity entity, ActionExecutionPhase phase) where T : IEventTrigger
        {
            return GetSequences<T>(default, entity, phase);
        }
        
        public List<Sequence> GetSequences<T>(T eventTrigger, Entity entity, ActionExecutionPhase phase) where T : IEventTrigger
        {
            return _actionProcessor.GetSequences(eventTrigger, entity, phase);
        }
        
        public List<Sequence> GetSequences<T>(T eventTrigger, Entity entity) where T : IEventTrigger
        {
            return _actionProcessor.GetAllEntitySequences(eventTrigger, entity);
        }
        
        public void TriggerEvent<T>(Entity sourceEntity) where T : IEventTrigger
        {
            _actionProcessor.QueueEvent<T>(sourceEntity);
        }
        
        public void TriggerEvent<T>() where T : IEventTrigger
        {
            _actionProcessor.QueueEvent<T>(Entity.Null);
        }
        
        public void TriggerEvent<T>(T eventTrigger, Entity sourceEntity) where T : IEventTrigger
        {
            _actionProcessor.QueueEvent(eventTrigger, sourceEntity);
        }
        
        public void TriggerEvent<T>(T eventTrigger) where T : IEventTrigger
        {
            _actionProcessor.QueueEvent(eventTrigger, Entity.Null);
        }

        public void TriggerCrashEvent<T>(T eventTrigger, Entity sourceEntity) where T : IEventTrigger
        {
            _actionProcessor.QueueCrashEvent(eventTrigger, sourceEntity);
        }
        
        public void CancelEvent<T>(T eventTrigger, Entity sourceEntity) where T : IEventTrigger
        {
            _actionProcessor.CancelTriggeredEvent(eventTrigger, sourceEntity);
        }
        
        public void CancelEvent<T>(T eventTrigger) where T : IEventTrigger
        {
            _actionProcessor.CancelTriggeredEvent(eventTrigger, Entity.Null);
        }
        
        public void Update(CoreManagers coreManagers)
        {
            if (ProcessorRunning)
            {
                _actionProcessor.ProcessActions(coreManagers);
            }
            else if (ViewerRunning)
            {
                _actionViewer.PlayNextAction();
            }
        }
        
        public void UpdateProcessor(CoreManagers coreManagers)
        {
            _actionProcessor.ProcessActions(coreManagers);
        }
        
        public void UpdateViewer()
        {
            _actionViewer.PlayNextAction();
        }
        
        public void AddActionVisual<T>(IActionVisualBehaviour actionVisualBehaviour) where T : IEventTrigger
        {
            _actionViewer.AddActionVisual<T>(actionVisualBehaviour);
        }
        
        public void RemoveAction<T>() where T : IActionEffect
        {
            _actionViewer.RemoveAction<T>();
        }
        
        public async Task WaitForAllActionsCompleted(CoreManagers coreManagers)
        {
            while (Running)
            {
                Update(coreManagers);
                await Task.Yield();
            }
        }

        public void ClearEventQueue()
        {
            _actionProcessor.ClearEventQueue();
        }
        
        public void Reset()
        {
            _actionProcessor.Reset();
            _actionViewer.Clear();
        }
    }
}