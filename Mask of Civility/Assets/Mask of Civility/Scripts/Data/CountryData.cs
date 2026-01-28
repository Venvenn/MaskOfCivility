
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public struct CountryData
{
    public string Name;
    public Color Colour;
    public int2 OriginPoint;
    public float Mask;
    public Dictionary<ResourceType, int> ResourceAmounts;
}
