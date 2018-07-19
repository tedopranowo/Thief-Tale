//CameraController.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ThiefTale
{
    /// <summary>
    /// CameraController class
    /// In summary, there are 3 steps how the default camera position is determined:
    /// 1. Calculate where the 'desired' camera position is
    /// 2. Lerp the camera position toward the 'desired' camera position
    /// 3. Clamp the camera y relative to the player y
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        #region fields=============================================================================
        [Tooltip("The unit this camera will follow")]
        private Character m_followedUnit;

        [Tooltip("The lerp speed of the camera")]
        [Range(0, 1)]
        [SerializeField]
        private float m_lerpSpeed = 0.05f;

        [Tooltip("The distance from the unit to the camera")]
        [SerializeField]
        private Vector3 m_distanceFromUnit;

        [Tooltip("The default rotation of the camera")]
        [SerializeField]
        private Vector3 m_defaultRotation;

        //----------------------
        [Header("X Position")]
        //----------------------
        [Tooltip("The speed of the desired x position change")]
        [SerializeField]
        private float m_desiredXPosSpeed = 3.0f;

        [Tooltip("The maximum distance of the camera's x from the followed unit's x")]
        [SerializeField]
        private float m_maxXDistanceFromCenter = 1.0f;

        private float m_oldXPosition;
        private float m_desiredXPosFromPlayer;

        //----------------------
        [Header("Y Position")]
        //----------------------
        [Tooltip("The upper error limit for the y value")]
        [SerializeField]
        private float m_maxHeightLimitFromCenter = 1.0f;

        [Tooltip("The lower error limit for the y value")]
        [SerializeField]
        private float m_minHeightLimitFromCenter = -1.0f;

        //----------------------
        // View Area
        //----------------------
        private List<ViewArea> m_viewAreas = new List<ViewArea>();

        #endregion

        #region properties=========================================================================
        private Vector3 followedUnitPosition
        {
            set
            {
                m_followedUnit.transform.position = value;
            }

            get
            {
                return m_followedUnit.transform.position;
            }
        }

        #endregion

        #region methods============================================================================
        /// <summary>
        /// Calculate the desired x position for the camera
        /// </summary>
        /// <returns> The desired X position for camera</returns>
        private float CalculateDesiredX()
        {
            //Calculate desired position
            if (m_oldXPosition != followedUnitPosition.x)
            {
                m_desiredXPosFromPlayer = m_desiredXPosFromPlayer + m_followedUnit.transform.forward.x * m_desiredXPosSpeed * Time.fixedDeltaTime;
                m_desiredXPosFromPlayer = Mathf.Clamp(m_desiredXPosFromPlayer, -m_maxXDistanceFromCenter, m_maxXDistanceFromCenter);

                m_oldXPosition = followedUnitPosition.x;
            }

            //Calculate desired x position
            float desiredXPos = m_desiredXPosFromPlayer + followedUnitPosition.x;

            //Debug.DrawLine(m_followedUnit.transform.position, new Vector3(desiredXPos, m_followedUnit.transform.position.y, m_followedUnit.transform.position.z), Color.red);

            return desiredXPos + m_distanceFromUnit.x;
        }

        /// <summary>
        /// Calculate the desired y position for the camera
        /// </summary>
        /// <returns>The desired Y position for camera</returns>
        private float CalculateDesiredY()
        {
            float desiredY = transform.position.y;

            //If the unit is ground, recenter the y to the unit
            if (m_followedUnit.isGrounded)
                desiredY = followedUnitPosition.y + m_distanceFromUnit.y;

            //Clamp the camera y position according to camera position
            desiredY = Mathf.Clamp(transform.position.y, followedUnitPosition.y - m_maxHeightLimitFromCenter + m_distanceFromUnit.y, followedUnitPosition.y - m_minHeightLimitFromCenter + m_distanceFromUnit.y);

            return desiredY;
        }

        /// <summary>
        /// Calculate the desired z position for the camera
        /// </summary>
        /// <returns> The desired Z position for camera </returns>
        private float CalculateDesiredZ()
        {
            return followedUnitPosition.z + m_distanceFromUnit.z;
        }

        /// <summary>
        /// A coroutine which shakes the camera
        /// </summary>
        /// <param name="strength">The strength / magnitude of the shake</param>
        /// <param name="duration">The duration of the shake</param>
        /// <returns></returns>
        private IEnumerator ShakeCoroutine(float strength, float duration)
        {
            while(duration > 0)
            {
                transform.position = transform.position + Random.insideUnitSphere * strength;
                yield return new WaitForFixedUpdate();
                duration -= Time.fixedDeltaTime;
            }
        }

        /// <summary>
        /// Shake the camera
        /// </summary>
        /// <param name="strength"> The strength / magnitude of the shake </param>
        /// <param name="duration"> The duration of the shake</param>
        public void Shake(float strength, float duration)
        {
            StartCoroutine(ShakeCoroutine(strength, duration));
        }

        /// <summary>
        /// Assign the view area to this camera. This will cause the camera to focus on that
        /// view area
        /// </summary>
        /// <param name="viewArea"> The view area to be assigned </param>
        public void AssignViewArea(ViewArea viewArea)
        {
            m_viewAreas.Add(viewArea);
        }

        /// <summary>
        /// Unassign the view area to this camera. This will also cause the camera to unfocus that view area if
        /// it is being focused
        /// </summary>
        /// <param name="viewArea"> The view area to be unassigned </param>
        public void UnassignViewArea(ViewArea viewArea)
        {
            m_viewAreas.Remove(viewArea);
        }

        /// <summary>
        /// Update the camera position
        /// </summary>
        /// <param name="lerpSpeed"> The lerp speed of the camera </param>
        public void UpdateCameraTransform(float lerpSpeed)
        {
            //If the view area list isn't empty, focus the camera to the view area at the last index
            if (m_viewAreas.Count > 0)
            {
                Vector3 targetPosition = m_viewAreas[m_viewAreas.Count - 1].desiredCameraPosition;
                Quaternion targetRotation = m_viewAreas[m_viewAreas.Count - 1].desiredCameraRotation;
                transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed);
            }
            //If the view area list is empty, follow the player
            else
            {
                //Calculate the desired position for the camera
                Vector3 targetPosition = new Vector3(CalculateDesiredX(), CalculateDesiredY(), CalculateDesiredZ());
                Quaternion targetRotation = Quaternion.identity;
                targetRotation.eulerAngles = m_defaultRotation;
                transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed);
            }
        }

        #endregion

        #region MonoBehaviours=====================================================================

        private void Awake()
        {
            m_followedUnit = PlayerController.instance.GetComponent<Character>();
        }
        private void FixedUpdate()
        {
            UpdateCameraTransform(m_lerpSpeed);
        }

        #endregion
    }
}

