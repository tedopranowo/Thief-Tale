//NavMeshRouteEditor.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System;

namespace ThiefTale
{
    [CustomEditor(typeof(NavMeshRoute))]
    public class NavMeshRouteEditor : Editor
    {
        #region enum===================================================================================
        private enum Mode
        {
            kDefault,
            kAddPoint
        }
        #endregion

        #region fields=================================================================================
        private Mode m_mode;
        private NavMeshRoute m_navMeshRoute;
        private int m_selectedIndex;
        #endregion

        #region methods================================================================================
        /// <summary>
        /// Draw an edittable point as a dot into the scene
        /// </summary>
        /// <param name="index"> The index of the point in the array </param>
        /// <param name="section"> The section of the point </param>
        /// <returns> The world position of the point</returns>
        private Vector3 ShowPoint(int index)
        {
            //Don't do anything if the index is not available
            if (index >= m_navMeshRoute.points.Length)
                return Vector3.zero;

            //Get the world position of the point
            Vector3 worldPos = m_navMeshRoute.transform.TransformPoint(m_navMeshRoute.points[index]);

            //Check if the button is selected
            Handles.color = IsSelected(index) ? Color.yellow : Color.cyan;
            if (DrawSelectablePoint(worldPos))
            {
                m_selectedIndex = index;
            }

            //Draw and update the position handle if neccessary
            if (IsSelected(index))
            {
                EditorGUI.BeginChangeCheck();
                worldPos = Handles.DoPositionHandle(worldPos, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(m_navMeshRoute, "Move Point");
                    EditorUtility.SetDirty(m_navMeshRoute);
                    m_navMeshRoute.points[index] = m_navMeshRoute.transform.InverseTransformPoint(worldPos);
                }
            }

            return worldPos;
        }

        /// <summary>
        /// Draw a point into the scene
        /// </summary>
        /// <param name="point"> Where the point is drawn </param>
        /// <returns>Return true if the point is selected</returns>
        private bool DrawSelectablePoint(Vector3 point)
        {
            float buttonSize = TTEditor.Config.kButtonPickSize * HandleUtility.GetHandleSize(point);
            if (Handles.Button(point, Quaternion.identity, buttonSize, buttonSize, Handles.DotHandleCap))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Return true if the point in that index is selected by the user
        /// </summary>
        private bool IsSelected(int index)
        {
            return m_selectedIndex == index;
        }

        /// <summary>
        /// Draw the nav mesh path from one point to another
        /// </summary>
        /// <returns> Returns true if path to p1 exist, otherwise return false</returns>
        private bool DrawPath(Vector3 p0, Vector3 p1)
        {
            //Calculate the path between p0 and p1
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(p0, p1, NavMesh.AllAreas, path);

            //If there is no path to the target, return false
            if (path.corners.Length == 0)
            {
                return false;
            }

            //Draw the path
            Handles.color = Color.green;
            for (int i = 1; i < path.corners.Length; ++i)
            {
                Handles.DrawLine(path.corners[i - 1], path.corners[i]);
            }

            return true;
        }
        #endregion

        #region Editor=================================================================================
        private void OnEnable()
        {
            m_navMeshRoute = target as NavMeshRoute;
            m_mode = Mode.kDefault;
        }

        private void OnSceneGUI()
        {
            //Draw the first point
            Vector3 p0 = ShowPoint(0);
            Vector3 p1;

            for (int i = 1; i < m_navMeshRoute.points.Length; ++i)
            {
                //Draw the point
                p1 = ShowPoint(i);

                //Draw the path from previous point to this point
                DrawPath(p0, p1);

                //Set the previous point as this point
                p0 = p1;
            }

            //If we are in add point mode
            if (m_mode == Mode.kAddPoint)
            {
                //Get the mouse position
                Event e = Event.current;

                //If mouse right click is pressed, back to default mode
                if (e.type == EventType.MouseDown && e.button == 1)
                {
                    m_mode = Mode.kDefault;
                    return;
                }

                // Raycast from the mouse position to world position
                SceneView sceneView = SceneView.currentDrawingSceneView;
                Camera camera = sceneView.camera;
                Vector3 viewportPoint = camera.ScreenToViewportPoint(e.mousePosition);
                viewportPoint.y = (viewportPoint.y - 0.5f) * (-1) + 0.5f;

                Ray ray = camera.ViewportPointToRay(viewportPoint);

                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                {
                    Vector3 worldPosition = hitInfo.point;

                    //If there is no path to destination, do not draw the point
                    if (m_navMeshRoute.points.Length != 0 && !DrawPath(p0, worldPosition))
                        return;

                    //Draw the new point
                    if (DrawSelectablePoint(worldPosition))
                    {
                        Vector3[] points = m_navMeshRoute.points;
                        Array.Resize(ref points, points.Length + 1);
                        points[points.Length - 1] = m_navMeshRoute.transform.InverseTransformPoint(worldPosition);
                        m_navMeshRoute.points = points;

                        EditorUtility.SetDirty(m_navMeshRoute);
                    }
                }
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            if (GUILayout.Button("Add Point"))
            {
                m_mode = Mode.kAddPoint;
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

    }
}

