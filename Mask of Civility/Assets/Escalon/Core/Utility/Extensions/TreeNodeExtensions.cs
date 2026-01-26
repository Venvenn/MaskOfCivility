using System.Collections.Generic;
using System.Linq;

namespace Escalon
{
    public static class TreeNodeExtensions 
    {
        public static IEnumerable<T> Values<T>(this IEnumerable<TreeNode<T>> nodes)
        {
            return nodes.Select(n => n.Value);
        }
    }
}
