
using Arch.Core;
using Arch.Core.Extensions;
using Escalon;
using UnityEngine;

public static class MapGeneratorSystem 
{
    public static Entity Generate(CoreManagers coreManagers)
    {
        Entity map = coreManagers.EntityManager.CreateEntity();

        MapGeneratorData mapGeneratorData = coreManagers.DataManager.Read<MapGeneratorData>();
        NoiseSettings noiseSettings = coreManagers.DataManager.Read<NoiseSettings>();

        float[,] noiseMap = HeightMapGenerator.GenerateNoiseMap(mapGeneratorData.Size.x, mapGeneratorData.Size.y, noiseSettings, Vector2.zero);
        float[,] falloffMap = FalloffGenerator.GenerateFalloffMap(mapGeneratorData.Size);
        MapData mapData = new MapData()
        {
            Tiles = new Entity[mapGeneratorData.Size.x, mapGeneratorData.Size.y]
        };
        GameObjectReference mapReference = new GameObjectReference()
        {
            GameObject = new GameObject("Map")
        };
        map.Add(mapReference);
        
        for (int y = 0; y < mapGeneratorData.Size.y; y++)
        {
            for (int x = 0; x < mapGeneratorData.Size.x; x++)
            {
                Entity entity = coreManagers.EntityManager.CreateEntity();
                
                double height = noiseMap[x, y] - falloffMap[x, y];
  
                
                if (height > mapGeneratorData.SeaLevel)
                {
                    GameObject tile = Object.Instantiate(mapGeneratorData.MapTile, mapReference.GameObject.transform);
                    mapData.Tiles[x, y] = entity;
                    tile.transform.position = new Vector3(x,(float)height,y);
                    GameObjectReference reference = new GameObjectReference()
                    {
                        GameObject = tile
                    };
                    entity.Add(reference);

                    CountryTileData countryTileData = new CountryTileData();
                    entity.Add(countryTileData);
                    
                }
            }
        }
        
        map.Add(mapData);
        return map;
    }
}
