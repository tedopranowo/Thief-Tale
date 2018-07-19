//Vector3Extension.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale
{
    public static class Vector3Extension
    {
        public static Vector2 GetHorizontal(this Vector3 v3)
        {
            return new Vector2(v3.x, v3.z);
        }
    }

}
