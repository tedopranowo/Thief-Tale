//DestroyOnCollision.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)

using UnityEngine;

namespace ThiefTale
{
    public class DestroyOnCollision : MonoBehaviour
    {
        #region MonoBehaviour======================================================================
        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }
        #endregion  
    }

}