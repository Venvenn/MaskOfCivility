using System;

namespace Escalon
{
    /// <summary>
    /// The the type of encounter an encounter line is
    /// </summary>
    [Serializable]
    public enum EncounterLineType
    {
        None,
        Battle,
        Forge,
        Shop
    }   
}