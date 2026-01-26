using System.Collections.Generic;
using System.Threading.Tasks;
using Arch.Core;

namespace Escalon.ActionSystem
{
    public abstract class Targeting : ISequenceStep
    {
        public abstract void AddFilter<F>(F filter) where F : IFilter;
        public abstract void FilterTargeting(ref List<Entity> targets, ActiveSequence sequence, CoreManagers coreManagers);
        public abstract Task<ActionStatus> Evaluate(ActiveSequence sequence, CoreManagers coreManagers);
        public abstract IActionTargeting GetTargetingBehaviour();
        
        public static Targeting<T> Create<T>(T actionTargeting) where T : IActionTargeting
        {
            return new Targeting<T>(actionTargeting);
        }
    }
    
    public class Targeting<T> : Targeting where T : IActionTargeting
    {
        /// <summary>
        /// The targeting behaviour to use
        /// </summary>
        private readonly T _actionTargeting;

        /// <summary>
        /// The targets that the effect will be applied to
        /// </summary>
        private readonly FilterList<IFilter> _filters;

        public Targeting(T actionTargeting)
        {
            _actionTargeting = actionTargeting;
            _filters = new FilterList<IFilter>();
        }

        public override void AddFilter<F>(F filter)
        {
            _filters.Add(filter);
        }

        public override void FilterTargeting(ref List<Entity> targets, ActiveSequence sequence, CoreManagers coreManagers)
        {
            for (int i = 0; i < _filters.Length; i++)
            {
                _filters[i].Filter(ref targets, sequence, coreManagers);
            }
        }

        public override Task<ActionStatus> Evaluate(ActiveSequence sequence, CoreManagers coreManagers)
        {
            List<Entity> entities = _actionTargeting.FindTargets(sequence, coreManagers);
            FilterTargeting(ref entities, sequence, coreManagers);
            sequence.SetTargets(entities, _actionTargeting);
            return Task.FromResult(ActionStatus.Running);
        }

        public override IActionTargeting GetTargetingBehaviour()
        {
            return _actionTargeting;
        }
    }
}