//Lifetime.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;
using System.Collections;

namespace ThiefTale
{
    public class Lifetime : MonoBehaviour
    {
        #region fields=============================================================================
        [SerializeField] private float m_lifetime;
        #endregion

        #region methods============================================================================
        private IEnumerator WaitThenDestroy(float time)
        {
            yield return new WaitForSeconds(time);

            Destroy(gameObject);
        }

        #endregion

        #region MonoBehaviour======================================================================
        private void Awake()
        {
            StartCoroutine(WaitThenDestroy(m_lifetime));
        }
        #endregion  
    }
}
