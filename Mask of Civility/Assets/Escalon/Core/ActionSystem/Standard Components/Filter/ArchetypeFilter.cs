using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Utils;
using Escalon;
using Escalon.ActionSystem;

namespace Escalon.ActionSystem
{
    public struct ArchetypeFilter : IFilter
    {
        private ComponentType[] _componentTypes;
    
        public ArchetypeFilter(ComponentType[] componentTypes)
        {
            _componentTypes = componentTypes;
        }
    
        public void Filter(ref List<Entity> targets, ActiveSequence sequence, CoreManagers coreManagers)
        {
            foreach (var entity in targets)
            {
                if (!coreManagers.EntityManager.World.HasRange(entity, _componentTypes))
                {
                    targets.Remove(entity);
                }
            }
        }
    }
}
