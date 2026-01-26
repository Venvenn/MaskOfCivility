using Arch.Core;
using System.Threading.Tasks;

namespace Escalon.ActionSystem
{
    /// <summary>
    /// Action effect used by the action system to affect the game world. Also functions as an event trigger.
    /// </summary>
    public interface IActionEffect  
    {
        /// <summary>
        /// Runs the logic of the action effect
        /// </summary>
        /// <param name="target">The target the logic will be applied to, selected by <see cref="IActionTargeting"/></param>
        /// <param name="sequence">The sequence this effect is part of</param>
        /// <param name="coreManagers">CoreManagers used to interact with the state of the game</param>
        Task Execute(Entity target, ActiveSequence sequence, CoreManagers coreManagers);
    }
}