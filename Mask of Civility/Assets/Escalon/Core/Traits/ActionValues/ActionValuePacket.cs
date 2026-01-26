
using Arch.Core;

namespace Escalon.Traits
{
    public struct ActionValuePacket
    {
        public Entity Entity;
        public ActionValueType ActionValueType;
        public CompoundActionValue ActionValue;
        public StatChange Modifier;
    }
}