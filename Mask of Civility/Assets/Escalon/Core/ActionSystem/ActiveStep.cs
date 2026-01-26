using System.Collections.Generic;

namespace Escalon.ActionSystem
{
    public struct ActiveStep
    {
        public List<ActiveSequence> PreEffect;
        public ISequenceStep Step;
        public List<ActiveSequence> PostEffect;
    }
}
