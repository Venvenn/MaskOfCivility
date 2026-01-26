using System.Collections.Generic;

namespace Escalon.ActionSystem
{
    /// <summary>
    /// A sequence can be subscribed to run when an event trigger is triggered, anything can be an event trigger, 
    /// </summary>
    public interface IEventTrigger
    {
        List<(string, object)> GetContextData()
        {
            return new List<(string, object)>();
        }
    }
}