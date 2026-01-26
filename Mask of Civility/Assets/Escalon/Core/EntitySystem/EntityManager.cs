using System;
using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Utils;
using Schedulers;

namespace Escalon
{
    /// <summary>
    /// Used interact with the Arch entity system
    /// </summary>
    public class EntityManager : BaseManager
    {
        private readonly World _world;
        public World World => _world;

        public EntityManager()
        {
            var jobScheduler = new JobScheduler(
                new JobScheduler.Config
                {
                    ThreadPrefixName = "EntityManager",
                    MaxExpectedConcurrentJobs = 64,
                    StrictAllocationMode = false,
                }
            );
            
            World.SharedJobScheduler = jobScheduler;
            
            _world = World.Create();
        }

        public Entity CreateEntity(ComponentType[] components = null)
        {
            return components == null || components.Length == 0 ? _world.Create() : _world.Create(components);;
        }

        public void CreateEntities(int count, ComponentType[] components = null)
        {
            for (int i = 0; i < count; i++)
            {
                CreateEntity(components);
            }
        }
        
        public void AddComponent<T>(Entity entity, T componentType) 
        {
            _world.Add(entity, componentType);
        }
        
        public void AddBlankComponent<T>(Entity entity, T componentType = default) 
        {
            _world.Add<T>(entity);
        }

        public void SetComponent<T>(Entity entity, T componentType)
        {
            Type type = componentType.GetType();
            
            if (_world.Has(entity, type))
            {
                _world.Set(entity, componentType);
            }
            else
            {
                _world.Add(entity, componentType);
            }
        }

        public void RemoveComponent<T>(Entity entity)
        {
            Type type = typeof(T);
            _world.Remove(entity,type);
        }

        public void RemoveComponent(Entity entity, ComponentType componentType)
        {
            _world.Remove(entity, componentType);
        }

        public T GetComponent<T>(Entity entity)
        {
            return (T)_world.Get(entity, typeof(T));
        }
        
        public ref T GetComponentReadWrite<T>(Entity entity)
        {
            return ref _world.Get<T>(entity);
        }

        public bool Has<T>(Entity entity)
        {
            return _world.Has<T>(entity);
        }
        
        public bool Has(Entity entity, Type type)
        {
            return _world.Has(entity, type);
        }
        
        public T GetComponent<T>(Entity entity, Type componentType)
        {
            return (T) _world.Get(entity, componentType);
        }

        public void DestroyEntity(Entity entity)
        {
            _world.Destroy(entity);
        }
        
        public void ClearWorld()
        {
            World.Clear();
        }
        
        public int DestroyWorld()
        {
            int worldId = _world.Id; 
            World.Destroy(_world);
            return worldId;
        }
    }
}   