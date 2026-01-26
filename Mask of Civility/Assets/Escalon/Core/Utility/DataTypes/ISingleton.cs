namespace Escalon.Utility
{
    /// <summary>
    /// Do not use a singleton without a very good reason for it.
    /// </summary>
    public class Singleton<T> where T : new()
    {
        /// <summary>
        /// Check to see if we're about to be destroyed.
        /// </summary>
        private static bool _shuttingDown = false;
    
        private static object _lock = new object();
        private static T _instance;

        /// <summary>
        /// Access singleton s_instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }

                    return _instance;
                }
            }
        }
    }

 
}
