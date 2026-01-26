using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Escalon
{
    public static class TypeLoaderExtensions
    {
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public static IEnumerable<Type> GetTypesWithInterface(this Assembly assembly, Type interfaceType,
            bool includeBaseInterface = false)
        {
            return assembly.GetLoadableTypes().Where(interfaceType.IsAssignableFrom)
                .Where(t => includeBaseInterface || !t.IsInterface);
        }
    }
}
