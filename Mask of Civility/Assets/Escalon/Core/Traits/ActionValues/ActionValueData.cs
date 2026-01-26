using System.Collections.Generic;

namespace Escalon.Traits
{
    public struct ActionValueData
    {
        public Dictionary<ActionValue, EnvironmentEvaluator> Values;
        public Dictionary<ActionValue, EnvironmentEvaluator> Modifiers;
        public Dictionary<ActionValue, EnvironmentEvaluator> Resources;
        
        public double this[ActionValueType type, ActionValue actionValue]
        {
            get
            {
                switch (type)
                {
                    case ActionValueType.Value:
                        return Values[actionValue].GetValue();
                    case ActionValueType.Modifier:
                        return Modifiers[actionValue].GetValue();
                    case ActionValueType.Resource:
                        return Resources[actionValue].GetValue();
                    default:
                        return 0;
                }
            }
        }

        public Dictionary<ActionValue, EnvironmentEvaluator> Get(ActionValueType type)
        {
            switch (type)
            {
                case ActionValueType.Value:
                    return Values;
                case ActionValueType.Modifier:
                    return Modifiers;
                case ActionValueType.Resource:
                    return Resources;
                default:
                    return null;
            }
        }
        
        public void Add(ActionValueType type, CompoundActionValue value, StatChange modifier)
        {
            switch (type)
            {
                case ActionValueType.Value:
                    Values[value.BaseActionValue].AddModifier(modifier);
                    if (value.CompoundCalculations?.Count > 0)
                    {
                        Values[value.BaseActionValue].AddCompoundCalculations(value.CompoundCalculations);
                    }
                    break;
                case ActionValueType.Modifier:  
                    Modifiers[value.BaseActionValue].AddModifier(modifier);
                    if (value.CompoundCalculations?.Count > 0)
                    {
                        Modifiers[value.BaseActionValue].AddCompoundCalculations(value.CompoundCalculations);
                    }
                    break;
                case ActionValueType.Resource:
                    Resources[value.BaseActionValue].AddModifier(modifier);
                    break;
            }
        }
        
        public void Remove(ActionValueType type, CompoundActionValue value, StatChange modifier)
        {
            switch (type)
            {
                case ActionValueType.Value:
                    Values[value.BaseActionValue].RemoveModifier(modifier);
                    break;
                case ActionValueType.Modifier:  
                    Modifiers[value.BaseActionValue].RemoveModifier(modifier);
                    break;
                case ActionValueType.Resource:
                    Resources[value.BaseActionValue].RemoveModifier(modifier);
                    break;
            }
        }
        
        public void Remove(ActionValueType type, CompoundActionValue value, ActionValueCompoundCalculation calculation)
        {
            switch (type)
            {
                case ActionValueType.Value:
                    Values[value.BaseActionValue].RemoveCompoundCalculation(calculation);
                    break;
                case ActionValueType.Modifier:  
                    Modifiers[value.BaseActionValue].RemoveCompoundCalculation(calculation);
                    break;
                case ActionValueType.Resource:
                    Resources[value.BaseActionValue].RemoveCompoundCalculation(calculation);
                    break;
            }
        }
    }
}