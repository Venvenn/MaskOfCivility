using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arch.Core;
using Arch.Core.Extensions;

namespace Escalon.ActionSystem
{
    /// <summary>
    /// The core unit the action system uses to execute behaviour
    /// </summary>
    public abstract class Action : ISequenceStep, IConditionalStep
    {
        /// <summary>
        /// Run the behaviour of this action 
        /// </summary>
        /// <param name="sequence">the sequence this action is part of</param>
        /// <param name="coreManagers">the core managers used to interact with the games data</param>
        /// <returns>The status of the action, if it succeeds or fails</returns>
        public abstract Task<ActionStatus> Evaluate(ActiveSequence sequence, CoreManagers coreManagers);
        
        /// <summary>
        /// Add a condition check to this step. If a sequence step has any condition checks, all must pass for the step to be evaluated
        /// </summary>
        /// <param name="check">the condition check to add to this step</param>
        /// <param name="targetType">which target gets passed to the condition check</param>
        /// <returns>The passed in condition check, this return value can be used to create compound checks</returns>
        public abstract ConditionCheck AddCheck<C>(C check, TargetType targetType = TargetType.EffectTarget) where C : IConditionCheck;
        
        public abstract IActionEffect GetEffect();
        public abstract Entity GetSourceEntity();
        public abstract void Clear();
        
        public override string ToString()
        {
            return GetEffect().GetType().Name;
        }
        
        /// <summary>
        /// Creates a new action with the assigned effect and targeting
        /// </summary>
        public static Action<T> Create<T>(T effect) where T : IActionEffect
        {
            return new Action<T>(effect);
        }
        
        /// <summary>
        /// Notification To Be Sent When Action Executed
        /// </summary>
        public static string Notification(Type type)
        {
            return $"{type.Name}.Notification";
        }
        
        /// <summary>
        /// Notification To Be Sent When Action Executed
        /// </summary>
        public static string Notification<T>()
        {
            return Notification(typeof(T));
        }
        
        /// <summary>
        /// Notification To Be Sent When Action Visualised
        /// </summary>
        public static string NotificationView(System.Type type)
        {
            return $"{type.Name}.ViewNotification";
        }

        /// <summary>
        /// Notification To Be Sent When Action Visualised
        /// </summary>
        public static string NotificationView<T>()
        {
            return NotificationView(typeof(T));
        }
    }
    
    /// <summary>
    /// A specific action created with a targeting behaviour and action effect.
    /// </summary>
    public class Action<T> : Action where T : IActionEffect
    {
        private readonly Dictionary<TargetType, List<ConditionCheck>> _conditionChecks;
        
        private Entity _source;
        private T _effect;
        private int _uses;
        private List<Entity> _targets;
        private bool _isAbility;
        
        public Action(T effect) 
        {
            _effect = effect;
            _source = Entity.Null;
            _targets = new List<Entity>();
            _conditionChecks = new Dictionary<TargetType,  List<ConditionCheck>>
            {
                { TargetType.EffectTarget, new List<ConditionCheck>() },
                { TargetType.EventSource, new List<ConditionCheck>() },
                { TargetType.SequenceSource, new List<ConditionCheck>() }
            };
        }

        public override async Task<ActionStatus> Evaluate(ActiveSequence sequence, CoreManagers coreManagers)
        {
            _source = sequence.SequenceSource;
 
            if (!_source.IsAlive())
            {
                return ActionStatus.Cancelled;
            }
            
            if (CheckConditions(sequence, coreManagers))
            {
                if (sequence.Targets != null && sequence.Targets.Count > 0)
                {
                    foreach (Entity target in sequence.Targets)
                    {
                        if (target == Entity.Null)
                        {
                            continue;   
                        }

                        bool valid = true;
                        foreach (var check in _conditionChecks[TargetType.EffectTarget])
                        {
                            if (!check.Execute(sequence, coreManagers, target))
                            {
                                valid = false;
                                break;
                            }
                        }

                        if (valid)
                        {
                            await ExecuteAction(target, sequence, coreManagers);
                        }
                    }

                    CreatPastAction(sequence);
                    Clear();
                }
            }
            
            return ActionStatus.Running;
        }
        
        private async Task ExecuteAction(Entity target, ActiveSequence sequence, CoreManagers coreManagers)
        {
            //Before
            var beforeSequences = await RunLinkedSequences(target, ActionExecutionPhase.Before, sequence, coreManagers);
            //Execute
            await RunThisAction(target, sequence, coreManagers);
            var executeSequences = await RunLinkedSequences(target, ActionExecutionPhase.Execute, sequence, coreManagers);
            //After
            var afterSequences = await RunLinkedSequences(target, ActionExecutionPhase.After, sequence, coreManagers);
            
            //Events
            List<PastSequence> usedPastSequences = new List<PastSequence>();
            usedPastSequences.AddRange(beforeSequences.used);
            usedPastSequences.AddRange(executeSequences.used);
            usedPastSequences.AddRange(afterSequences.used);
            
            List<PastSequence> receivedPastSequences = new List<PastSequence>();
            receivedPastSequences.AddRange(beforeSequences.received);
            receivedPastSequences.AddRange(executeSequences.received);
            receivedPastSequences.AddRange(afterSequences.received);
            
            PastEvent usedEvent = new PastEvent(usedPastSequences, _effect.ToUsedEvent(), sequence.SequenceSource);
            PastEvent receivedEvent = new PastEvent(receivedPastSequences, _effect.ToReceivedEffect(), target);
            sequence.AddPastEvent(usedEvent);
            sequence.AddPastEvent(receivedEvent);
        }

        private async Task RunThisAction(Entity target, ActiveSequence sequence, CoreManagers coreManagers)
        {
            if (sequence.Status != ActionStatus.Cancelled)
            {
                var executionTask = _effect.Execute(target, sequence, coreManagers);
                _targets.Add(target);
                while (sequence.Status == ActionStatus.Waiting)
                {
                    if (executionTask.IsFaulted)
                    {
                        sequence.Resume();
                        throw executionTask.Exception;
                    }
                    else if (executionTask.IsCanceled)
                    {
                        sequence.Resume();
                        throw new TaskCanceledException("CustomEffect task was cancelled.");
                    }

                    await Task.Yield();
                }
            }
        }

        private async Task<(List<PastSequence> used, List<PastSequence> received)> RunLinkedSequences(Entity target, ActionExecutionPhase phase, ActiveSequence sequence, CoreManagers coreManagers)
        {
            List<PastSequence> usedPastSequences = new List<PastSequence>();
            List<PastSequence> receivedPastSequences = new List<PastSequence>();
            
            if (sequence.Status != ActionStatus.Cancelled)
            {
                sequence.Wait();
                var usedSequences = coreManagers.ActionManager.GetSequences(_effect.ToUsedEvent(), sequence.SequenceSource, phase);
                var targetedSequences = coreManagers.ActionManager.GetSequences( _effect.ToReceivedEffect(), target, phase);

                //Used Event
         
                foreach (Sequence usedSequence in usedSequences)
                {
                    ActiveSequence activeSequence = usedSequence.Build(sequence.EventSource, _effect.ToUsedEvent(), coreManagers, target);
                    activeSequence.SetTargets(sequence.Targets, sequence.CurrentTargeting);
                    PastSequence pastSequences = await activeSequence.Run(coreManagers);
                    usedPastSequences.Add(pastSequences);
                }
                PastEvent usedEvent = new PastEvent(usedPastSequences, _effect.ToUsedEvent(), sequence.SequenceSource);
                sequence.AddPastEvent(usedEvent);
                
                //Received Event
       
                foreach (Sequence targetedSequence in targetedSequences)
                {
                    ActiveSequence activeSequence = targetedSequence.Build(sequence.EventSource, _effect.ToReceivedEffect(), coreManagers, target);
                    activeSequence.SetTargets(sequence.Targets, sequence.CurrentTargeting);
                    PastSequence pastSequences = await activeSequence.Run(coreManagers);
                    receivedPastSequences.Add(pastSequences);
                }
      
                sequence.Resume();
            }
            
            return (usedPastSequences, receivedPastSequences);
        }
        
        public override IActionEffect GetEffect()
        {
            return _effect;
        }
        
        public override Entity GetSourceEntity()
        {
            return _source;
        }

        public override void Clear()
        {
            _targets.Clear();
            _isAbility = false;
        }

        private void CreatPastAction(ActiveSequence sequence)
        {
            PastAction pastAction = new PastAction(sequence.SequenceSource, _effect, sequence.CurrentTargeting, sequence.EventTrigger, _targets, _isAbility);
            sequence.AddPastAction(pastAction);
        }

        private bool CheckConditions(ActiveSequence sequence, CoreManagers coreManagers)
        {
            foreach (var check in _conditionChecks[TargetType.SequenceSource])
            {
                if (!check.Execute(sequence, coreManagers, sequence.SequenceSource))
                {
                    return false;
                }
    
            }
            foreach (var check in _conditionChecks[TargetType.EventSource])
            {
                if(!check.Execute(sequence, coreManagers, sequence.EventSource))
                {
                    return false;
                }
            }
            return true;
        }

        public override ConditionCheck AddCheck<C>(C check, TargetType targetType = TargetType.EffectTarget) 
        {
            ConditionCheck conditionCheck = new ConditionCheck<C>(check, targetType);
            _conditionChecks[targetType].Add(conditionCheck);
            return conditionCheck;
        }
    }
}

