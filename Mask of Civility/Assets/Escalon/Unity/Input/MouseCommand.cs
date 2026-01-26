using UnityEngine;

namespace Escalon.Unity
{
    public struct MouseCommand : ICommandInput
    {
        public Vector2 MousePosition;
        public Vector2 MouseScrollDelta;
        public bool LeftMouseButtonValue;
        public bool RightMouseButtonValue;
        public bool LeftMouseButtonUp;
        public bool RightMouseButtonUp;
    }
}
