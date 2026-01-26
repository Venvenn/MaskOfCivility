using System;
using System.Collections.Generic;

namespace Escalon
{
    [Serializable]
    public struct WorldSaveData
    {
        public List<EntitySaveData> Entities;

        public WorldSaveData(List<EntitySaveData> entities)
        {
            Entities = entities;
        }
    }
}