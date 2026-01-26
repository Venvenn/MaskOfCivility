using System;

namespace Escalon
{
    /// <summary>
    /// The state of an encounter sequence
    /// </summary>
    [Serializable]
    public enum EncounterStatus
    {
        Running,
        Complete,
        Waiting
    }
}