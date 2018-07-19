//Item.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale
{
    public abstract class Item : ScriptableObject
    {
        #region fields=============================================================================
        [SerializeField]
        [TextArea]
        [Tooltip("The description of the item")]
        private string m_description;

        [SerializeField]
        [Tooltip("The image icon of the item")]
        private Sprite m_icon;
        #endregion

        #region properties=========================================================================
        /// <summary>
        /// Return the description of the item
        /// </summary>
        public string description
        {
            get
            {
                return m_description;
            }
        }

        /// <summary>
        /// Return the icon image of the item
        /// </summary>
        public Sprite icon
        {
            get
            {
                return m_icon;
            }
        }
        #endregion
    }

}