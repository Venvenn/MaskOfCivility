using System;

namespace Escalon
{
    [Serializable]
    public struct EntitySaveData : IEquatable<EntitySaveData>
    {
        public int Id;
        public string ComponentData;

        public EntitySaveData(int id, Type worldLayer, string componentData)
        {
            Id = id;
            ComponentData = componentData;
        }

        public bool Equals(EntitySaveData other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is EntitySaveData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}