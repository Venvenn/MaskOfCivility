using System.Collections.Generic;
using UnityEngine;

namespace Escalon
{
    public interface IUpdateable
    {
        void Update(float dt);
    }
    
    public static class UpdateExtensions
    {
        public static void Update(this Container container)
        {
            List<IUpdateable> aspects = container.GetUpdateableAspects();
            for (int i = aspects.Count - 1; i >= 0; i--)
            {
                aspects[i].Update(Time.deltaTime);
            }
        }
    }
}
