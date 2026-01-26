using System;
using UnityEngine;

namespace Escalon
{
    [Serializable]
    public class BezierCurveCubic
    {
        [SerializeField] Vector3 p0 = Vector3.zero;
        [SerializeField] Vector3 p1 = Vector3.zero;
        [SerializeField] Vector3 p2 = Vector3.zero;
        [SerializeField] Vector3 p3 = Vector3.zero;

        public Vector3 P0 { get => p0; set => p0 = value; }
        public Vector3 P1 { get => p1; set => p1 = value; }
        public Vector3 P2 { get => p2; set => p2 = value; }
        public Vector3 P3 { get => p3; set => p3 = value; }

        public BezierCurveCubic(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }

        public Vector3 Compute(float t)
        {
            return Mathf.Pow(1 - t, 3) * p0 + 3 * Mathf.Pow(1 - t, 2) * t * p1 + 3 * (1 - t) * Mathf.Pow(t, 2) * p2 + Mathf.Pow(t, 3) * p3;
        }
    }
}
