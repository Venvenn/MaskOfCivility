using Nova;
using UnityEngine;
#if !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
#endif

namespace Escalon.Unity
{
    /// <summary>
    /// Simple input manager example for the UI Controls sample.
    /// </summary>
    public class NovaInputProcessor : Aspect, IUpdateable
    {
        private InputManagerUnity _inputManager;

        public NovaInputProcessor(InputManagerUnity inputManager)
        {
            _inputManager = inputManager;
        }

        public void Update(float dt)
        {
            UpdateMouse();
        }

        #region Mouse
#if ENABLE_LEGACY_INPUT_MANAGER
        private bool MousePresent => Input.mousePresent;
        private Vector2 MousePosition => Input.mousePosition;
        private Vector2 MouseScrollDelta => Input.mouseScrollDelta;
        private bool LeftMouseButtonValue => Input.GetMouseButton(0);
        private bool LeftMouseButtonUp => Input.GetMouseButtonUp(0);
        private bool RightMouseButtonValue => Input.GetMouseButton(1);
        private bool RightMouseButtonUp => Input.GetMouseButtonUp(1);
#else
        private bool MousePresent => Mouse.current != null;
        private Vector2 MousePosition => Mouse.current.position.ReadValue();
        private Vector2 MouseScrollDelta => Mouse.current.scroll.ReadValue().normalized;
        private bool LeftMouseButtonValue => Mouse.current.leftButton.isPressed;
        private bool LeftMouseButtonUp => Mouse.current.leftButton.wasReleasedThisFrame;
        private bool RightMouseButtonValue => Mouse.current.rightButton.isPressed;
        private bool RightMouseButtonUp => Mouse.current.rightButton.wasReleasedThisFrame;
#endif

        private void UpdateMouse()
        {
            if (!MousePresent)
            {
                return;
            }

            MouseCommand mouseCommand = new MouseCommand()
            {
                MousePosition = MousePosition,
                MouseScrollDelta = MouseScrollDelta,
                LeftMouseButtonValue = LeftMouseButtonValue,
                LeftMouseButtonUp = LeftMouseButtonUp,
                RightMouseButtonValue = RightMouseButtonValue,
                RightMouseButtonUp = RightMouseButtonUp,
            };
            
            _inputManager.PushInputCommand(mouseCommand);
        }
        #endregion
    }
}

