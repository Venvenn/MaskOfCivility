using System.Collections.Generic;
using Arch.Core;
using Escalon;
using Escalon.ActionSystem;

namespace Escalon.ActionSystem
{
    public struct TargetEventSource : IActionTargeting
    {
        public List<Entity> FindTargets(ActiveSequence sequence, CoreManagers coreManagers)
        {
            return new List<Entity>(new[] { sequence.EventSource });
        }
    }
}
