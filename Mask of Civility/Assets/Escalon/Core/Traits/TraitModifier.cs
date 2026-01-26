
namespace Escalon.Traits
{
    /// <summary>
    /// A modifier to be used with a trait
    /// </summary>
    public struct TraitModifier 
    {
        public ActionValue Value; 
        public StatChange Modifier;

        public TraitModifier(ActionValue value, StatChange modifier)
        {
            Value = value;
            Modifier = modifier;
        }
    }
}
