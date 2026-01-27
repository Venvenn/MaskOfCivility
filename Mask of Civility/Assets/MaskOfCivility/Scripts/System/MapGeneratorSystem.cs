
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Arch.Core;
using Arch.Core.Extensions;
using Escalon;
using NUnit.Framework.Internal;
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
        
        MapGeneratorData mapGeneratorData = coreManagers.DataManager.Read<MapGeneratorData>();
        Texture2D texture = new Texture2D(mapGeneratorData.Size.x, mapGeneratorData.Size.y, TextureFormat.RGB24, false);
        MapData mapData = map.Get<MapData>();
        for (int y = 0; y < mapGeneratorData.Size.y; y++)
        {
            for (int x = 0; x < mapGeneratorData.Size.x; x++)
            {
                if (mapData.Tiles[x, y] != Entity.Null)
                {
                    Entity holder = mapData.Tiles[x, y].Get<CountryTileData>().HardHolder;
                    if (holder != Entity.Null)
                    {
                        if (holder.TryGet<CountryData>(out var countryData))
                        {
                            Color colour = countryData.Colour;
                            texture.SetPixel(x,y, colour);
                        }
                        else
                        {
                            Debug.Log(holder.ToString());
                        }
                    }
                    else
                    {
                        texture.SetPixel(x,y, Color.white);
                    }
          
                }
                else
                {
                    texture.SetPixel(x,y, Color.blue);
                }
            }
        }
        byte[] bytes = texture.EncodeToPNG();
        var dirPath = Application.dataPath + "/../SaveImages/";
        if(!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        await File.WriteAllBytesAsync(dirPath + "Image" + ".png", bytes);
        Debug.Log("Done");
        return map;
    }

    private static void GenerateLand(Entity map, CoreManagers coreManagers)
    {
        MapGeneratorData mapGeneratorData = coreManagers.DataManager.Read<MapGeneratorData>();
        NoiseSettings noiseSettings = coreManagers.DataManager.Read<NoiseSettings>();

        float[,] noiseMap = HeightMapGenerator.GenerateNoiseMap(mapGeneratorData.Size.x, mapGeneratorData.Size.y,
            noiseSettings, Vector2.zero);
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

                float height = noiseMap[x, y] - falloffMap[x, y];

                if (height > mapGeneratorData.SeaLevel)
                {
                    GameObject tile = Object.Instantiate(mapGeneratorData.MapTile, mapReference.GameObject.transform);
                    mapData.Tiles[x, y] = entity;
                    tile.transform.position = new Vector3(x, height, y);
                    GameObjectReference reference = new GameObjectReference()
                    {
                        GameObject = tile,
                        Renderer = tile.GetComponent<MeshRenderer>()
                    };
                    entity.Add(reference);

                    CountryTileData countryTileData = new CountryTileData()
                    {
                        Height = height,
                        HardHolder = Entity.Null,
                        SoftHolder = Entity.Null
                    };
                    entity.Add(countryTileData);
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
                Colour = color
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
            tasks[i] = Task.Run( () => Flood(mapData.Tiles,claims, mapData.Countries[index].Get<CountryData>().OriginPoint, mapGeneratorData.Size, mapData.Countries[index], coreManagers.EntityManager));
        }
        await Task.WhenAll(tasks);

        // for (int y = 0; y < mapGeneratorData.Size.y; y++)
        // {
        //     for (int x = 0; x < mapGeneratorData.Size.x; x++)
        //     {
        //
        //         Entity tile = mapData.Tiles[x, y];
        //         if (tile != Entity.Null)
        //         {
        //        
        //             int id = (y * mapGeneratorData.Size.x) + x;
        //             CountryTileData countryTileData = tile.Get<CountryTileData>();
        //             countryTileData.HardHolder = claims[id];
        //             countryTileData.SoftHolder = claims[id];
        //             tile.Set(countryTileData);
        //             await Task.Delay(100);
        //         }
        //     }
        // }

        map.Set(mapData);
    }
    
    public static async Task Flood(Entity[,] tiles, Entity[] claim, int2 point, int2 size, Entity entity, EntityManager entityManager)
    {
        int id = point.y * size.x + point.x;
        if (tiles[point.x, point.y] != Entity.Null && point.x >= 0 && point.x < size.x && point.y >= 0 && point.y < size.y)
        {
            if (ValidTarget())
            {
                System.Random random = new  System.Random();
                claim[id] = entity;
                Task[]tasks = new Task[8];
                await SetTileData();

                if (random.Next() >0.5f)
                {
                    tasks[0] = Task.Run(()=>Flood(tiles, claim, new int2(point.x + 1, point.y), size, entity, entityManager));
                }
                if (random.Next() >0.5f)
                {
                    tasks[1] = Task.Run(()=> Flood(tiles, claim, new int2(point.x - 1, point.y), size, entity, entityManager));
                }
                if (random.Next() >0.5f)
                {
                    tasks[2] = Task.Run(()=> Flood(tiles, claim, new int2(point.x, point.y + 1), size, entity, entityManager));
                }
                if (random.Next() >0.5f)
                {
                    tasks[3] = Task.Run(()=> Flood(tiles, claim,new int2(point.x, point.y - 1), size, entity, entityManager));
                }
                if (random.Next() >0.5f)
                {
                    tasks[4] = Task.Run(()=> Flood(tiles, claim, new int2(point.x + 1, point.y+1), size, entity, entityManager));
                }
                if (random.Next() >0.5f)
                {
                    tasks[5] = Task.Run(()=> Flood(tiles, claim, new int2(point.x - 1, point.y-1), size, entity, entityManager));
                }
                if (random.Next() >0.5f)
                {
                    tasks[6] = Task.Run(()=> Flood(tiles, claim, new int2(point.x-1, point.y + 1), size, entity, entityManager));
                }
                if (random.Next() >0.5f)
                {
                    tasks[7] = Task.Run(()=> Flood(tiles, claim,new int2(point.x+1, point.y - 1), size, entity, entityManager));
                }
            }
        }

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
            await Task.Delay(200);
        }
    }
}
