using System.Threading.Tasks;

namespace Escalon
{
    /// <summary>
    /// A handler for an invokable message response 
    /// </summary>
    public struct StandardHandler : IHandlerWrapper
    {
        private readonly NotificationReceiver.Handler _handler;

        public StandardHandler(NotificationReceiver.Handler handler)
        {
            _handler = handler;
        }
    
        public Task Execute(object sender, object args)
        {
            _handler(sender, args);
            return Task.CompletedTask;
        }
    }
}

