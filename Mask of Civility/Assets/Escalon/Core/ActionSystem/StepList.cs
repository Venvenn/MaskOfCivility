
using System;

namespace Escalon.ActionSystem
{
    [Serializable]
    public class StepList<T> : NonBoxingList<T> where T : IStep
    {
    }
}
