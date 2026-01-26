using System.Collections.Generic;
using UnityEngine;

namespace Escalon
{
    public class EditorDataIncludedSO : ScriptableObject
    {
        public List<ScriptableObject> IncludedDataWrappers = new List<ScriptableObject>();
    }
}