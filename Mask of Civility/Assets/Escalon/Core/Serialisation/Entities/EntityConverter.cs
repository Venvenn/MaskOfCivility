using System;
using System.Collections.Generic;
using System.Linq;
using Arch.Core;
using Arch.Core.Extensions;
using Escalon;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Escalon
{
    public class EntityConverter : JsonConverter
    {
        private readonly List<Entity> _writeEntities;

        public EntityConverter(List<Entity> writeEntities)
        {
            _writeEntities = writeEntities;
        }

        public override bool CanRead => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                Entity entityToSave = Entity.Null;

                switch (value)
                {
                    case Entity entity:
                    {
                        entityToSave = entity;
                        break;
                    }
                    default:
                    {
                        Debug.LogError($"Trying to ready type {value.GetType()} as entity, which it is not");
                        break;
                    }
                }

                if (!_writeEntities.Contains(entityToSave) && World.Worlds[entityToSave.WorldId] != null &&
                    entityToSave.IsAlive())
                {
                    _writeEntities.Add(entityToSave);
                }

                JValue o = new JValue(entityToSave.Id);
                o.WriteTo(writer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Entity) || objectType == typeof(Entity);
        }
    }
}