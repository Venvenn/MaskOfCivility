using System.Collections.Generic;
using Arch.Core;

namespace Escalon.ActionSystem
{
    public struct PastSequence : IPastActionSystemData
    {
        public List<PastAction> Actions;
        public List<PastEvent> Events;
        public Entity SequenceSource;
        public ActionStatus Status;

        public PastSequence(List<PastAction> actions, List<PastEvent> events, Entity sequenceSource,  ActionStatus status)
        {
            Actions = new List<PastAction>(actions);
            Events = new List<PastEvent>(events);
            SequenceSource = sequenceSource;
            Status = status;
        }

        public List<IPastActionSystemData> GetAllActions(bool include = false)
        {
            List<IPastActionSystemData> actions = new List<IPastActionSystemData> {};

            for (int i =  Actions.Count-1; i >=0; i--)
            {
                actions.Add(Actions[i]);   
            }
            // foreach (var pastEvent in Events)
            // {
            //     actions.AddRange(pastEvent.GetAllActions());   
            // }

            return actions;
        }
    }
}