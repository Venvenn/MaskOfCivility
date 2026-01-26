using System;

namespace Escalon
{
    [Serializable]
    public struct DataStore
    {
        public SerializableDictionary<Type, IData> DataObjects;
        public string Id;
        public string Version;
        public string Notes;
        public bool IncludeInGame;
        public bool Default;
    }
}