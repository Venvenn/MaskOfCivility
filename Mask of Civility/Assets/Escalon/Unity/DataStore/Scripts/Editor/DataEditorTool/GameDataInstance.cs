using UnityEngine;

namespace Escalon
{
    public class GameDataInstance
    {
        public bool Include;
        public ScriptableObject DataWrapper;
        public string GUID;

        public void SetInclude(bool include)
        {
            Include = include;
        }
    }
}