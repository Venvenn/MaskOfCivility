namespace Escalon.ActionSystem
{
    /// <summary>
    /// Turns an effect into an event. Any sequences responding to this event will happen as part as part of that
    /// effects sequence step and start with the current targeting of the parent sequence 
    /// </summary>
    public struct ReceivedEffect<T> : IEventTrigger where T : IActionEffect
    {
        public T Effect;

        public ReceivedEffect(T effect)
        {
            Effect = effect;
        }
    }
}

