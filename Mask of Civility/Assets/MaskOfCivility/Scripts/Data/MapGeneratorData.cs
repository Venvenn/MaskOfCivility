
using Unity.Mathematics;
using UnityEngine;

public struct MapGeneratorData : IData
{
    public int2 Size;
    public float SeaLevel;
    public int CountryCount;
    public GameObject MapTile;
}

