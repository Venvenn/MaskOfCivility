using System.Collections.Generic;
using Arch.Core;
using Escalon.Traits;

namespace Escalon
{
    /// <summary>
    /// A system for interacting with modifiable values attached to environments
    /// </summary>
    public static class ActionValueSystem
    {
        public static void Add(ActionValuePacket packet, EntityManager  entityManager)
        {
            Add(packet.Entity, packet.ActionValue, packet.Modifier, packet.ActionValueType, entityManager);
        }

        public static void Add(Entity entity, CompoundActionValue actionValue, StatChange modifier, ActionValueType type, EntityManager  entityManager)
        {
            ref ActionValueData actionValueData = ref entityManager.GetComponentReadWrite<ActionValueData>(entity);
            actionValueData.Get(type).TryAdd(actionValue.BaseActionValue, new EnvironmentEvaluator());
            actionValueData.Add(type, actionValue.BaseActionValue, modifier);
        }

        public static void Remove(Entity entity, ActionValue actionValue, StatChange modifier,
            ActionValueType type, CoreManagers coreManagers)
        {
            ref ActionValueData actionValueData = ref coreManagers.EntityManager.GetComponentReadWrite<ActionValueData>(entity);
            if (actionValueData.Get(type).ContainsKey(actionValue))
            {
                actionValueData.Remove(type, actionValue, modifier);
            }
        }

        public static void Remove(Entity entity, ActionValue actionValue, ActionValueCompoundCalculation compound,
            ActionValueType type, CoreManagers coreManagers)
        {
            ref ActionValueData actionValueData = ref coreManagers.EntityManager.GetComponentReadWrite<ActionValueData>(entity);
            if (actionValueData.Get(type).ContainsKey(actionValue))
            {
                actionValueData.Remove(type, actionValue, compound);
            }
        }

        public static void Clear(Entity entity, ActionValue actionValue, ActionValueType type, CoreManagers coreManagers, float newBaseValue = 0)
        {
            ref ActionValueData actionValueData = ref coreManagers.EntityManager.GetComponentReadWrite<ActionValueData>(entity);
            if (actionValueData.Get(type).ContainsKey(actionValue))
            {
                actionValueData.Get(type)[actionValue].ClearModifiers(newBaseValue);
            }
        }

        public static double GetActionValue(Entity entity, CompoundActionValue actionValue, CoreManagers coreManagers, double baseValue = 0)
        {
            ExternalDataMap environmentDataMap = coreManagers.DataManager.Read<ExternalDataMap>();
            TreeNode<Entity> environmentNode = coreManagers.EntityManager.GetComponent<EnvironmentData>(entity).Environment;
            ActionValueData actionValueData = coreManagers.EntityManager.GetComponent<ActionValueData>(entity);

            if (environmentDataMap.ExternalMapping.TryGetValue(actionValue, out var value))
            {
                baseValue = value.Invoke(coreManagers.DataManager);
            }
            else if (int.TryParse(actionValue, out int resulInt))
            {
                baseValue = resulInt;
            }
            else if (float.TryParse(actionValue, out float resultFloat))
            {
                baseValue = resultFloat;
            }
            else
            {
                if (actionValueData.Values.TryGetValue(actionValue.BaseActionValue, out EnvironmentEvaluator flatValue))
                {
                    baseValue = flatValue.GetValue(baseValue);
                    baseValue = EvaluateCompound(flatValue.GetCompoundCalculations(), entity, coreManagers, baseValue);
                }

                foreach (TreeNode<Entity> environment in environmentNode.SelfAndAncestors)
                {
                    ActionValueData actionData = coreManagers.EntityManager.GetComponent<ActionValueData>(environment.Value);
                    if (actionData.Modifiers.TryGetValue(actionValue.BaseActionValue,
                            out EnvironmentEvaluator modifiers))
                    {
                        baseValue = modifiers.GetValue(baseValue);
                        baseValue = EvaluateCompound(modifiers.GetCompoundCalculations(), entity, coreManagers, baseValue);
                    }
                }
            }

            if (actionValue.CompoundCalculations?.Count > 0)
            {
                baseValue = EvaluateCompound(actionValue.CompoundCalculations, entity, coreManagers, baseValue);
            }

            return baseValue;
        }

        private static double EvaluateCompound(List<ActionValueCompoundCalculation> calculations, Entity entity, CoreManagers coreManagers, double baseValue = 0)
        {
            foreach (ActionValueCompoundCalculation calculation in calculations)
            {
                double value = GetActionValue(entity, calculation.ActionValue, coreManagers);

                switch (calculation.Type)
                {
                    case ArithmeticOperatorType.Add:
                        baseValue += value;
                        break;
                    case ArithmeticOperatorType.Subtract:
                        baseValue -= value;
                        break;
                    case ArithmeticOperatorType.Divide:
                        baseValue /= value;
                        break;
                    case ArithmeticOperatorType.Multiply:
                        baseValue *= value;
                        break;
                }
            }

            return baseValue;
        }
    }
}