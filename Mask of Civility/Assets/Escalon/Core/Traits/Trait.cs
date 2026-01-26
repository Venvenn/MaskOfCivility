
namespace Escalon.Traits
{
    /// <summary>
    /// A trait can contain a name, values, evaluator, sequences and an effect
    /// </summary>
    public class Trait
    {
        public string Name;
        public TraitModifier[] TraitValue;
        public TraitModifier[] EnvironmentModifiers;
        public TraitSequence[] Sequences;
        public AbilitySystem.AbilityEffect Effect;
    }
}