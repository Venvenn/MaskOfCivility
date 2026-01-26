using System.Threading.Tasks;

namespace Escalon
{
    public interface IHandlerWrapper
    {
        Task Execute(object sender, object args);
    }
}
