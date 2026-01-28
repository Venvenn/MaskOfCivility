
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Arch.Core;
using Arch.Core.Extensions;
using Escalon;
using Unity.Mathematics;
using UnityEngine;
using Debug = Escalon.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class MapGeneratorSystem 
{
    public static async Task<Entity> Generate(CoreManagers coreManagers)
    {
        Entity map = coreManagers.EntityManager.CreateEntity();
        GenerateLand(map, coreManagers);
        await GenerateCountries(map, coreManagers);
        
        MapData mapData = map.Get<MapData>();
        PlayerData playerData = new PlayerData()
        {
            Country = mapData.Countries.Random(new System.Random())
        };
        coreManagers.DataManager.Write(playerData);
        
        // MapGeneratorData mapGeneratorData = coreManagers.DataManager.Read<MapGeneratorData>();
        // Texture2D texture = new Texture2D(mapGeneratorData.Size.x, mapGeneratorData.Size.y, TextureFormat.RGB24, false);
        // MapData mapData = map.Get<MapData>();
        // for (int y = 0; y < mapGeneratorData.Size.y; y++)
        // {
        //     for (int x = 0; x < mapGeneratorData.Size.x; x++)
        //     {
        //         if (mapData.Tiles[x, y] != Entity.Null)
        //         {
        //             Entity holder = mapData.Tiles[x, y].Get<CountryTileData>().HardHolder;
        //             if (holder != Entity.Null)
        //             {
        //                 if (holder.TryGet<CountryData>(out var countryData))
        //                 {
        //                     Color colour = countryData.Colour;
        //                     texture.SetPixel(x,y, colour);
        //                 }
        //                 else
        //                 {
        //                     Debug.Log(holder.ToString());
        //                 }
        //             }
        //             else
        //             {
        //                 texture.SetPixel(x,y, Color.white);
        //             }
        //   
        //         }
        //         else
        //         {
        //             texture.SetPixel(x,y, Color.blue);
        //         }
        //     }
        // }
        // byte[] bytes = texture.EncodeToPNG();
        // var dirPath = Application.dataPath + "/../SaveImages/";
        // if(!Directory.Exists(dirPath))
        // {
        //     Directory.CreateDirectory(dirPath);
        // }
        // await File.WriteAllBytesAsync(dirPath + "Image" + ".png", bytes);
        Debug.Log("Done");
        return map;
    }

    private static void GenerateLand(Entity map, CoreManagers coreManagers)
    {
        MapGeneratorData mapGeneratorData = coreManagers.DataManager.Read<MapGeneratorData>();
        MapGeneratorDynamicData mapGeneratorDynamicData = coreManagers.DataManager.Read<MapGeneratorDynamicData>();
        NoiseSettings noiseSettings = coreManagers.DataManager.Read<NoiseSettings>();
        ResourceConfig resourceConfig = coreManagers.DataManager.Read<ResourceConfig>();

        float[,] noiseMap = HeightMapGenerator.GenerateNoiseMap(mapGeneratorData.Size.x, mapGeneratorData.Size.y, noiseSettings, Vector2.zero);
        float[,] falloffMap = FalloffGenerator.GenerateFalloffMap(new int2(mapGeneratorData.Size.x, mapGeneratorData.Size.y));
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

                float height = noiseMap[x, y] - falloffMap[x, y];

                if (height > mapGeneratorData.SeaLevel)
                {
                    TileView tile = Object.Instantiate(mapGeneratorDynamicData.Prefab, mapReference.GameObject.transform);
                    mapData.Tiles[x, y] = entity;
                    tile.transform.position = new Vector3(x, height, y);
                    tile.Entity = entity;
                    GameObjectReference reference = new GameObjectReference()
                    {
                        GameObject = tile.gameObject,
                        Visual = tile.View,
                        Renderer = tile.Renderer
                    };
                    entity.Add(reference);
                    
                    TileViewData tileViewData = new TileViewData()
                    {
                        View = tile,
                    };
                    entity.Add(tileViewData);
                    
                    CountryTileData countryTileData = new CountryTileData()
                    {
                        Height = height,
                        HardHolder = Entity.Null,
                        SoftHolder = Entity.Null,
                    };
                    entity.Add(countryTileData);
                    
                    ResourceType type = (ResourceType) Random.Range(0, Enum.GetNames(typeof(ResourceType)).Length);
                    Vector2Int range = resourceConfig.AmountRange[type];
                    ResourceData resourceData = new ResourceData()
                    {
                        ResourceType = type,
                        Amount = Random.Range(range.x, range.y)
                    };
                    entity.Add(resourceData);
                }
                else
                {
                    mapData.Tiles[x, y] = Entity.Null;
                }
            }
        }

        map.Add(mapData);
    }

    private static async Task GenerateCountries(Entity map, CoreManagers coreManagers)
    {
        MapGeneratorData mapGeneratorData = coreManagers.DataManager.Read<MapGeneratorData>();
        MapData mapData = map.Get<MapData>();
        mapData.Countries = new Entity[mapGeneratorData.CountryCount];
        
        List<Color> pickedColours = new List<Color>
        {
            Color.blue,
            Color.white
        };
        
        for (int i = 0; i < mapGeneratorData.CountryCount; i++)
        {
            Entity entity = coreManagers.EntityManager.CreateEntity();
            mapData.Countries[i] = entity;
            
            Color color = Color.clear;
            while (color == Color.clear)
            {
               Color newColor = Random.ColorHSV();
               if (!pickedColours.Contains(newColor))
               {
                   color = newColor;
               }
            }

            CountryData countryData = new CountryData()
            {
                Name = i.ToString(),
                Colour = color,
                ResourceAmounts = new Dictionary<ResourceType, int>()
            };

            bool found = false;
            
            while (!found)
            {
                int2 point = new int2(Random.Range(0, mapGeneratorData.Size.x), Random.Range(0, mapGeneratorData.Size.y));
                Entity tileEntity = mapData.Tiles[point.x, point.y];
                if (tileEntity != Entity.Null && tileEntity.TryGet<CountryTileData>(out var tileData) && tileData.HardHolder == Entity.Null)
                {
                    tileData.HardHolder = entity;
                    tileData.SoftHolder = entity;
                    countryData.OriginPoint = point;
                    found = true;
                    tileEntity.Set(tileData);
                }
            }

            entity.Add(countryData);
        }

        Task[] tasks = new Task[mapData.Countries.Length];
        Entity[] claims = new Entity[mapGeneratorData.Size.y * mapGeneratorData.Size.x];
        Array.Fill(claims, Entity.Null);
        for (int i = 0; i < mapData.Countries.Length; i++)
        {
            int index = i;
            tasks[i] = Task.Run( () => Flood(mapData.Tiles,claims, mapData.Countries[index].Get<CountryData>().OriginPoint, new int2(mapGeneratorData.Size.x, mapGeneratorData.Size.y), mapData.Countries[index], coreManagers.EntityManager));
        }
        await Task.WhenAll(tasks);

        map.Set(mapData);
    }
    
    public static async Task Flood(Entity[,] tiles, Entity[] claim, int2 point, int2 size, Entity entity, EntityManager entityManager)
    {
        List<Task> tasks = new List<Task>(8);
        int id = point.y * size.x + point.x;
        if (tiles[point.x, point.y] != Entity.Null && point.x >= 0 && point.x < size.x && point.y >= 0 && point.y < size.y)
        {
            if (ValidTarget())
            {
                System.Random random = new  System.Random();
                claim[id] = entity;
          
                await SetTileData();

                if (random.Next() >0.5f)
                {
                    tasks.Add(Task.Run(()=>Flood(tiles, claim, new int2(point.x + 1, point.y), size, entity, entityManager)));
                }
                if (random.Next() >0.5f)
                {
                    tasks.Add( Task.Run(()=> Flood(tiles, claim, new int2(point.x - 1, point.y), size, entity, entityManager)));
                }
                if (random.Next() >0.5f)
                {
                    tasks.Add(Task.Run(()=> Flood(tiles, claim, new int2(point.x, point.y + 1), size, entity, entityManager)));
                }
                if (random.Next() >0.5f)
                {
                    tasks.Add(Task.Run(()=> Flood(tiles, claim,new int2(point.x, point.y - 1), size, entity, entityManager)));
                }
                if (random.Next() >0.75f)
                {
                    tasks.Add(Task.Run(()=> Flood(tiles, claim, new int2(point.x + 1, point.y+1), size, entity, entityManager)));
                }
                if (random.Next() >0.75f)
                {
                    tasks.Add(Task.Run(()=> Flood(tiles, claim, new int2(point.x - 1, point.y-1), size, entity, entityManager)));
                }
                if (random.Next() >0.75f)
                {
                    tasks.Add(Task.Run(()=> Flood(tiles, claim, new int2(point.x-1, point.y + 1), size, entity, entityManager)));
                }
                if (random.Next() >0.75f)
                {
                    tasks.Add(Task.Run(()=> Flood(tiles, claim,new int2(point.x+1, point.y - 1), size, entity, entityManager)));
                }
            }
        }

        await Task.WhenAll(tasks);

        bool ValidTarget()
        {
            return claim[id] == Entity.Null && tiles[point.x, point.y] != Entity.Null;
        }

        async Task SetTileData()
        {
            CountryTileData countryTileData = entityManager.GetComponent<CountryTileData>(tiles[point.x, point.y]);
            countryTileData.HardHolder = entity;
            countryTileData.SoftHolder =entity;
            entityManager.SetComponent(tiles[point.x, point.y], countryTileData);
            await Task.Delay(50);
        }
    }
}
