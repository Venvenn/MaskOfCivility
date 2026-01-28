
using UnityEngine;

public class NoiseDataSO : ToolDataObject
{
    [SerializeField]
    private NoiseSettings _noiseSettings;
    public override IData GetData()
    {
        return _noiseSettings;
    }
}
