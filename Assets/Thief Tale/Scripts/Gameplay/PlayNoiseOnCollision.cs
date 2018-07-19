//PlayNoiseOnCollision.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale
{
    public class PlayNoiseOnCollision : MonoBehaviour
    {
        #region fields=============================================================================
        [SerializeField] Noise m_noise;
        #endregion

        #region MonoBehaviour======================================================================
        private void OnCollisionEnter(Collision collision)
        {
            m_noise.Play(transform.position);
        }
        #endregion
    }

}