
namespace Escalon.ActionSystem
{
    /// <summary>
    /// a step in an action sequence that can have conditional checks for execution as well as a number of uses
    /// </summary>
    public interface IConditionalStep
    {
        /// <summary>
        /// Add a condition check to this step. If a sequence step has any condition checks, all must pass for the step to be evaluated
        /// </summary>
        /// <param name="check">the condition check to add to this step</param>
        /// <param name="targetType">which target gets passed to the condition check</param>
        /// <returns>The passed in condition check, this return value can be used to create compound checks</returns>
        ConditionCheck AddCheck<T>(T check, TargetType targetType) where T : IConditionCheck;
    }
}