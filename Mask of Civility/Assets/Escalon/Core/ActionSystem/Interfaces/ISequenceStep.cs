
using System.Threading.Tasks;

namespace Escalon.ActionSystem
{
    /// <summary>
    /// a step in an action sequence, is responsible for containing the behaviour of a step in the action sequence 
    /// </summary>
    public interface ISequenceStep : IStep
    {
        /// <summary>
        /// Plays and resolves this step in the sequence
        /// </summary>
        /// <param name="sequence"> the sequence this step is a part of</param>
        /// <param name="coreManagers"> the core managers used  interact with the data of the game</param>
        /// <returns></returns>
        Task<ActionStatus> Evaluate(ActiveSequence sequence, CoreManagers coreManagers);
    }
}