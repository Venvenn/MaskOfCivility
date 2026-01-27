using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;

public class TileVisualsSystem : BaseSystem<World, float>
{
    private QueryDescription _desc = new QueryDescription().WithAll<GameObjectReference, CountryTileData>();
    
    public TileVisualsSystem(World world) : base(world)
    {
    }

    public override void Update(in float t)
    {
        World.Query(in _desc, (ref GameObjectReference gameObjectReference, ref CountryTileData tile) => 
        {
            if (tile.HardHolder != Entity.Null && tile.HardHolder.TryGet<CountryData>(out var countryData))
            {
                if (gameObjectReference.Renderer.material.color != countryData.Colour)
                {
                    gameObjectReference.Renderer.material.color = countryData.Colour;
                }
            }
        });  
    }
}
