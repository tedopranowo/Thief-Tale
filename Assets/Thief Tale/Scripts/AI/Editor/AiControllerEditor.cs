//AiControllerEditor.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEditor;
using UnityEngine;

namespace ThiefTale
{
    [CustomEditor(typeof(AiController))]
    public class AiControllerEditor : Editor
    {
        //#region fields=============================================================================
        //private SerializedProperty m_vision;
        //private SerializedProperty m_spline;
        //private SerializedProperty m_nextSplineTime;
        //private AiController m_aiController;
        //#endregion

        //#region Editor=============================================================================
        //private void OnEnable()
        //{
        //    m_vision = serializedObject.FindProperty("m_vision");
        //    m_spline = serializedObject.FindProperty("m_spline");
        //    m_nextSplineTime = serializedObject.FindProperty("m_nextSplineTime");
        //    m_aiController = (AiController)target;
        //}

        //public override void OnInspectorGUI()
        //{
        //    serializedObject.Update();

        //    //Draw the vision property
        //    EditorGUILayout.PropertyField(m_vision);
        //    EditorGUILayout.PropertyField(m_spline);

        //    if (m_aiController.route != null)
        //    {
        //        EditorGUILayout.Slider(m_nextSplineTime, 0.0f, m_aiController.route.points.Length - 1, "Starting index");

        //        //Update the position of the character
        //        Vector3 pathPosition = m_aiController.route.GetPoint(m_nextSplineTime.floatValue);

        //        if (Application.isEditor)
        //            m_aiController.transform.position = new Vector3(pathPosition.x, m_aiController.transform.position.y, pathPosition.z);
        //    }

        //    serializedObject.ApplyModifiedProperties();
        //}
        //#endregion 
    }

}
