using System;
using Arch.Core;
using Newtonsoft.Json;

namespace Escalon.ActionSystem
{
    /// <summary>
    /// A sequence is a set of actions and conditions that will be run exactly in order 
    /// </summary>
    [Serializable]
    public class Sequence : IDisposable
    {
        /// <summary>
        /// a list of all the steps in this sequence
        /// </summary>
        [JsonProperty]
        private StepList<ISequenceStep> _steps;
        
        /// <summary>
        /// The entity that this sequence is 'attached' to
        /// </summary>
        public Entity SequenceSource { get; set; }
        public IEventTrigger EventTrigger { get; set; }
        
        /// <summary>
        /// The number of steps in this sequence
        /// </summary>
        public int Length => _steps.Length;

        public StepList<ISequenceStep> Steps => _steps;
    
        public Sequence(Entity sequenceSource)
        {
            SequenceSource = sequenceSource;
            _steps = new StepList<ISequenceStep>();
        }
        
        public void AddStep<T>(ref T step) where T : ISequenceStep
        {
            _steps.Add(step);
        }
        
        public void InsertStep<T>(int index, ref T step) where T : ISequenceStep
        {
            _steps.Insert(index, step);
        }

        public void SetEvent(IEventTrigger eventTrigger)
        {
            EventTrigger = eventTrigger;
        }
        
        /// <summary>
        /// runs through each of the steps in the sequence then resets the sequence
        /// </summary>
        public ActiveSequence Build(Entity eventSource, IEventTrigger eventTrigger, CoreManagers coreManagers, Entity eventTriggerTarget)
        {
            ActiveSequence activeSequence = new ActiveSequence(SequenceSource, eventSource, eventTrigger, eventTriggerTarget);
            for (int i = 0; i < _steps.Length; i++)
            {
                var step = _steps[i];
                activeSequence.AddStep(ref step);
            }
            return activeSequence;
        }

        public void Remove<T>(CoreManagers coreManagers) where T: IEventTrigger
        {
            coreManagers.ActionManager.RemoveSequence<T>(this);
        }
        
        public void Dispose()
        {
            Debug.Assert(_steps.Length > 0, $"[Sequence] Sequence for entity {SequenceSource} has no steps and so has not been created correctly");
        }
    }
}

