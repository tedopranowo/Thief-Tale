//SplineEditor.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)

using UnityEditor;
using UnityEngine;

namespace ThiefTale
{
    [CustomEditor(typeof(SplineRoute))]
    public class SplineEditor : Editor
    {
        #region fields=================================================================================
        private SplineRoute m_spline;
        private int m_selectedIndex;
        private BezierPoint.Section m_selectedSection = BezierPoint.Section.kNone;

        private const float m_pickSize = 0.04f;
        #endregion

        #region methods================================================================================
        /// <summary>
        /// Draw the point as a dot into the scene
        /// </summary>
        /// <param name="index"> The index of the point in the array </param>
        /// <param name="section"> The section of the point </param>
        /// <returns> The world position of the point</returns>
        private Vector3 ShowPoint(int index, BezierPoint.Section section)
        {
            //Get the world position of the point
            Vector3 worldPos = m_spline.transform.TransformPoint(m_spline.points[index].Get(section));

            //Check if the button is selected
            Handles.color = IsSelected(index, section) ? Color.yellow : Color.cyan;
            float buttonSize = TTEditor.Config.kButtonPickSize * HandleUtility.GetHandleSize(worldPos);
            if (Handles.Button(worldPos, Quaternion.identity, buttonSize, buttonSize, Handles.DotHandleCap))
            {
                m_selectedIndex = index;
                m_selectedSection = section;
            }

            //Draw and update the position handle if neccessary
            if (IsSelected(index, section))
            {
                EditorGUI.BeginChangeCheck();
                worldPos = Handles.DoPositionHandle(worldPos, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(m_spline, "Move Point");
                    EditorUtility.SetDirty(m_spline);
                    m_spline.points[index].Set(section, m_spline.transform.InverseTransformPoint(worldPos));
                }
            }

            return worldPos;
        }

        /// <summary>
        /// Check if a point is selected by the user
        /// </summary>
        /// <param name="index"> The index of the bezier point </param>
        /// <param name="section"> The section of the bezier point </param>
        /// <returns> Return true if the point is selected </returns>
        private bool IsSelected(int index, BezierPoint.Section section)
        {
            return m_selectedIndex == index && m_selectedSection == section;
        }
        #endregion

        #region Editor=============================================================================
        private void OnEnable()
        {
            m_spline = (SplineRoute)target;
        }

        private void OnSceneGUI()
        {
            //Don't draw anything if the spline doesn't have any point
            if (m_spline.points.Length == 0)
                return;

            Vector3 p0 = ShowPoint(0, BezierPoint.Section.kLocalPosition);

            //Draw the spline
            for (int i = 1; i < m_spline.points.Length; ++i)
            {
                Vector3 p1 = ShowPoint(i - 1, BezierPoint.Section.kEndTangent);
                Vector3 p2 = ShowPoint(i, BezierPoint.Section.kStartTangent);
                Vector3 p3 = ShowPoint(i, BezierPoint.Section.kLocalPosition);

                Handles.color = Color.cyan;
                Handles.DrawLine(p0, p1);
                Handles.DrawLine(p2, p3);

                Handles.DrawBezier(p0, p3, p1, p2, Color.green, null, 2.0f);

                p0 = p3;
            }
        }

        public override void OnInspectorGUI()
        {
            //Update the serialized property
            serializedObject.Update();

            DrawDefaultInspector();

            if (GUILayout.Button("Add Curve"))
            {
                m_spline.AddCurve();
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}

