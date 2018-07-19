//OnHitByProjectile.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;
using UnityEngine.Events;

namespace ThiefTale
{
    public class OnHitByProjectile : MonoBehaviour
    {
        #region fields=============================================================================
        [SerializeField] private UnityEvent m_onHitByProjectile;
        #endregion

        #region methods============================================================================
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Projectile>() != null)
                m_onHitByProjectile.Invoke();
        }
        #endregion
    }

}
