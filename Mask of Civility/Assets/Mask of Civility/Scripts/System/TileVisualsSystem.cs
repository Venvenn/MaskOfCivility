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
            if (tile.HardHolder != Entity.Null)
            {
                tileView.View.SetResource();
            }
        }); 
    }

    public override void Update(in float t)
    {
        SelectionData selectionData = _coreManagers.DataManager.Read<SelectionData>();
        World.Query(in _desc, (Entity entity, ref GameObjectReference gameObjectReference, ref CountryTileData tile) => 
        {
            if (tile.HardHolder != Entity.Null && tile.HardHolder.TryGet<CountryData>(out var countryData))
            {
                if (gameObjectReference.Renderer.material.color != countryData.Colour)
                {
                    gameObjectReference.Renderer.material.color = countryData.Colour;
                }
                
                if (entity == selectionData.SelectedTile)
                {
                    gameObjectReference.Visual.transform.localPosition = new Vector3(0,1, 0);
                }
                else if(entity == selectionData.HoveredTile)
                {
                    gameObjectReference.Visual.transform.localPosition = new Vector3(0,0.5f, 0);
                }
                else
                {
                    gameObjectReference.Visual.transform.localPosition = new Vector3(0,0, 0);
                }
            }
        });  
    }
}
