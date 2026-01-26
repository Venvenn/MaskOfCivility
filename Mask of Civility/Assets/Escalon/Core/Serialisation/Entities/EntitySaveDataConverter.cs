using System;
using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Extensions;
using Newtonsoft.Json;

namespace Escalon
{
    public class EntitySaveDataConverter : JsonConverter
    {
        private readonly Dictionary<int, Entity> _entityLookup;

        public override bool CanWrite => false;

        public EntitySaveDataConverter(Dictionary<int, Entity> entityLookup)
        {
            _entityLookup = entityLookup;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            long entityIdLong = (long)reader.Value;
            int entityId = (int)entityIdLong;

            Entity returnEntity = Entity.Null;

            if (_entityLookup.TryGetValue(entityId, out Entity createdEntity))
            {
                returnEntity = createdEntity;
            }

            if (objectType == typeof(Entity))
            {
                return returnEntity;
            }
            else
            {
                return returnEntity;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Entity) || objectType == typeof(Entity);
        }
    }
}