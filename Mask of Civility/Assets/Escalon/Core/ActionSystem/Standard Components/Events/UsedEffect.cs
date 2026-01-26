
namespace Escalon.ActionSystem
{
    /// <summary>
    /// Turns an effect into an event. Any sequences responding to this event will happen as part as part of that
    /// effects sequence step and start with the current targeting of the parent sequence 
    /// </summary>
    public struct UsedEffect<T> : IEventTrigger where T : IActionEffect
    {
        public readonly T Effect;

        public UsedEffect(T effect)
        {
            Effect = effect;
        }
        
        public static implicit operator T(UsedEffect<T> archetypeWrapper)
        {
            return archetypeWrapper.Effect;
        } 
    }
}