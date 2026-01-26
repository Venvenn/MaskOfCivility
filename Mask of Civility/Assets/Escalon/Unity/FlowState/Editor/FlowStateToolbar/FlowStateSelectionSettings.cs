using UnityEngine;

[CreateAssetMenu(menuName = "Escalon/FlowStateSelectionSettings",fileName = "FlowStateSelectionSettings")]
public class FlowStateSelectionSettings : ScriptableObject
{
    public string SelectedEntryFlowState;
    public string[] ValidStatesForEntry;
}
