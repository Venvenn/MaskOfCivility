using System.Collections.Generic;
using Arch.Core;

namespace Escalon.ActionSystem
{
    public struct PastEvent : IPastActionSystemData
    {
        public List<PastSequence> Sequences;
        public IEventTrigger EventTrigger;
        public Entity EventSource;

        public PastEvent(List<PastSequence> sequences, IEventTrigger eventTrigger, Entity eventSource)
        {
            Sequences = new List<PastSequence>(sequences);
            EventTrigger = eventTrigger;
            EventSource = eventSource;
        }

        public List<IPastActionSystemData> GetAllActions(bool includeSubEvents = false)
        {
            List<IPastActionSystemData> actions = new List<IPastActionSystemData>();

            for (int i =  Sequences.Count-1; i >=0; i--)
            {
                actions.AddRange(Sequences[i].GetAllActions());
            }
            
            if (includeSubEvents && actions.Count >0)
            {
                actions.Add(this);
            }
    
            return actions;
        }
        
    }
}



