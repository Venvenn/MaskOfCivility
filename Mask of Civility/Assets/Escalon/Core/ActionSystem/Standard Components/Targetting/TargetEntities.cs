using System.Collections.Generic;
using System.Linq;
using Arch.Core;

namespace Escalon.ActionSystem
{
    public struct TargetEntities : IActionTargeting
    {
        private Entity[] _entities;
    
        public TargetEntities(params Entity[] entities)
        {
            _entities = entities;
        }
    
        public List<Entity> FindTargets(ActiveSequence sequence, CoreManagers coreManagers)
        {
            return _entities.ToList();
        }
    }
}
