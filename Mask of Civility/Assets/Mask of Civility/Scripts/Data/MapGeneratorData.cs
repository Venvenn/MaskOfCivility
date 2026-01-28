
using System;
using UnityEngine;

[Serializable]
public struct MapGeneratorData : IData
{
    public Vector2Int Size;
    public float SeaLevel;
    public int CountryCount;
}

[Serializable]
public struct MapGeneratorDynamicData : IData
{
    public TileView Prefab;
}