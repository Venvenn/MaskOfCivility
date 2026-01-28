using Escalon;
using UnityEngine;

public class TimeConfigSO : ToolDataObject
{
    [SerializeField]
    private TimeConfig _timeConfig;
    
    public override IData GetData()
    {
        return _timeConfig;
    }
    
}
