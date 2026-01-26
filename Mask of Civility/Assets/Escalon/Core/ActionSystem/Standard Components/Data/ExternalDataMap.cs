using System;
using System.Collections.Generic;

namespace Escalon.Traits
{
    [Serializable]
    public struct ExternalDataMap : IData
    {
        public Dictionary<string, Func<DataManager, float>> ExternalMapping;
    }
}