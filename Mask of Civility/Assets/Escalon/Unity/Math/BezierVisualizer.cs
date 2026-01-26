using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Escalon
{
    public class BezierVisualizer : MonoBehaviour
    {
        [SerializeField] public BezierCurveCubic bezierCurve = null;

        [SerializeField] [Range(0.1f,2.0f)] public float uiScale = 1.0f;
        [HideInInspector]
        public bool isEditable = true;

        #region scene parameters

        enum VisualizationType
        {
            SOLID_LINE                          = 0,
            INTERPOLATION_SAMPLES               = 1,
            CURVE_SAMPLES                       = 2,
        }

        [Header("Visualization Parameters")]
        [SerializeField] int m_sampleCount = 100;
        [SerializeField] VisualizationType m_visualizationType = VisualizationType.SOLID_LINE;
        [SerializeField] float m_curveSampleSpacing = 0.05f;
        [SerializeField] bool m_curveSampleIncludeEnd = false;

        #endregion

        // UpdateProcessor is called once per frame
        void Update()
        {

        }

        private void OnDrawGizmos()
        {
            // Draw the main curve.

            Gizmos.color = Color.white;
            Vector3[] curveSamples = BezierSamplerBatched.InterpolateSample(bezierCurve, m_sampleCount);
            for(int i = 0; i < curveSamples.Length - 1; ++i)
            {
                Gizmos.DrawLine(curveSamples[i], curveSamples[i + 1]);
            }

            // Draw interpolations.

            switch (m_visualizationType)
            {
                case VisualizationType.INTERPOLATION_SAMPLES:
                    {
                        Gizmos.color = Color.yellow;
                        Vector3[] samples = BezierSamplerBatched.InterpolateSample(bezierCurve, m_sampleCount);
                        for(int i = 0; i < samples.Length; ++i)
                        {
                            Gizmos.DrawSphere(samples[i], 0.02f * uiScale);
                        }
                        break;
                    }
                case VisualizationType.CURVE_SAMPLES:
                    {
                        Gizmos.color = Color.green;
                        List<Vector3> samples = BezierSamplerBatched.ApproximateCurveSample(bezierCurve, m_curveSampleSpacing, m_sampleCount, m_curveSampleIncludeEnd);
                        for(int i = 0; i < samples.Count; ++i)
                        {
                            Gizmos.DrawSphere(samples[i], 0.02f * uiScale);
                        }
                        break;
                    }
            }

            // Draw the Bezier points.

            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(bezierCurve.P0, 0.02f * uiScale);
            Gizmos.DrawSphere(bezierCurve.P1, 0.02f * uiScale);
            Gizmos.DrawSphere(bezierCurve.P2, 0.02f * uiScale);
            Gizmos.DrawSphere(bezierCurve.P3, 0.02f * uiScale);
        }

#if UNITY_EDITOR
        #region scene
        public static void DrawScene(BezierVisualizer bv)
        {
            if(bv.isEditable)
            {
                BezierCurveCubic bc = bv.bezierCurve;
                bc.P0 = Handles.PositionHandle(bc.P0, Quaternion.identity);
                bc.P1 = Handles.PositionHandle(bc.P1, Quaternion.identity);
                bc.P2 = Handles.PositionHandle(bc.P2, Quaternion.identity);
                bc.P3 = Handles.PositionHandle(bc.P3, Quaternion.identity);
            }
        }
        #endregion
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(BezierVisualizer))]
    public class E_BezierVisualizer : Editor
    {
        BezierVisualizer bezierCurve;

        private void OnEnable()
        {
            bezierCurve = (BezierVisualizer)target;
        }

        private void OnSceneGUI()
        {
            BezierVisualizer.DrawScene(bezierCurve);
        }
    }
#endif
}
