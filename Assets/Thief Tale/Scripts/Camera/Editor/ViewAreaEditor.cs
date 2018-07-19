//ViewAreaEditor.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEditor;
using UnityEngine;

namespace ThiefTale
{
    [CustomEditor(typeof(ViewArea))]
    public class ViewAreaEditor : Editor
    {
        #region fields=============================================================================
        private ViewArea m_viewArea;
        private SerializedProperty m_cameraTransformProperty;
        private SerializedProperty m_lookAtPlayerProperty;
        #endregion

        #region methods============================================================================
        /// <summary>
        /// Convert a boundary into camera viewport point
        /// </summary>
        /// <param name="bounds"> The boundary to be converted to viewport point </param>
        /// <returns> The rect of the viewport point </returns>
        private Rect GetViewportRectFromBoundary(Bounds bounds)
        {
            const int kBoundCornersCount = 8;

            //Convert all 8 corners of the bounds into screen position
            Vector2[] boundaryViewportPoints = new Vector2[kBoundCornersCount];
            boundaryViewportPoints[0] = Camera.main.WorldToViewportPoint(bounds.min);
            boundaryViewportPoints[1] = Camera.main.WorldToViewportPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.max.z));
            boundaryViewportPoints[2] = Camera.main.WorldToViewportPoint(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z));
            boundaryViewportPoints[3] = Camera.main.WorldToViewportPoint(new Vector3(bounds.min.x, bounds.max.y, bounds.max.z));
            boundaryViewportPoints[4] = Camera.main.WorldToViewportPoint(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z));
            boundaryViewportPoints[5] = Camera.main.WorldToViewportPoint(new Vector3(bounds.max.x, bounds.min.y, bounds.max.z));
            boundaryViewportPoints[6] = Camera.main.WorldToViewportPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z));
            boundaryViewportPoints[7] = Camera.main.WorldToViewportPoint(bounds.max);

            //Find the min and max value for boundaryViewportPoints
            Vector2 minViewportPoint = boundaryViewportPoints[0];
            Vector2 maxViewportPoint = boundaryViewportPoints[0];
            for (int i = 1; i < kBoundCornersCount; ++i)
            {
                minViewportPoint = Vector3.Min(minViewportPoint, boundaryViewportPoints[i]);
                maxViewportPoint = Vector3.Max(maxViewportPoint, boundaryViewportPoints[i]);
            }

            //Create the rect
            Rect viewportRect = new Rect();
            viewportRect.min = minViewportPoint;
            viewportRect.max = maxViewportPoint;

            return viewportRect;
        }

        /// <summary>
        /// Calculate the desired camera position to view the area by using the collider boundary and camera rotation
        /// </summary>
        private void CalculateDesiredCameraPos()
        {
            //Get the boundary of the current view area
            Bounds viewBoundary = m_viewArea.bounds;

            //Assume the camera has the required angle
            Transform cameraTransform = Camera.main.transform;
            Quaternion cameraOldRotation = cameraTransform.rotation;
            Vector3 cameraOldPosition = cameraTransform.position;
            cameraTransform.rotation = m_viewArea.desiredCameraRotation;

            //Get the viewport rect of the boundary
            Rect currentViewportRect = GetViewportRectFromBoundary(viewBoundary);
            float accuracy = Mathf.Max(currentViewportRect.width, currentViewportRect.height);
            bool isExceeding = accuracy > 1.05f;
            float rateOfChange = 1.0f;

            while (accuracy < 0.95f || accuracy > 1.05f)
            {
                //Calculate the required scaling
                Vector2 requiredScaling = currentViewportRect.size - Vector2.one;

                //Move the camera backward to adjust the scale
                cameraTransform.position += cameraTransform.forward * rateOfChange * ((isExceeding) ? (-1) : (1));

                //Update the current viewport rect
                currentViewportRect = GetViewportRectFromBoundary(viewBoundary);
                accuracy = Mathf.Max(currentViewportRect.width, currentViewportRect.height);

                bool isNowExceeding = accuracy > 1.05f;
                if (isNowExceeding != isExceeding)
                {
                    isExceeding = isNowExceeding;
                    rateOfChange /= 2;
                }
            }

            //Print viewport
            currentViewportRect = GetViewportRectFromBoundary(viewBoundary);

            Debug.Log(currentViewportRect);

            //Calculate how much moving the camera movement to the right and up affect the viewport
            cameraTransform.position += cameraTransform.right + cameraTransform.up;
            Rect viewportRectOnOneUnitUpRight = GetViewportRectFromBoundary(viewBoundary);
            Vector2 affectedPositionOnUpRight = new Vector2(viewportRectOnOneUnitUpRight.x - currentViewportRect.x, viewportRectOnOneUnitUpRight.y - currentViewportRect.y);
            cameraTransform.position -= cameraTransform.right + cameraTransform.up;

            //Move the camera to horizontally to adjust the position
            cameraTransform.position += cameraTransform.right * ((-currentViewportRect.xMin) / affectedPositionOnUpRight.x);
            cameraTransform.position += cameraTransform.up * ((-currentViewportRect.yMin) / affectedPositionOnUpRight.y);

            //Print viewport
            currentViewportRect = GetViewportRectFromBoundary(viewBoundary);

            Debug.Log(currentViewportRect);


            //Save the desired position
            m_viewArea.desiredCameraPosition = cameraTransform.position;

            //Reset the camera to the old position
            cameraTransform.position = cameraOldPosition;
            cameraTransform.rotation = cameraOldRotation;
        }
        #endregion



        #region Editor=============================================================================
        private void OnEnable()
        {
            m_viewArea = (ViewArea)target;
            m_cameraTransformProperty = serializedObject.FindProperty("m_cameraTransform");
            m_lookAtPlayerProperty = serializedObject.FindProperty("m_lookAtPlayer");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            #region Draw Camera Transform UI
            EditorGUILayout.BeginHorizontal();

            //Create the input field for camera transform
            EditorGUILayout.PropertyField(m_cameraTransformProperty);

            // If the camera transform is null, 
            // add a button to create a new transform
            if (m_cameraTransformProperty.objectReferenceValue as Transform == null)
            {
                const string selectButtonText = "Create";
                const string selectButtonTooltip = "Create a GameObject and set it as desired camera transform";
                if (GUILayout.Button(new GUIContent(selectButtonText, selectButtonTooltip), GUILayout.Width(100)))
                {
                    //Create a game object representing the desired camera transform
                    GameObject desiredCamera = new GameObject("Desired Camera");
                    desiredCamera.transform.parent = m_viewArea.transform;
                    desiredCamera.transform.localPosition = Vector3.zero;
                    desiredCamera.AddComponent<DrawCameraFrustrum>();

                    //Set the camera transform to the newly created object
                    m_cameraTransformProperty.objectReferenceValue = desiredCamera;

                    //Transform m_desiredCameraTransform = m_cameraTransformProperty.objectReferenceValue as Transform;
                    Selection.activeGameObject = desiredCamera;
                }
            }
            // If the camera transform is not null,
            // Create a button to auto adjust the camera position if the camera transform is not null
            else
            {
                const string autoButtonText = "Auto Adjust";
                const string autoButtonTooltip = "Automatically adjust the camera position to view the whole area";
                if (GUILayout.Button(new GUIContent(autoButtonText, autoButtonTooltip), GUILayout.Width(100)))
                {
                    CalculateDesiredCameraPos();
                }
            }

            EditorGUILayout.EndHorizontal();
            #endregion

            //Draw look at player UI
            EditorGUILayout.PropertyField(m_lookAtPlayerProperty);

            serializedObject.ApplyModifiedProperties();
        }
        #endregion  
    }

}