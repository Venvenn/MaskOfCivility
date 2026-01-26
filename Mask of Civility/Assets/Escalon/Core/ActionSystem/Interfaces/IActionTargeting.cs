using System.Collections.Generic;
using Arch.Core;

namespace Escalon.ActionSystem
{
    /// <summary>
    /// Action targeting used by the action system to find target entities in the game world to apply effects to
    /// </summary>
    public interface IActionTargeting
    {
        /// <summary>
        /// Should contain the logic to find the appropriate targets for the targeting behaviour
        /// </summary>
        /// <param name="sequence">The sequence this targeting behaviour is part of</param>
        /// <param name="coreManagers">Core set of managers used ot interact with the data of the game</param>
        List<Entity> FindTargets(ActiveSequence sequence, CoreManagers coreManagers);
    }
}