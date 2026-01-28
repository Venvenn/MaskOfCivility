using Escalon;
using Unity.Mathematics;
using UnityEngine;

public class ResourceDataSO : ToolDataObject
{
    [SerializeField]
    private SerializableDictionary<ResourceType, float> _startModifier;
    [SerializeField]
    private SerializableDictionary<ResourceType, Vector2Int> _amountRange;
    
    public override IData GetData()
    {
        return new ResourceConfig()
        {
            StartModifier = _startModifier,
            AmountRange = _amountRange
        };
    }
}
