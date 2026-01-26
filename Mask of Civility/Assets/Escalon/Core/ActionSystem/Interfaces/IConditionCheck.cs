using Arch.Core;

namespace Escalon.ActionSystem
{
    /// <summary>
    /// A check used as part of the action system to decide whether to run or cancel an action 
    /// </summary>
    public interface IConditionCheck
    {
        /// <summary>
        /// Executes the condition check
        /// </summary>
        /// <param name="target">the target, does not need to be used in the check but may sometimes be helpful</param>
        /// <param name="sequence">he sequence this check is apart of</param>
        /// <param name="coreManagers">core set of managers used ot interact with the data of the game</param>
        /// <returns>True if the check success and false if it has failed</returns>
        bool Execute(Entity target, ActiveSequence sequence, CoreManagers coreManagers);
    }
   
}
