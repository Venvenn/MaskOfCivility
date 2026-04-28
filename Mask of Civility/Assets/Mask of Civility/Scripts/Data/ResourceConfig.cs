using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct ResourceConfig : IData
{
    public Dictionary<ResourceTypes, float> StartModifier;
    public Dictionary<ResourceTypes, Vector2Int> AmountRange;
}
