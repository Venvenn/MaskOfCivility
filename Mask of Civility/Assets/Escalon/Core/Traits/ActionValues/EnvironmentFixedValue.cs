namespace Escalon.Traits
{
    public struct EnvironmentFixedValue : IActionValueEvaluator
    {
        public float Value;
        
        public float GetValue(float environmentValue = 0)
        {
            return Value = environmentValue;
        }
    }
}