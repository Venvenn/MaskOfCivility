using System.Threading.Tasks;

namespace Escalon
{
    /// <summary>
    /// The specific context relating to an input
    /// </summary>
    public interface IInputContext 
    {
        Task ExecuteInput(CoreManagers coreManagers);
    }
}
