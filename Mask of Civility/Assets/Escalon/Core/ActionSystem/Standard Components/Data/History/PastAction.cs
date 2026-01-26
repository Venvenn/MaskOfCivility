using System.Collections.Generic;
using Arch.Core;

namespace Escalon.ActionSystem
{
    public struct PastAction : IPastActionSystemData
    {
        public IActionEffect Effect;
        public IActionTargeting Targeting;
        public IEventTrigger EventTrigger;
        public Entity[] TargetedEntities;
        public Entity ActionSource;
        public bool IsAbility;
        
        public PastAction(Entity actionSource, IActionEffect effect, IActionTargeting targeting, IEventTrigger eventTrigger, List<Entity> targets, bool isAbility)
        {
            ActionSource = actionSource;
            Effect = effect;
            Targeting = targeting;
            EventTrigger = eventTrigger;
            TargetedEntities = targets.ToArray();
            IsAbility = isAbility;
        }

        public List<IPastActionSystemData> GetAllActions(bool include = false)
        {
            return new List<IPastActionSystemData> {this};
        }
    }
}