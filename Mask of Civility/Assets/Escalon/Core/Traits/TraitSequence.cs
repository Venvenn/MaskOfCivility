using Escalon.ActionSystem;

namespace Escalon.Traits
{
    /// <summary>
    /// a sequence to be used with a trait
    /// </summary>
    public struct TraitSequence
    {
        public Sequence Sequence; 
        public IEventTrigger Event;
        public bool SourceOnly;

        public TraitSequence(IEventTrigger @event, Sequence sequence, bool sourceOnly)
        {
            Sequence = sequence;
            Event = @event;
            SourceOnly = sourceOnly;
        }
    }
}
