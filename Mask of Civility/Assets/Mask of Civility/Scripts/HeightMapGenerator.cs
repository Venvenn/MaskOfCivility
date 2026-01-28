using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class HeightMapGenerator : MonoBehaviour
{
    public enum NormalizeMode
    {
        Local,
        Global
    }

    //2D simplex noise 
    public static float[,] GenerateNoiseMap(int width, int height, NoiseSettings settings, Vector2 sampleCentre)
    {
        //create noise map
        var noiseMap = new float[width, height];

        if (settings.Seed == 0)
        {
            settings.Seed = UnityEngine.Random.Range(0, int.MaxValue);
        }
        
        var random = new Random(settings.Seed);

        var octaveOffsets = new List<Vector2>();

        float amplitude = 1;
        float frequency = 1;
        float maxPossibleHeight = 0;

        //create noise octaves
        for (var i = 0; i < settings.Octaves; i++)
        {
            var offsetX = random.Next(-100000, 100000) + settings.Offset.X + sampleCentre.x;
            var offsetY = random.Next(-100000, 100000) - settings.Offset.Y - sampleCentre.y;
            octaveOffsets.Add(new Vector2(offsetX, offsetY));

            maxPossibleHeight += amplitude;
            amplitude *= settings.Persistence;
        }

        
        //set varibles
        var maxLocalNoiseHeight = float.MinValue;
        var minLocalNoiseHeight = float.MaxValue;
        var halfWidth = width / 2.0f;
        var halfHeight = height / 2.0f;

        //ceate a noise value for each point in noise map
        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            amplitude = 1;
            frequency = 1;
            float noiseHeight = 0;

            for (var i = 0; i < settings.Octaves; i++)
            {
                //sample simplex noise
                var sampleX = (x - halfWidth + octaveOffsets[i].x) / settings.Scale * frequency;
                var sampleY = (y - halfHeight + octaveOffsets[i].y) / settings.Scale * frequency;

                var simplexValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                noiseHeight += simplexValue * amplitude;

                //modify noise based on inputs
                amplitude *= settings.Persistence;
                frequency *= settings.Lacunarity;
            }

            if (noiseHeight > maxLocalNoiseHeight) maxLocalNoiseHeight = noiseHeight;

            if (noiseHeight < minLocalNoiseHeight) minLocalNoiseHeight = noiseHeight;

            noiseMap[x, y] = noiseHeight;

            if (settings.NormalizeMode == NormalizeMode.Global)
            {
                var normalizedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight / 0.9f);
                noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
            }
        }

        if (settings.NormalizeMode == NormalizeMode.Local)
        {
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                }
            }
        }
        
        return noiseMap;
    }
}

[Serializable]
public struct NoiseSettings : IData
{
    [SerializeField] public float Lacunarity;

    [SerializeField] public HeightMapGenerator.NormalizeMode NormalizeMode;

    [SerializeField] public int Octaves;

    [SerializeField] public System.Numerics.Vector2 Offset;

    [SerializeField] [Range(0, 1)] public float Persistence;

    [SerializeField] public float Scale;

    [SerializeField] public int Seed;

    public void ValidateValue()
    {
        Scale = Mathf.Max(Scale, 0.01f);
        Octaves = Mathf.Max(Octaves, 1);
        Lacunarity = Mathf.Max(Lacunarity, 1);
        Persistence = Mathf.Clamp01(Persistence);
    }
}
