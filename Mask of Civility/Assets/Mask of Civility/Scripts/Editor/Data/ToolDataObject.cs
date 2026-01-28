using Escalon;
using UnityEngine;

public abstract class ToolDataObject :  ScriptableObject, IEditorGameData
{
    public abstract IData GetData();

    public virtual void Delete()
    {
    }

    public virtual void Duplicate(string folderPath, string newFilePath)
    {
    }
}
