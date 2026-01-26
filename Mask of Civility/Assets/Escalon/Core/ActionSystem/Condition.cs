using System.Collections.Generic;
using System.Threading.Tasks;
using Arch.Core;

namespace Escalon.ActionSystem
{
    /// <summary>
    /// A step that checks if a sequence should continue or not
    /// </summary>
    public struct Condition : ISequenceStep, IConditionalStep
    {
        private List<ConditionCheck> _conditionChecks;
        
        public void Init()
        {
            _conditionChecks = new List<ConditionCheck>();
        }
        
        public static Condition Create<T>(T check, TargetType targetType = TargetType.SequenceSource) where T : IConditionCheck
        {
            Condition condition = new Condition();
            condition.Init();
            condition.AddCheck(check, targetType);
            return condition;
        }
        
        public Task<ActionStatus> Evaluate(ActiveSequence sequence, CoreManagers coreManagers)
        {
            bool success = false;
            
            foreach (var conditionCheck in _conditionChecks)
            {
                if (conditionCheck.GetTargetType() == TargetType.EffectTarget)
                {
                    foreach (var target in sequence.Targets)
                    {
                        success |= conditionCheck.Execute(sequence, coreManagers, target);
                    }
                }
                else
                {
                    success = conditionCheck.Execute(sequence, coreManagers, Entity.Null);
                }
                
                if (!success)
                {
                    break;
                }
            }

            return Task.FromResult(success ? ActionStatus.Running : ActionStatus.Cancelled);
        }
        
        public ConditionCheck AddCheck<T>(T check, TargetType targetType) where T : IConditionCheck
        {
            ConditionCheck conditionCheck = new ConditionCheck<T>(check, targetType);
            _conditionChecks.Add(conditionCheck);
            return conditionCheck;
        }
    }
}