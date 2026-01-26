using System;
using System.Collections.Generic;
using Arch.Core;

namespace Escalon.ActionSystem
{
    /// <summary>
    /// A temporary struct to represent a queued event that has been triggered but not yet run.
    /// </summary>
    public struct TriggeredEvent
    {
        public Entity Source;
        public Type EventType;
        public IEventTrigger Trigger;
        public Queue<ActiveSequence> ActiveSequences;
        
        private List<PastSequence> _completedSequences;
        
        public TriggeredEvent(Entity source, IEventTrigger trigger)
        {
            Source = source;
            Trigger = trigger;
            EventType = trigger.GetType();

            ActiveSequences = new Queue<ActiveSequence>();
            _completedSequences = new List<PastSequence>();
        }
        
        public void AddPastSequences(PastSequence pastSequence)
        {
            _completedSequences.Add(pastSequence);
        }

        public List<PastEvent> CreatePastEvent()
        {
            List<PastEvent> pastEvents = new List<PastEvent>();
            PastEvent pastEvent = new PastEvent(_completedSequences, Trigger, Source);
            pastEvents.Add(pastEvent);

            foreach (PastSequence pastSequence in _completedSequences)
            {
                pastEvents.AddRange(pastSequence.Events);
            }
            
            _completedSequences.Clear();
            return pastEvents;
        }
    }
}

