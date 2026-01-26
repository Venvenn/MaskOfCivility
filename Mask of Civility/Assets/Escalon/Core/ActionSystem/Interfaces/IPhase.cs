
namespace Escalon.ActionSystem
{
    /// <summary>
    /// Phase of a gameplay loop, can be used to give more fine grain control the flow of actions and their visualisation. 
    /// </summary>
    public interface IPhase
    {
        void TriggerEnterPhase<T>() where T : IEventTrigger;
    }
}

