
using System.Collections.Generic;
using Arch.Core;
using Escalon.Traits;

namespace Escalon.ActionSystem
{
    public struct TargetNew<T> : IActionTargeting
    {
        private ActionValue _count;

        public TargetNew(ActionValue count)
        {
            _count = count;
        }

        public List<Entity> FindTargets(ActiveSequence sequence, CoreManagers coreManagers)
        {
            int value = (int)ActionValueSystem.GetActionValue(sequence.SequenceSource, _count, coreManagers);

            List<Entity> targets = new List<Entity>(value);
            for (int i = 0; i < value; i++)
            {
                targets.Add(coreManagers.EntityManager.CreateEntity());
            }
            return targets;
        }
    }
}

