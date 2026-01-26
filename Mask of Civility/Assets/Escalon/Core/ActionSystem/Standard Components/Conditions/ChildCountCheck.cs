using System.Linq;
using Arch.Core;

namespace Escalon.ActionSystem
{
    public struct ChildCountCheck : IConditionCheck
    {
        private EqualityOperatorType _equalityOperatorType;
        private int _value;

        public ChildCountCheck(int value, EqualityOperatorType equalityOperatorType)
        {
            _value = value;
            _equalityOperatorType = equalityOperatorType;
        }

        public bool Execute(Entity target, ActiveSequence sequence, CoreManagers coreManagers)
        {
            int count = coreManagers.EntityManager.GetComponent<EnvironmentData>(target).Environment.Children.Count();

            switch (_equalityOperatorType)
            {
                case EqualityOperatorType.LessThan:
                    return _value < count;
                case EqualityOperatorType.Equal:
                    return _value == count;
                case EqualityOperatorType.GreaterThan:
                    return _value > count;
            }

            return false;
        }
    }
}