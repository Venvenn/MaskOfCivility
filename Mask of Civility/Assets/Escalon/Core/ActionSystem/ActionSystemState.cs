using System;

namespace Escalon.ActionSystem
{
    /// <summary>
    /// Represents the state of the action processor 
    /// </summary>
    [Flags]
    public enum ActionSystemState 
    {
        Inactive = 0,
        NextSequence = 1 << 0,
        NextEvent = 1 << 1,
        NextAction = 1 << 2,
        Active = NextSequence | NextEvent | NextAction
    }
}
