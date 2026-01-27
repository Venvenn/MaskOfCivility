
namespace Escalon
{
    /// <summary>
    /// Core set of manager used to interact with data
    /// </summary>
    public class CoreManagers : Aspect
    {
        public readonly EntityManager EntityManager;
        public readonly DataManager DataManager;

        public CoreManagers(EntityManager entityManager, DataManager dataManager)
        {
            EntityManager = entityManager;
            DataManager = dataManager;
        }
    }
}
