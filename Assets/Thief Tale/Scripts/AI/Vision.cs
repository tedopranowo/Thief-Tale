//Vision.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale
{
    public class Vision : MonoBehaviour
    {
        #region fields=============================================================================
        [Tooltip("The range of the vision")]
        [SerializeField]
        private float m_range;

        [Tooltip("The angle of the cone vision in degree")]
        [SerializeField]
        private float m_angle;

        #endregion

        #region properties=========================================================================
        /// <summary>
        /// Return the vision's range
        /// </summary>
        public float range
        {
            get
            {
                return m_range;
            }
        }
        #endregion

        #region methods============================================================================
        /// <summary>
        /// Return true if player is within vision
        /// </summary>
        /// <returns></returns>
        public bool IsPlayerSighted()
        {
            Vector3 playerPosition = PlayerController.instance.transform.position + 0.5f * Vector3.up;
            Vector3 directionToPlayer = playerPosition - transform.position;
            directionToPlayer.y = 0;
            Vector3 unitForward = transform.forward;
            unitForward.y = 0;
            float squaredDistance = directionToPlayer.sqrMagnitude;

            //If the player is within vision range
            if (squaredDistance <= m_range * m_range)
            {
                //If the player is within vision angle
                if (Vector3.Angle(directionToPlayer, transform.forward) < m_angle)
                {
                    RaycastHit hitInfo;
                    LayerMask hitLayer = (1 << 0) // default
                        | (1 << 8)                // Player
                        | (1 << 11);              // block vision

                    //If there is a collider between the character and the player, return false
                    if (Physics.Linecast(transform.position, playerPosition, out hitInfo, hitLayer, QueryTriggerInteraction.Collide))
                    {
                        if (hitInfo.collider.GetComponent<PlayerController>() != null)
                            return true;
                    }
                }
            }

            return false;
        }
        #endregion

        private void OnDrawGizmos()
        {
            //Draw an estimation of the vision area. Note that the gizmo being drawn isn't
            //the same as actual vision since the vision is is a circle instead of a
            //rectangle
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawFrustum(Vector3.zero, m_angle, m_range, 0.0f, 1.0f);
            Gizmos.matrix = Matrix4x4.identity;
        }

    }

}
