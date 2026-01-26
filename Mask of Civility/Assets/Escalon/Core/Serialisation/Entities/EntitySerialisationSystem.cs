using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Extensions;
using Newtonsoft.Json;


namespace Escalon
{
    public class EntitySerialisationSystem
    {
        public static WorldSaveData SerialiseWorld(World world, CoreManagers coreManagers)
        {
            List<EntitySaveData> entitySaveData = new List<EntitySaveData>();
            List<Entity> entitiesToSerialise = new List<Entity>();
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                Converters = new List<JsonConverter>()
                {
                    new EntityConverter(entitiesToSerialise),
                }
            };

            foreach (Archetype archetype in world.Archetypes.Items)
            {
                for (int i = 0; i < archetype.ChunkCount; i++)
                {
                    Chunk chunk = archetype.Chunks[i];
                    for (int j = 0; j < chunk.Count; j++)
                    {
                        EntitySaveData saveData = SerialiseEntity(chunk.Entities[j], settings, coreManagers);
                        entitySaveData.Add(saveData);
                    }
                }
            }

            return new WorldSaveData()
            {
                Entities = entitySaveData
            };
        }

        /// <summary>
        /// Deserialises and creates world. Will add to world if world already exists
        /// </summary>
        public static World DeserialiseWorld(WorldSaveData worldSaveData, CoreManagers coreManagers)
        {
            Dictionary<int, Entity> entityLookup = new Dictionary<int, Entity>(worldSaveData.Entities.Count);
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                Converters = new List<JsonConverter>()
                {
                    new EntitySaveDataConverter(entityLookup),
                }
            };

            coreManagers.EntityManager.World.Clear();
            World world = coreManagers.EntityManager.World;
            
            //Create Entities
            foreach (EntitySaveData entity in worldSaveData.Entities)
            {
                //Create new version of entity
                Entity newEntity = world.Create();
                entityLookup.Add(entity.Id, newEntity);
            }

            //Create components
            foreach (EntitySaveData entity in worldSaveData.Entities)
            {
                object[] componentData = JsonConvert.DeserializeObject<object[]>(entity.ComponentData, settings);
                foreach (var component in componentData)
                {
                    world.Add(entityLookup[entity.Id], component);
                }
            }

            return world;
        }

        public static EntitySaveData SerialiseEntity(Entity entity, JsonSerializerSettings settings, CoreManagers coreManagers)
        {
            object[] components = entity.GetAllComponents();
            string serialisedComponents = JsonConvert.SerializeObject(components, settings);
            return new EntitySaveData
            {
                Id = entity.Id,
                ComponentData = serialisedComponents
            };
        }
    }
}
