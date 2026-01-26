using System;
using Arch.Core;
using Newtonsoft.Json;

namespace Escalon
{
    [Serializable]
    public struct EnvironmentData 
    {
        [JsonIgnore]
        public TreeNode<Entity> Environment;
    }
}