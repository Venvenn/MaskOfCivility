using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arch.Core;
using Arch.Core.Extensions;
using UnityEngine;

namespace Escalon.ActionSystem
{
    /// <summary>
    /// Responsible for queueing events and running sequences. 
    /// </summary>
    public class ActionProcessor
    {
        /// <summary>
        /// All the events, keyed by the event triggers they are attached to
        /// </summary>
        private readonly Dictionary<Type, Event> _events = new Dictionary<Type, Event>();
        
        /// <summary>
        /// the current queue of events to be run
        /// </summary>
        private readonly Queue<TriggeredEvent> _eventQueue = new Queue<TriggeredEvent>();

        /// <summary>
        /// current event triggers that should be skipped
        /// </summary>
        private Dictionary<Type, Entity> _canceledEventTriggers = new Dictionary<Type, Entity>();
        
        /// <summary>
        /// the event currently being run
        /// </summary>
        private TriggeredEvent _activeEvent;

        private ActiveSequence _activeSequence;
        
        /// <summary>
        /// the current tate of the action system
        /// </summary>
        private ActionSystemState _actionSystemState = ActionSystemState.Inactive;

        /// <summary>
        /// What state the action processor is currently in. 
        /// </summary>
        public ActionSystemState ActionSystemState => _actionSystemState;

        /// <summary>
        /// the current crash queue of events to be run
        /// </summary>
        private readonly Queue<TriggeredEvent> _crashEventQueue = new Queue<TriggeredEvent>();

        public void AddSequence<T>(T eventTrigger, Sequence actionSequence, Entity source, ActionExecutionPhase executionPhase = ActionExecutionPhase.Execute, int useCount = -1) where T : IEventTrigger
        {
            Type type = eventTrigger != null ? eventTrigger.GetType() : typeof(T);

            if (!_events.ContainsKey(type))
            {
                CreateEvent(type);
            }
            _events[type].Add(source, executionPhase, actionSequence);
        }
        
        public void QueueEvent<T>(Entity source) where T : IEventTrigger
        {
            QueueEvent<T>(default, source);
        }
        
        public void QueueEvent<T>(T eventType, Entity source) where T : IEventTrigger
        {
            Type type = eventType.GetType();
            
            if (!_events.ContainsKey(type))
            {
                 CreateEvent(type);
            }
            
            _eventQueue.Enqueue(new TriggeredEvent(source, eventType));
            
            if ((_actionSystemState & ActionSystemState.Active) == 0) 
            {
                _actionSystemState = ActionSystemState.NextEvent;
            }
        }

        public void QueueCrashEvent<T>(T eventType, Entity source) where T : IEventTrigger
        {
            Type type = eventType.GetType();
            
            if (!_events.ContainsKey(type))
            {
                 CreateEvent(type);
            }
            
            _crashEventQueue.Enqueue(new TriggeredEvent(source, eventType));

            if ((_actionSystemState & ActionSystemState.Active) == 0) 
            {
                _actionSystemState = ActionSystemState.NextEvent;
            }
        }

        public void RemoveSequence<T>(T eventTrigger, Sequence actionSequence)
        {
            Type type = eventTrigger.GetType();
            _events[type].Remove(actionSequence.SequenceSource, actionSequence);
        }

        public void RemoveAllSequence<T>(T eventTrigger, Sequence actionSequence)
        {
            Type type = eventTrigger != null ? eventTrigger.GetType() : typeof(T);
            _events[type].RemoveAll(actionSequence);
        }

        public void RemoveAllSequences(Entity entity)
        {
            foreach (var eventType in _events)
            {
                eventType.Value.RemoveAll(entity);
            }    
        }
        
        public List<Sequence> GetSequences<T>(T eventTrigger, Entity entity, ActionExecutionPhase phase) where T : IEventTrigger
        {
            Type type = eventTrigger.GetType();
            bool valid = CheckEventValid(type, entity);
            return _events.TryGetValue(type, out var @event) && valid ? @event.GetSequences(entity, phase) : new List<Sequence>();
        }
        
        public List<Sequence> GetAllEntitySequences<T>(T eventTrigger, Entity entity) where T : IEventTrigger
        {
            Type type = eventTrigger.GetType();
            bool valid = CheckEventValid(type, entity);
            return _events.TryGetValue(type, out var @event) && valid ? @event.GetAllEntitySequences(entity) : new List<Sequence>();
        }
        
        public void ProcessActions(CoreManagers coreManagers)
        {
            switch (_actionSystemState)
            {
                case ActionSystemState.NextEvent:
                    RunNextEvent(coreManagers);
                    break;
                case ActionSystemState.NextSequence:
                    RunNextSequence(coreManagers);
                    break;
                case ActionSystemState.NextAction:
                    //Wait for completion
                    break;
            }
        }
        
        public void CancelTriggeredEvent<T>(T eventTrigger, Entity source) where T : IEventTrigger
        {
            _canceledEventTriggers.Add(eventTrigger.GetType(), source);
        }
        
        private void RunNextEvent(CoreManagers coreManagers)
        {
            if (_activeEvent.ActiveSequences != null)
            {
                List<PastEvent> pastEvents = _activeEvent.CreatePastEvent();
                coreManagers.DataManager.ClearData<EventSequenceData>();
                _activeEvent.Source.PostNotification(Action.Notification(_activeEvent.EventType), _activeEvent.Trigger);
                _activeEvent = default;
                if (_eventQueue.Count == 0 && _crashEventQueue.Count == 0)
                {
                    _actionSystemState = ActionSystemState.Inactive;
                }
                this.PostNotification(ActionManager.k_eventExecuted, pastEvents);
            }
            
            if (_eventQueue.Count > 0 || _crashEventQueue.Count > 0)
            {
                if (_eventQueue.Count > 0)
                {
                    _activeEvent = _eventQueue.Dequeue();
                }
                else if (_crashEventQueue.Count > 0)
                {
                    _activeEvent = _crashEventQueue.Dequeue();
                }

                if (CheckEventValid(_activeEvent.EventType, _activeEvent.Source))
                {
                    var sequences = GetAllEntitySequences(_activeEvent.Trigger,_activeEvent.Source);
                    foreach (Sequence sequence in sequences)
                    {
                        _activeEvent.ActiveSequences.Enqueue(sequence.Build(_activeEvent.Source, _activeEvent.Trigger, coreManagers, Entity.Null));
                    }

                    EventSequenceData eventSequenceData = new EventSequenceData()
                    {
                        Data = new Dictionary<string, object>()
                    };
                    foreach (var data in _activeEvent.Trigger.GetContextData())
                    {
                        eventSequenceData.Data.Add(data.Item1, data.Item2);
                    }
                    coreManagers.DataManager.Write(eventSequenceData);
                    
                    _actionSystemState = ActionSystemState.NextSequence;
                }
                else
                {
                    _actionSystemState = ActionSystemState.NextEvent;
                }

                this.PostNotification(ActionManager.k_eventRun, _activeEvent);
            }
            else
            {
                _canceledEventTriggers.Clear();   
            }
            
            ProcessActions(coreManagers);
        }

        private bool CheckEventValid(Type eventType, Entity eventSource)
        {
            bool canceled = _canceledEventTriggers.ContainsKey(eventType) && (_canceledEventTriggers[eventType] == eventSource || _canceledEventTriggers[eventType] == Entity.Null);
            return !canceled;
        }

        private async void RunNextSequence(CoreManagers coreManagers)
        {
            if (_activeEvent.ActiveSequences.Count > 0)
            {
                _activeSequence = _activeEvent.ActiveSequences.Dequeue();

                _actionSystemState = ActionSystemState.NextAction;
                try
                {
                    PastSequence completedSequence = await _activeSequence.Run(coreManagers);
                    _activeEvent.AddPastSequences(completedSequence);
                    _actionSystemState = ActionSystemState.NextSequence;
                }
                catch (Exception e)
                {
                    Debug.LogError($"Sequence Crashed: {e.Message}, {e.StackTrace}");
                }
            }
            else
            {
                _actionSystemState = ActionSystemState.NextEvent;
            }

            ProcessActions(coreManagers);
        }
        
        private Event CreateEvent(Type type)
        {
            Event newEvent = new Event(type);
            _events.Add(type, newEvent);
            return newEvent;
        }
 
        public void ClearEventQueue()
        {
            _eventQueue.Clear();
        }
        
        public void Reset()
        {
            _events.Clear();
            _eventQueue.Clear();
            _crashEventQueue.Clear();
            _canceledEventTriggers.Clear();
            _activeEvent = default;
            _actionSystemState = ActionSystemState.Inactive; 
        }
    }
}