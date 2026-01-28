using System.Collections.Concurrent;
using Arch.Core;
using Arch.System;
using Escalon;
using System.Collections.Generic;
using Arch.Core.Extensions;

public class ResourceSystem : BaseSystem<World, float>
{
    public const string k_updatePlayerResources = "ResourceSystem.UpdatePlayerResources";
    
    private QueryDescription _desc = new QueryDescription().WithAll<GameObjectReference, ResourceData, CountryTileData>();

    private QueryDescription _countryQuery = new QueryDescription().WithAll<CountryData>();
    private CoreManagers _coreManagers;

    public ResourceSystem(World world, CoreManagers coreManagers) : base(world)
    {
        _coreManagers = coreManagers;
    }

    public override void Initialize()
    {
        ResourceConfig resourceConfig = _coreManagers.DataManager.Read<ResourceConfig>();
        Dictionary<Entity, Dictionary<ResourceType, int>> resourceAmounts = new Dictionary<Entity, Dictionary<ResourceType, int>>();
        World.Query(in _desc,
            (Entity entity, ref GameObjectReference gameObjectReference, ref ResourceData resourceData,
                ref CountryTileData tile) =>
            {
                Entity countryEntity = tile.HardHolder;
                if (countryEntity != Entity.Null)
                {
                    if (!resourceAmounts.ContainsKey(countryEntity))
                    {
                        resourceAmounts.Add(countryEntity, new Dictionary<ResourceType, int>());
                    }

                    if (!resourceAmounts[countryEntity].ContainsKey(resourceData.ResourceType))
                    {
                        resourceAmounts[countryEntity].Add(resourceData.ResourceType, 0);
                    }

                    resourceAmounts[countryEntity][resourceData.ResourceType] += resourceData.Amount;
                }
            });
        
        World.Query(in _countryQuery, (Entity entity, ref CountryData gameObjectReference) =>
        { 
            foreach (var (key, value) in resourceAmounts[entity])
            {
                gameObjectReference.ResourceAmounts[key] = (int) (value * resourceConfig.StartModifier[key]);
            }
        });
        
        Notification.AddObserver<FSGame>(OnDayTick, WorldTimeManager.k_dayTick);
        this.PostNotification(k_updatePlayerResources);
    }

    public void OnDayTick(object sender, object args)
    {
        World.Query(in _desc, (Entity entity, ref GameObjectReference gameObjectReference, ref CountryTileData tile, ref ResourceData resourceData) =>
        {
            if (tile.HardHolder != Entity.Null && tile.HardHolder.TryGet<CountryData>(out var countryData))
            {
                countryData.ResourceAmounts[resourceData.ResourceType] += resourceData.Amount;
            }
        });
        
        this.PostNotification(k_updatePlayerResources, true);
    }
}


