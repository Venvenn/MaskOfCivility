using Escalon;
using UnityEngine;

public class ActionDataSO : ToolDataObject
{
    [SerializeField]
    private TileActionsData _actionData;
    
    public override IData GetData()
    {
        return _actionData;
    }
}
