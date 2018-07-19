//Tool.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)

using UnityEngine;

namespace ThiefTale
{
    public abstract class Tool : Item
    {
        #region methods=============================================================================
        /// <summary>
        /// This function will be called when the unit obtain the tools
        /// </summary>
        /// <param name="unit"> The unit which obtain the tool </param>
        public abstract void OnObtained(Character unit);

        /// <summary>
        /// THis function will be called when this item is removed from the unit's inventory
        /// </summary>
        /// <param name="unit"> The unit which removes the tool </param>
        public abstract void OnRemoved(Character unit);
        #endregion
    }

}
