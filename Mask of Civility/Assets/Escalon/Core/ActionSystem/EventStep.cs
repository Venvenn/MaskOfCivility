using System.Collections.Generic;
using System.Threading.Tasks;
using Arch.Core;

namespace Escalon.ActionSystem
{
    public abstract class EventStep : ISequenceStep
    {
        public abstract Task<ActionStatus> Evaluate(ActiveSequence sequence, CoreManagers coreManagers);
        
        public static EventStep Create<T>(T eventTrigger) where T : IEventTrigger
        {
            return new EventStep<T>(eventTrigger, Entity.Null);
        }
        
        public static EventStep Create<T>(T eventTrigger, Entity eventSource) where T : IEventTrigger
        {
            return new EventStep<T>(eventTrigger, eventSource);
        }
    }
    
    /// <summary>
    /// A sequence step used to trigger an event as part of the sequence,
    /// so any effects linked to that event will trigger here as part of this step
    /// </summary>
    public class EventStep<T> : EventStep where T : IEventTrigger
    {
        private readonly Entity _eventSource;
        private readonly T _eventTrigger;
        private readonly List<ConditionCheck> _conditionChecks;
        
        public EventStep(T eventTrigger, Entity eventSource)
        {
            _eventTrigger = eventTrigger;
            _eventSource = eventSource;
            _conditionChecks = new List<ConditionCheck>();
        }
        
        public override Task<ActionStatus> Evaluate(ActiveSequence sequence, CoreManagers coreManagers)
        {
            bool success = true;
            
            foreach (var conditionCheck in _conditionChecks)
            {
                success = conditionCheck.Execute(sequence, coreManagers, sequence.SequenceSource);
                if (!success)
                {
                    break;
                }
            }   

            if (success)
            {
                ExecuteEvent(sequence, coreManagers);
                return Task.FromResult(ActionStatus.Running);
            }
            
            return Task.FromResult(ActionStatus.Cancelled);
        }
        
        private async void ExecuteEvent(ActiveSequence sequence, CoreManagers coreManagers)
        {
            sequence.Wait();
            var sequences = coreManagers.ActionManager.GetSequences(_eventTrigger, _eventSource);
            List<PastSequence> pastSequences = new List<PastSequence>();
            foreach (Sequence eventSequence in sequences)
            {
                ActiveSequence activeSequence = eventSequence.Build(sequence.EventSource, _eventTrigger, coreManagers, Entity.Null);
                PastSequence pastSequence = await activeSequence.Run(coreManagers);
                pastSequences.Add(pastSequence);
            }
            
            PastEvent pastEvent = new PastEvent(pastSequences, _eventTrigger, _eventSource);
            sequence.AddPastEvent(pastEvent);
            
            _eventSource.PostNotification(Action.Notification(_eventTrigger.GetType()), _eventTrigger);
            sequence.Resume();
        }
        
        public ConditionCheck AddCheck<C>(C check, TargetType targetType) where C : IConditionCheck
        {
            ConditionCheck conditionCheck = new ConditionCheck<C>(check, targetType);
            _conditionChecks.Add(conditionCheck);
            return conditionCheck;
        }
    }
  
}
