using System.Collections.Generic;
using System.Threading.Tasks;

namespace Escalon
{
    /// <summary>
    /// Responsible for receiving and handling input
    /// </summary>
    public abstract class InputManager : BaseManager, IUpdateable
    {
        public const string k_requestInput = "InputManager.RequestInput";
        public const string k_setInputMask = "InputManager.SetInputMask";

        private Queue<ICommandInput> _inputQueue = new Queue<ICommandInput>();
        
        public int QueueCount => _inputQueue.Count;

        protected abstract void Init();
        public abstract void ProcessCommands();
        protected abstract Task RequestInput(object sender, object args);

        public void PushInputCommand(ICommandInput command)
        {
            _inputQueue.Enqueue(command);
        }

        public ICommandInput PopInputCommand()
        {
            return _inputQueue.Dequeue();
        }

        public void Update(float dt)
        {
            ProcessCommands();
        }
    }
}
