using UnityEngine;

namespace Escalon
{
    public static class Statistics
    {
        public static float RandomApproxCornerDistributionSample(float min, float max)
        {
            float v1 = Random.Range(min, max);
            float v2 = Random.Range(min, max);
            return (v1 + v2) / 2.0f;
        }

        public static int RandomApproxCornerDistributionSample(int min, int max)
        {
            int v1 = Random.Range(min, max);
            int v2 = Random.Range(min, max);
            return (v1 + v2) / 2;
        }

        public static int RandomApproxSlopeDistributionSample(int min, int max)
        {
            int v1 = Random.Range(min - (max - min), max);
            int v2 = Random.Range(min - (max - min), max);
            int v3 =  (v1 + v2) / 2;
            if(v3 < min)
                v3 += (max - min);
            return v3;
        }

        public static float RandomApproxNormalDistributionSample(float min, float max)
        {
            float v1 = Random.Range(min, max);
            float v2 = Random.Range(min, max);
            float v3 = Random.Range(min, max);
            return (v1 + v2 + v3) / 3.0f;
        }
    }
}
