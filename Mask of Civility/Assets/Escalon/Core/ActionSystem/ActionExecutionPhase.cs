
namespace Escalon.ActionSystem
{
    /// <summary>
    /// The priority of an action within the same sequence
    /// </summary>
    public enum ActionExecutionPhase
    {
        Before,
        Execute,
        After,
    }
}