//Slingshot.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)

using UnityEngine;

namespace ThiefTale
{
    [CreateAssetMenu(fileName = "Slingshot", menuName = "ThiefTale/Item/Slingshot")]
    public class Slingshot : Tool
    {
        #region fields=============================================================================
        [SerializeField] private int m_damage = 0;
        [SerializeField] private float m_projectileSpeed = 1.0f;
        [SerializeField] private GameObject m_projectile = null;

        private Character m_owner;
        #endregion

        #region methods============================================================================
        /// <summary>
        /// Shoot the projectile to target location
        /// </summary>
        /// <param name="targetPosition"> The target of the shot </param>
        public void Shoot(Vector3 targetPosition)
        {
            Vector3 spawnLocation = m_owner.transform.position + Vector3.up * 0.5f;
            Vector3 direction = targetPosition - spawnLocation;

            //[TODO]: Do not hardcode the projectile spawn location
            GameObject projectile = Instantiate(m_projectile, spawnLocation + direction.normalized, Quaternion.identity);

            //[TODO]: Refactor
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.damage = m_damage;

            Rigidbody rigidbody = projectile.GetComponent<Rigidbody>();
            rigidbody.velocity = direction.normalized * m_projectileSpeed;
        }

        #endregion

        #region tool override======================================================================
        public override void OnObtained(Character unit)
        {
            m_owner = unit;
        }

        public override void OnRemoved(Character unit)
        {
        }
        #endregion
    }

}