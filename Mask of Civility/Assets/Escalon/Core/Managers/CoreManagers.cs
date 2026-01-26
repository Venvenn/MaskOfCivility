
using Escalon.ActionSystem;

namespace Escalon
{
    /// <summary>
    /// Core set of manager used to interact with data
    /// </summary>
    public class CoreManagers : Aspect
    {
        public readonly EntityManager EntityManager;
        public readonly DataManager DataManager;
        public readonly ActionManager ActionManager;

        public CoreManagers(EntityManager entityManager, DataManager dataManager, ActionManager actionManager)
        {
            EntityManager = entityManager;
            DataManager = dataManager;
            ActionManager = actionManager;
        }
    }
}
