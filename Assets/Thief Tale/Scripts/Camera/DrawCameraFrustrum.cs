
using UnityEngine;

namespace ThiefTale
{
    public class DrawCameraFrustrum : MonoBehaviour
    {
        private void OnDrawGizmosSelected()
        {
            //Get the main camera
            Camera mc = Camera.main;

            //Draw the camera frustum
            Gizmos.color = Color.grey;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawFrustum(Vector3.zero, mc.fieldOfView, mc.farClipPlane, mc.nearClipPlane, mc.aspect);

            //Reset the gizmo setting
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.white;
        }

    }

}
