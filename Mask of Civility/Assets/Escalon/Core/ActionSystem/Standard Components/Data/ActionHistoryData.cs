
using System;
using System.Collections.Generic;

namespace Escalon.ActionSystem
{
    [Serializable]
    public struct ActionHistoryData : IData    
    {
        public List<IPastActionSystemData> History;
    }

}
