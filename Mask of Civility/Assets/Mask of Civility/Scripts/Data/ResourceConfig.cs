using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct ResourceConfig : IData
{
    public Dictionary<ResourceType, float> StartModifier;
    public Dictionary<ResourceType, Vector2Int> AmountRange;
}
