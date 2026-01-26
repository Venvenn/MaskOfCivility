using System.Threading.Tasks;

namespace Escalon
{
    /// <summary>
    /// A handler for an invokable message response that can be awaited 
    /// </summary>
    public readonly struct AwaitableHandler : IHandlerWrapper
    {
        private readonly NotificationReceiver.AwaitableHandler _handler;

        public AwaitableHandler(NotificationReceiver.AwaitableHandler handler)
        {
            _handler = handler;
        }
    
        public async Task Execute(object sender, object args)
        {
            await _handler(sender, args);
        }
    }
}

