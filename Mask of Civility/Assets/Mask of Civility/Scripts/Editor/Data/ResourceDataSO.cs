using Escalon;
using Unity.Mathematics;
using UnityEngine;

public class ResourceDataSO : ToolDataObject
{
    [SerializeField]
    private SerializableDictionary<ResourceTypes, float> _startModifier;
    [SerializeField]
    private SerializableDictionary<ResourceTypes, Vector2Int> _amountRange;
    
    public override IData GetData()
    {
        return new ResourceConfig()
        {
            StartModifier = _startModifier,
            AmountRange = _amountRange
        };
    }
}
