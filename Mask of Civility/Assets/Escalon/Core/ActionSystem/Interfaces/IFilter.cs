using System.Collections.Generic;
using Arch.Core;

namespace Escalon.ActionSystem
{
    public interface IFilter
    {
        void Filter(ref List<Entity> targets, ActiveSequence sequence, CoreManagers coreManagers);
    }
}

