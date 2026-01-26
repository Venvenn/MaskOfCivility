using System;
using System.Collections.Generic;
using Arch.Core;

namespace Escalon.Traits
{
    /// <summary>
    /// An action value that contains a combination of other values that are evaluated as one
    /// </summary>
    [Serializable]
    public struct CompoundActionValue
    {
        public readonly ActionValue BaseActionValue;
        public List<ActionValueCompoundCalculation> CompoundCalculations;
        
        public CompoundActionValue(ActionValue name)
        {
            BaseActionValue = name;
            CompoundCalculations = new List<ActionValueCompoundCalculation>();
        }
        
        public CompoundActionValue(ActionValue name, params ActionValueCompoundCalculation[] compoundCalculations)
        {
            BaseActionValue = name;
            CompoundCalculations = new List<ActionValueCompoundCalculation>(compoundCalculations);
        }
    
    
        public void Add(Entity entity, CompoundActionValue value, ArithmeticOperatorType operatorType =ArithmeticOperatorType.Add, int order = 0)
        {
            CompoundCalculations.Add(new ActionValueCompoundCalculation(entity, value, operatorType, order));
        }
        
        public static implicit operator CompoundActionValue(ActionValue actionValue)
        {
            return new CompoundActionValue(actionValue);
        }
        
        public static implicit operator CompoundActionValue(int actionValue)
        {
            return new CompoundActionValue(actionValue.ToString());
        }
        
        public static implicit operator CompoundActionValue(string actionValue)
        {
            return new CompoundActionValue(actionValue);
        }
        
        public static implicit operator string (CompoundActionValue actionValue)
        {
            return actionValue.BaseActionValue;
        }
    }
}

