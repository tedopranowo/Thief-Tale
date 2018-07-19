//Projectile.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale
{
    public class Projectile : MonoBehaviour
    {
        #region fields=============================================================================
        [SerializeField] private int m_damage;
        #endregion

        #region properties========================================================================
        public int damage
        {
            set
            {
                m_damage = value;
            }
            get
            {
                return m_damage;
            }
        }
        #endregion

        #region MonoBehaviour======================================================================
        private void OnCollisionEnter(Collision collision)
        {
            Destructible destructibleObj = collision.gameObject.GetComponent<Destructible>();

            //If this object colliding with a destructible object
            if (destructibleObj != null)
            {
                destructibleObj.TakeDamage(m_damage);
            }

            Destroy(gameObject);
        }
        #endregion
    }

}
