
namespace Escalon.ActionSystem
{
    /// <summary>
    /// Methods used to construct compound conditions
    /// </summary>
    public static class ConditionExtensions 
    {
        public static ConditionCheck And<T>(this ConditionCheck check, T andCheck, TargetType targetType) where T : IConditionCheck 
        {
            ConditionCheck conditionCheck = new ConditionCheck<T>(andCheck, targetType);
            check.SetCompoundEffect(conditionCheck, LogicalOperatorType.And);
            return conditionCheck;
        }
        
        public static ConditionCheck Or<T>(this ConditionCheck check, T orCheck, TargetType targetType) where T : IConditionCheck 
        {
            ConditionCheck conditionCheck = new ConditionCheck<T>(orCheck, targetType);
            check.SetCompoundEffect(conditionCheck, LogicalOperatorType.Or);
            return conditionCheck;
        }
        
        public static ConditionCheck Not<T>(this ConditionCheck check, T notCheck, TargetType targetType) where T : IConditionCheck 
        {
            ConditionCheck conditionCheck = new ConditionCheck<T>(notCheck, targetType);
            check.SetCompoundEffect(conditionCheck, LogicalOperatorType.Not);
            return conditionCheck;
        }
    }
}
