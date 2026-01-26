using System.Threading.Tasks;
using Nova;
using UnityEngine;

namespace Escalon.Unity
{
    public class InputManagerUnity : InputManager
    {
        private const uint k_mousePointerControlID = 1;
        private const uint k_scrollWheelControlID = 2;

        private Camera _camera;
        private LayerMask _layerMask = -1;
        
        public InputManagerUnity(Camera camera)
        {
            Init();
            _camera = camera;
        }
    
        protected sealed override void Init()
        {
            Notification.AddObserver<FSApplication>(RequestInput, k_requestInput);
            Notification.AddObserver<FSApplication>(SetInputMask, k_setInputMask);
        }

        protected override async Task RequestInput(object sender, object args)
        {
            switch (args)
            {
                case IInputContext popupInput:
                {
                    await popupInput.ExecuteInput(Container.GetAspect<CoreManagers>());
                    break;
                }
            }
        }
        
        private void SetInputMask(object sender, object args)
        {
            switch (args)
            {
                case LayerMask layerMask:
                {
                    _layerMask = layerMask;
                    break;
                }
                case string layerMaskName:
                {
                    _layerMask = LayerMask.GetMask(layerMaskName);
                    break;
                }
                case int layerMaskId:
                {
                    _layerMask = layerMaskId;
                    break;
                }
            }
        }

        public override void ProcessCommands()
        {
            while (QueueCount > 0)
            {
                ICommandInput command = PopInputCommand();
                switch (command)
                {
                    case MouseCommand mouseCommand:
                    {
                        ProcessMouseCommand(mouseCommand);
                        break;
                    }
                }
            }
        }

        private void ProcessMouseCommand(MouseCommand mouseCommand)
        {
            // Get the current world-space ray of the mouse
            Ray mouseRay = _camera.ScreenPointToRay(mouseCommand.MousePosition);

            // Get the current scroll wheel delta
            Vector2 mouseScrollDelta = mouseCommand.MouseScrollDelta;

            if (mouseScrollDelta != Vector2.zero)
            {
                // Invert scrolling for a mouse-type experience,
                // otherwise will scroll track-pad style.
                if (true)
                {
                    mouseScrollDelta.y *= -1f;
                }

                // Create a new Interaction.Update from the mouse ray and scroll wheel control id
                Interaction.Update scrollInteraction = new Interaction.Update(mouseRay, k_scrollWheelControlID);

                // Feed the scroll update and scroll delta into Nova's Interaction APIs
                Interaction.Scroll(scrollInteraction, mouseScrollDelta);
            }

            // Create a new Interaction.Update from the mouse ray and pointer control id
            Interaction.Update pointInteraction = new Interaction.Update(mouseRay, k_mousePointerControlID);

            // Feed the pointer update and pressed state to Nova's Interaction APIs
            Interaction.Point(pointInteraction,  mouseCommand.LeftMouseButtonValue, layerMask:_layerMask);

            // if (mouseCommand.LeftMouseButtonUp)
            // {
            //     // If the mouse button was released this frame, fire the OnPostClick
            //     // event with the hit UIBlock (or null if there wasn't one)
            //     Interaction.TryGetActiveReceiver(k_mousePointerControlID, out UIBlockHit hit);
            //     OnPostClick?.Invoke(hit.UIBlock);
            // }
        }
    }
}



