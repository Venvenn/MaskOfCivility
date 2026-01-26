using System.Collections.Generic;
using System.Linq;
using Arch.Core;

namespace Escalon.Traits
{
    /// <summary>
    /// System for interacting with and managing an entities environment
    /// </summary>
    public static class EnvironmentSystem
    {
        public static void AddParentEnvironment(Entity entity, Entity parentEntity, CoreManagers coreManagers)
        {
            if (parentEntity != Entity.Null)
            {
                EnvironmentData environmentData = coreManagers.EntityManager.GetComponent<EnvironmentData>(entity);
                EnvironmentData parentEnvironmentData = coreManagers.EntityManager.GetComponent<EnvironmentData>(parentEntity);
                parentEnvironmentData.Environment.Add(environmentData.Environment);
            }
        }

        public static List<Entity> GetAllEntitiesInEnvironment(Entity environmentEntity, CoreManagers coreManagers)
        {
            List<Entity> entities = new List<Entity>();
            EnvironmentData environmentData = coreManagers.EntityManager.GetComponent<EnvironmentData>(environmentEntity);

            var nodes = environmentData.Environment.Children;

            foreach (var node in nodes)
            {
                entities.Add(node.Value);
            }

            return entities;
        }

        public static Entity GetParent(Entity entity, EntityManager entityManager)
        {
            return entityManager.GetComponent<EnvironmentData>(entity).Environment.Parent.Value;
        }

        public static List<Entity> GetEntities(Entity environment, EnvironmentFilter filter, CoreManagers coreManagers, int count = -1)
        {
            EnvironmentData environmentData = coreManagers.EntityManager.GetComponent<EnvironmentData>(environment);
            
            List<Entity> entities = new List<Entity>();
            List<TreeNode<Entity>> nodes = new List<TreeNode<Entity>>();

            if ((filter & EnvironmentFilter.Self) != 0)
            {
                nodes.Add(environmentData.Environment);
            }

            if ((filter & EnvironmentFilter.Ancestors) != 0)
            {
                nodes.AddRange(environmentData.Environment.Descendants.ToList());
            }
            else if ((filter & EnvironmentFilter.Parent) != 0)
            {
                nodes.Add(environmentData.Environment.Parent);
            }

            if ((filter & EnvironmentFilter.Descendants) != 0)
            {
                nodes.AddRange(environmentData.Environment.Descendants.ToList());
            }
            else if ((filter & EnvironmentFilter.Children) != 0)
            {
                nodes.AddRange(environmentData.Environment.Children.ToList());
            }

            if (count >= 0)
            {
                nodes = nodes.Shuffle(coreManagers.DataManager.Read<RandomData>().Random);

                foreach (var node in nodes)
                {
                    if (count > 0)
                    {
                        entities.Add(node.Value);
                        count--;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                entities.AddRange(nodes.Select(node => node.Value));
            }

            return entities;
        }
    }
}