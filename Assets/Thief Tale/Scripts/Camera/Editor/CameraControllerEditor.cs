//CameraController.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEditor;
using UnityEngine;

namespace ThiefTale
{
    [CustomEditor(typeof(CameraController))]
    public class CameraControllerEditor : Editor
    {
        public void OnSceneGUI()
        {
            if (!Application.isPlaying)
            {
                CameraController camera = target as CameraController;
                camera.UpdateCameraTransform(1.0f);
            }

        }
    }
}

