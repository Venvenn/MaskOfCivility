using System;

namespace Escalon.Traits
{
    /// <summary>
    /// a single modifier applied to a value
    /// </summary>
    [Serializable]
    public class StatChange
    {
        public double Value;
        public ModifierType Type;
        public int Order;

        public StatChange(double value, ModifierType type = ModifierType.Flat, int order = 0)
        {
            Value = value;
            Type = type;
            Order = order;
        }

        public static implicit operator StatChange(double value)
        {
            return new StatChange(value);
        }

        public static implicit operator double(StatChange modifier)
        {
            return modifier.Value;
        }
    }
}