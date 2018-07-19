//Destructible.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale
{
    public class Destructible : MonoBehaviour
    {
        #region fields=============================================================================
        [SerializeField] private int m_maxHp;
        private int m_currentHp;
        #endregion

        #region methods============================================================================
        public void TakeDamage(int damage)
        {
            m_currentHp -= damage;

            //Destroy self if it has 0 hp
            if (m_currentHp <= 0)
                Destroy(gameObject);
        }
        #endregion

        #region MonoBehaviour======================================================================
        private void Awake()
        {
            m_currentHp = m_maxHp;
        }

        #endregion
    }

}
