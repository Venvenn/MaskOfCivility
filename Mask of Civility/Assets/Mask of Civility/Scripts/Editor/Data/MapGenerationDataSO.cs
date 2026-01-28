
using UnityEngine;

public class MapGenerationDataSO : ToolDataObject
{
    [SerializeField]
    private MapGeneratorData _generatorData;
    public override IData GetData()
    {
        return _generatorData;
    }
}
