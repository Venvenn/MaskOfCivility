using System.Collections.Generic;
using UnityEngine;

namespace Escalon
{
    /// <summary>
    /// Functions for sampling a BezierCurve. For now it is static because it doesn't need any state.
    /// </summary>
    public static class BezierSamplerBatched
    {
        /// <summary>
        /// Samples equally spaced in interpolation space.
        /// </summary>
        /// <param name="bc"></param>
        /// <param name="sampleCount"></param>
        /// <returns></returns>
        public static Vector3[] InterpolateSample(BezierCurveCubic bc, int sampleCount)
        {
            Vector3[] samples = new Vector3[sampleCount];
            for(int i = 0; i < sampleCount; ++i)
            {
                samples[i] = bc.Compute(i / (float)sampleCount);
            }

            return samples;
        }

        /// <summary>
        /// Samples equally spaced along the curve. 
        /// <paramref name="sampleDistance"/> defines how far the samples should try to be on the curve.
        /// <paramref name="sampleCount"/> defines how many samples we do between the sample count for accuracy.
        /// </summary>
        /// <param name="bc"></param>
        /// <param name="sampleDistance"></param>
        /// <param name="sampleCount"></param>
        /// <returns></returns>
        public static List<Vector3> ApproximateCurveSample (BezierCurveCubic bc, float sampleDistance, int sampleCount=100, bool includeEnd=false)
        {
            List<Vector3> samples = new List<Vector3>();

            for(int i = 0; i < sampleCount; ++i)
            {
                Vector3 p0 = bc.Compute(i / (float)sampleCount);

                if (samples.Count == 0)
                    samples.Add(p0);
                else
                {
                    float d = Vector3.Distance(samples[samples.Count - 1], p0);
                    if (d >= sampleDistance)
                        samples.Add(p0);
                    else if (i == sampleCount - 1 && includeEnd)
                        samples.Add(p0);
                }
            }

            return samples;
        }

        public static void ApproximateCurveSingleSample (
            out Vector3 nextPos, 
            out float nextTime, 
            BezierCurveCubic bc, 
            float previousT, 
            float sampleTimeDistance,
            float stopPosDistance, 
            int sampleCount=10)
        {
            Vector3 pT = bc.Compute(previousT);
            float dt = sampleTimeDistance / sampleCount;

            nextTime = previousT;
            nextPos = Vector3.zero;
            for(int i = 0; i < sampleCount; ++i)
            {
                nextTime = previousT + i * dt;
                nextPos = bc.Compute(nextTime);

                float d = Vector3.Distance(nextPos, pT);
                if (d >= stopPosDistance)
                    return;
            }
        }
    }
}
