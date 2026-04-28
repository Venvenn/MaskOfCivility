using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Escalon;
using UnityEngine;

public class TileVisualsSystem : BaseSystem<World, float>
{
    private QueryDescription _desc = new QueryDescription().WithAll<GameObjectReference, CountryTileData, TileViewData>();
    private CoreManagers _coreManagers;
    
    public TileVisualsSystem(World world,  CoreManagers coreManagers) : base(world)
    {
        _coreManagers = coreManagers;
    }

    public override void Initialize()
    {
        World.Query(in _desc, (Entity entity, ref GameObjectReference gameObjectReference, ref CountryTileData tile, ref TileViewData tileView) =>
        {
            tileView.View.SetDestination(Vector3.zero, Random.Range(1f, 5));
        }); 
    }

    public override void Update(in float t)
    {
        SelectionData selectionData = _coreManagers.DataManager.Read<SelectionData>();
        PlayerData playerData = _coreManagers.DataManager.Read<PlayerData>();
        World.Query(in _desc, (Entity entity, ref GameObjectReference gameObjectReference, ref CountryTileData tile,  ref TileViewData tileView) => 
        {
            if (tile.HardHolder != Entity.Null && tile.HardHolder.TryGet<CountryData>(out var countryData))
            {
                if (gameObjectReference.Renderer.material.color != countryData.Colour)
                {
                    gameObjectReference.Renderer.material.color = countryData.Colour;
                }
                
                if (entity == selectionData.SelectedTile)
                {
                    tileView.View.SetDestination(new Vector3(0,1f, 0), 0.2f);
                }
                else if(entity == selectionData.HoveredTile)
                {
                    tileView.View.SetDestination(new Vector3(0,0.5f, 0), 0.2f);
                }
                else
                {
                    tileView.View.SetDestination(Vector3.zero, 0.1f);
                }

                if (playerData.Country == tile.HardHolder)
                {
                    tileView.View.SetResource();
                }
            }
        });  
    }
}
