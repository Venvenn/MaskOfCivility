using Arch.Core;

namespace Escalon.ActionSystem
{
    /// <summary>
    /// Base class for a created condition check 
    /// </summary>
    public abstract class ConditionCheck
    {
        public abstract TargetType GetTargetType();
        public abstract void SetCompoundEffect(ConditionCheck compoundEffect, LogicalOperatorType operatorType);
        public abstract bool Execute(ActiveSequence sequence, CoreManagers coreManagers, Entity target);
    }

    /// <summary>
    /// Abstract condition check class that allows for the creation of compound checks. 
    /// </summary>
    public class ConditionCheck<T> : ConditionCheck where T : IConditionCheck
    {
        private readonly TargetType _targetType;
        private T _baseCheck;
        private LogicalOperatorType _compoundOperator;
        private ConditionCheck _compoundCheck;

        public ConditionCheck(T baseCheck, TargetType targetType) 
        {
            _targetType = targetType;
            _baseCheck = baseCheck;
            _compoundCheck = null!;
        }

        public override TargetType GetTargetType()
        {
            return _targetType;
        }

        public override void SetCompoundEffect(ConditionCheck compoundEffect, LogicalOperatorType operatorType)
        {
            _compoundCheck = compoundEffect;
            _compoundOperator = operatorType;
        }
        
        public override bool Execute(ActiveSequence sequence, CoreManagers coreManagers, Entity target)
        {
            target = _targetType switch
            {
                TargetType.SequenceSource => sequence.SequenceSource,
                TargetType.EventSource => sequence.EventSource,
                TargetType.EffectTarget => target,
                _ => Entity.Null
            };

            bool result = _baseCheck.Execute(target, sequence, coreManagers);

            return _compoundOperator switch
            {
                LogicalOperatorType.And => result && _compoundCheck.Execute(sequence, coreManagers, target),
                LogicalOperatorType.Or => result || _compoundCheck.Execute(sequence, coreManagers, target),
                LogicalOperatorType.Not => result && !_compoundCheck.Execute(sequence, coreManagers, target),
                _ => result
            };
        }
    }
}