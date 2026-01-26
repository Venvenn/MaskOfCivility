
using System.Threading.Tasks;
using Arch.Core;

namespace Escalon.ActionSystem
{
    /// <summary>
    /// Visual behaviour for an action
    /// </summary>
    public interface IActionVisualBehaviour
    {
        Task Execute(Entity entity);
    }
}
