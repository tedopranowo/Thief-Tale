//Route.cs
//Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale
{
    public abstract class Route : MonoBehaviour
    {
        #region methods================================================================================
        public abstract Vector3 GetPoint(int t);

        public abstract int GetLength();
        #endregion
    }

}
