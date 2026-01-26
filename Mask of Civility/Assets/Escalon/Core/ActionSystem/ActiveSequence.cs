using System.Collections.Generic;
using System.Threading.Tasks;
using Arch.Core;

namespace Escalon.ActionSystem
{
    /// <summary>
    /// A sequence is a set of actions and conditions that will be run exactly in order 
    /// </summary>
    public class ActiveSequence
    {
        /// <summary>
        /// Actions that have been completed during the current run of the sequence
        /// </summary>
        private readonly List<PastAction> _completedActions;

        /// <summary>
        /// past events triggered as steps within this sequence
        /// </summary>
        private readonly List<PastEvent> _completedEvents;

        /// <summary>
        /// a list of all the steps in this sequence
        /// </summary>
        private readonly NonBoxingQueue<ISequenceStep> _steps;

        /// <summary>
        /// the current status of this sequence
        /// </summary>
        private ActionStatus _status;
        
        /// <summary>
        /// The entity that this sequence is 'attached' to
        /// </summary>
        public Entity SequenceSource { get; }

        /// <summary>
        /// The entity that triggered the event this sequence is 'attached' to
        /// </summary>
        public Entity EventSource { get; private set; }

        /// <summary>
        /// The Entities that will be targeted by any effects run by this sequence
        /// </summary>
        public List<Entity> Targets { get; private set; }

        /// <summary>
        /// The targeting behaviour currently being used. 
        /// </summary>
        public IActionTargeting CurrentTargeting { get; private set; }

        /// <summary>
        /// The event that triggered this sequence 
        /// </summary>
        public IEventTrigger EventTrigger { get; private set; }

        /// <summary>
        /// The target for the event that triggered this sequence.
        /// </summary>
        public Entity EventTriggerTarget { get; private set; }

        /// <summary>
        /// The current sequence status
        /// </summary>
        public ActionStatus Status => _status;

        /// <summary>
        /// The number of steps in this sequence
        /// </summary>
        public int Count => _steps.Length;

        /// <summary>
        /// If the sequence is currently running 
        /// </summary>
        public bool Running => _status == ActionStatus.Running || _status == ActionStatus.Waiting;
        
        public ActiveSequence(Entity sequenceSource, Entity eventSource, IEventTrigger eventTrigger, Entity eventTriggerTarget)
        {
            SequenceSource = sequenceSource;
            EventSource = eventSource;
            EventTrigger = eventTrigger;
            EventTriggerTarget = eventTriggerTarget;
            Targets = new List<Entity>();
            CurrentTargeting = null;

            _completedActions = new List<PastAction>();
            _completedEvents = new List<PastEvent>();
            _steps = new NonBoxingQueue<ISequenceStep>();
            _status = ActionStatus.Pending;
        }
        
        /// <summary>
        /// Sets the current targets for this sequence
        /// </summary>
        public void SetTargets(List<Entity> targets, IActionTargeting targeting = default)
        {
            Targets = targets;
            if (CurrentTargeting != default)
            {
                CurrentTargeting = targeting;
            }
        }
        
        public void AddStep<T>(ref T step) where T : ISequenceStep
        {
            _steps.Enqueue(step);
        }
        
        public void Wait()
        {
            _status = ActionStatus.Waiting;
        }
        
        public void Cancel()
        {
            _status = ActionStatus.Cancelled;
        }
        
        public void Resume()
        {
            _status = ActionStatus.Running;
        }
        
        /// <summary>
        /// runs through each of the steps in the sequence then resets the sequence
        /// </summary>
        public async Task<PastSequence> Run(CoreManagers coreManagers)
        {
            while (_status != ActionStatus.Complete || _status !=  ActionStatus.Cancelled)
            {
                if (_status == ActionStatus.Running || _status == ActionStatus.Pending)
                {
                    _status = await RunStep(coreManagers);
                }
                else
                {
                    await Task.Yield();
                }
            }
            
            return CreatePastSequence();
        }
        
        private async Task<ActionStatus> RunStep(CoreManagers coreManagers)
        {
            if (Count == 0)
            {
                return ActionStatus.Complete;
            }
            
            ActionStatus result = await _steps.Dequeue().Evaluate(this, coreManagers);
            
            return _status == ActionStatus.Waiting || _status == ActionStatus.Cancelled ? _status : Count > 0 || result == ActionStatus.Cancelled  ? result : ActionStatus.Complete;
        }

        public void AddPastAction(PastAction action)
        {
            _completedActions.Add(action);
        }
        
        public void AddPastEvent(PastEvent pastEvent)
        {
            _completedEvents.Add(pastEvent);
        }
        
        /// <summary>
        /// Creates a past sequence struct that is intended to be used by the action history 
        /// </summary>
        private PastSequence CreatePastSequence()
        {
            PastSequence pastSequence = new PastSequence(_completedActions, _completedEvents, SequenceSource, _status);
            _completedActions.Clear();
            return pastSequence;
        }
    }
}

