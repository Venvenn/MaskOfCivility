
using System.Collections.Generic;

namespace Escalon.ActionSystem
{
    public interface IPastActionSystemData
    {
        List<IPastActionSystemData> GetAllActions(bool includeSubEvents = false);
    }
} 

