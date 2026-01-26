using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Arch.Core;

namespace Escalon
{
    /// <summary>
    /// Simple converter for turning a class's methods into ability delegates
    /// </summary>
    public static class AbilitySystem
    {
        public delegate void AbilityEffect(Entity entity, CoreManagers coreManagers);

        public static List<AbilityEffect> CreateAbilities<T>()
        {
            Type type = typeof(T);
            var methodInfo = type.GetMethods();
            List<AbilityEffect> effects = new List<AbilityEffect>();

            foreach (var method in methodInfo)
            {
                if (IsMethodCompatibleWithDelegate<AbilityEffect>(method))
                {
                    effects.Add(Delegate.CreateDelegate(typeof(AbilityEffect), null, method) as AbilityEffect);
                }
            }

            return effects;
        }

        private static bool IsMethodCompatibleWithDelegate<T>(MethodInfo method) where T : class
        {
            Type delegateType = typeof(T);
            MethodInfo delegateSignature = delegateType.GetMethod("Invoke");

            bool parametersEqual = delegateSignature
                .GetParameters()
                .Select(x => x.ParameterType)
                .SequenceEqual(method.GetParameters()
                    .Select(x => x.ParameterType));

            return delegateSignature.ReturnType == method.ReturnType &&
                   parametersEqual;
        }
    }
}