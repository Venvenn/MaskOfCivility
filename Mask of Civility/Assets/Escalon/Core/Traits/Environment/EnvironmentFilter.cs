using System;

namespace Escalon.Traits
{
    /// <summary>
    /// A filter for which area of the environment tree should be looked at
    /// </summary>
    [Flags]
    public enum EnvironmentFilter
    {
        None = 0,
        Self = 1 << 0,
        Children = 1 << 1,
        Descendants = 1 << 2,
        Parent = 1 << 3,
        Ancestors = 1 << 4
    }
}