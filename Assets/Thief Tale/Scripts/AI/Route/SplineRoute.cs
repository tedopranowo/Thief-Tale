//Spline.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using System;
using UnityEngine;

namespace ThiefTale
{
    public class SplineRoute : Route
    {
        #region fields=================================================================================
        [Tooltip("The route of the spline")]
        [SerializeField]
        private BezierPoint[] m_points = new BezierPoint[0];

        [Tooltip("How many section does each spline has?" +
            "\n Decreasing the value make the spline smoother, but makes the performance worse")]
        [Range(0, 1)]
        [SerializeField]
        private float m_splineInterval = 0.1f;

        #endregion

        #region properties=============================================================================
        public BezierPoint[] points
        {
            set
            {
                m_points = value;
            }
            get
            {
                return m_points;
            }
        }
        #endregion

        #region methods================================================================================
        /// <summary>
        /// Add a new bezier curve to the spline
        /// </summary>
        public void AddCurve()
        {
            //Resize the array
            Array.Resize(ref m_points, m_points.Length + 1);

            BezierPoint newPoint = (m_points.Length != 0) ? new BezierPoint(m_points[m_points.Length - 2]) : new BezierPoint();

            newPoint.localPosition += Vector3.right;
            newPoint.startTangent = newPoint.localPosition + Vector3.down;
            newPoint.endTangent = newPoint.localPosition + Vector3.up;

            m_points[m_points.Length - 1] = newPoint;
        }

        /// <summary>
        /// Return how many points exist in the route
        /// </summary>
        /// <returns></returns>
        public override int GetLength()
        {
            return Mathf.Max(0, (points.Length - 1) * (int)(1 / m_splineInterval) + 1);
        }

        /// <summary>
        /// Return the point at index t
        /// </summary>
        /// <param name="t"> The index of the point (t must be less than length) </param>
        public override Vector3 GetPoint(int t)
        {
            float splineSection = t * m_splineInterval;

            return CalculatePoint(splineSection);
        }

        /// <summary>
        /// Calculate the position of a point at interpolated index
        /// </summary>
        /// <param name="t"> The interpolation index </param>
        /// <returns></returns>
        private Vector3 CalculatePoint(float t)
        {
            int i = (int)t;
            t = t - i;

            if (t == m_points.Length - 1)
                return points[i].localPosition;

            return transform.TransformPoint(CalculatePoint(
                points[i].localPosition, points[i + 1].localPosition, points[i].endTangent, points[i + 1].startTangent, t));
        }

        /// <summary>
        /// Calculates a point in the bezier curves
        /// </summary>
        /// <param name="startPos"> The start position</param>
        /// <param name="endPos"> The end position </param>
        /// <param name="startTangent"> The start tangent </param>
        /// <param name="endTangent"> The end tangent </param>
        /// <param name="t"> The interpolation value (0 - 1)</param>
        /// <returns></returns>
        private Vector3 CalculatePoint(Vector3 startPos, Vector3 endPos, Vector3 startTangent, Vector3 endTangent, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1 - t;

            return oneMinusT * oneMinusT * oneMinusT * startPos +
                3.0f * oneMinusT * oneMinusT * t * startTangent +
                3.0f * oneMinusT * t * t * endTangent +
                t * t * t * endPos;
        }
        #endregion
    }

}