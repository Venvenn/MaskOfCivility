
using System.Collections.Generic;
using Arch.Core;

namespace Escalon.ActionSystem
{
    public struct TargetEffectSource : IActionTargeting
    {
        public List<Entity> FindTargets(ActiveSequence sequence, CoreManagers coreManagers)
        {
            return new List<Entity>(new []{sequence.SequenceSource});
        }
    }
}

