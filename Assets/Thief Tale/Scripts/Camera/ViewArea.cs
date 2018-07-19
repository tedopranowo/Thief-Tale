//ViewArea.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale
{
    [RequireComponent(typeof(Collider))]
    public class ViewArea : MonoBehaviour
    {
        #region fields=============================================================================
        [Tooltip("The desired camera transform when the player enters the area")]
        [SerializeField]
        private Transform m_cameraTransform;

        [Tooltip("If enabled, the camera will look at the player while maintaining position")]
        [SerializeField]
        private bool m_lookAtPlayer;
        #endregion

        #region properties=========================================================================
        public Bounds bounds
        {
            get
            {
                return GetComponent<Collider>().bounds;
            }
        }

        public Vector3 desiredCameraPosition
        {
            set
            {
                m_cameraTransform.position = value;
            }
            get
            {
                return m_cameraTransform.position;
            }
        }

        public Quaternion desiredCameraRotation
        {
            set
            {
                m_cameraTransform.rotation = value;
            }
            get
            {
                return m_cameraTransform.rotation;
            }
        }

        #endregion

        #region MonoBehaviour======================================================================
        private void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerDetector");
        }

        private void Update()
        {
            if (m_lookAtPlayer)
            {
                m_cameraTransform.LookAt(PlayerController.instance.transform);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //Check if the object entering the trigger is controlled by a plyaer
            PlayerController playerController = other.GetComponent<PlayerController>();

            //Assign this view area to the camera
            if (playerController != null)
            {
                Camera.main.GetComponent<CameraController>().AssignViewArea(this);
            }

        }

        private void OnTriggerExit(Collider other)
        {
            //Check if the object entering the trigger is controlled by a plyaer
            PlayerController playerController = other.GetComponent<PlayerController>();

            //Unassign this view area to the camera
            if (playerController != null)
            {
                Camera.main.GetComponent<CameraController>().UnassignViewArea(this);
            }
        }

        #endregion
    }

}
