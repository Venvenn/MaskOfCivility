using System;
using Arch.Core;

namespace Escalon.Traits
{
    /// <summary>
    /// a single calculation applied to a value
    /// </summary>
    [Serializable]
    public class ActionValueCompoundCalculation
    {
        public Entity TargetEntity;
        public CompoundActionValue ActionValue;
        public ArithmeticOperatorType Type;
        public int Order;

        public ActionValueCompoundCalculation(Entity targetEntity, CompoundActionValue actionValue, ArithmeticOperatorType type = ArithmeticOperatorType.Add, int order = 0)
        {
            TargetEntity = targetEntity;
            ActionValue = actionValue;
            Type = type;
            Order = order;
        }
    }
}