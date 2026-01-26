using System;

namespace Escalon
{
    [Serializable]
    public struct RandomData : IData
    {
        public int Seed;
        public Random Random;
    }
}
