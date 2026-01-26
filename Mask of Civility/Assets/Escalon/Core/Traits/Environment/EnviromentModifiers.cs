using System;
using System.Collections.Generic;

namespace Escalon.Traits
{
    /// <summary>
    /// Linked to a single action value, if dirty its evaluated when that value is used
    /// </summary>
    [Serializable]
    public class EnvironmentEvaluator 
    {
        private List<StatChange> _modifiers = new List<StatChange>();
        private List<ActionValueCompoundCalculation> _compoundCalculations = new List<ActionValueCompoundCalculation>();
        private bool _isDirty = true;
        private double _value = 0;
        
        public void AddModifier(StatChange modifier)
        {
            _modifiers.Add(modifier);
            _modifiers.Sort(CompareModifierOrder);
            _isDirty = true;
        }
        
        public void AddCompoundCalculation(ActionValueCompoundCalculation calculation)
        {
            _compoundCalculations.Add(calculation);
            _compoundCalculations.Sort(CompareModifierOrder);
        }
        
        public void AddCompoundCalculations(IEnumerable<ActionValueCompoundCalculation> calculations)
        {
            _compoundCalculations.AddRange(calculations);
            _compoundCalculations.Sort(CompareModifierOrder);
        }

        public bool RemoveModifier(StatChange modifier)
        {
            bool removed = _modifiers.Remove(modifier);
            _isDirty |= removed;

            return removed;
        }
        
        public bool RemoveCompoundCalculation(ActionValueCompoundCalculation calculation)
        {
            bool removed = _compoundCalculations.Remove(calculation);
            return removed;
        }

        private int CompareModifierOrder(StatChange a, StatChange b)
        {
            if (a.Order < b.Order)
                return -1;

            return a.Order > b.Order ? 1 : 0;
        }
        
        private int CompareModifierOrder(ActionValueCompoundCalculation a, ActionValueCompoundCalculation b)
        {
            if (a.Order < b.Order)
                return -1;

            return a.Order > b.Order ? 1 : 0;
        }

        public double GetValue(double environmentValue = 0)
        {
            return _isDirty || environmentValue != 0 ? _value = CalculateFinalValue(environmentValue) : _value;
        }
        
        public List<ActionValueCompoundCalculation> GetCompoundCalculations()
        {
            return _compoundCalculations;
        }

        private double CalculateFinalValue(double environmentValue)
        {
            double finalValue = environmentValue;
            double sumPercentAdd = 0;

            for (int i = 0; i < _modifiers.Count; i++)
            {
                StatChange mod = _modifiers[i];

                switch (mod.Type)
                {
                    case ModifierType.Flat:
                    {
                        finalValue += mod.Value;
                        break;
                    }
                    case ModifierType.PercentAdd:
                    {
                        sumPercentAdd += mod.Value;

                        if (i + 1 >= _modifiers.Count || _modifiers[i + 1].Type != ModifierType.PercentAdd)
                        {
                            finalValue *= 1 + sumPercentAdd;
                            sumPercentAdd = 0;
                        }

                        break;
                    }

                    case ModifierType.PercentMult:
                        finalValue *= mod.Value;
                        break;
                }

            }

            _isDirty = false;

            return (float)Math.Round(finalValue, 4);
        }

        public void ClearModifiers(float newBaseline = -1)
        {
            _modifiers.Clear();
            _compoundCalculations.Clear();
            _isDirty = true;
            if (newBaseline != -1)
            {
                StatChange startingValueModifier = new StatChange(newBaseline);
                AddModifier(startingValueModifier);
            }
        }
    }
}
