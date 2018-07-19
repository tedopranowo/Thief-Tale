//ItemContainer.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale
{
    [System.Serializable]
    public class ItemContainer
    {
        #region fields=============================================================================
        [SerializeField]
        private Item m_item;

        private ItemUI m_itemUI;
        #endregion

        #region properties=========================================================================
        public Item content
        {
            set
            {
                m_item = value;

                if (m_itemUI)
                    m_itemUI.image = m_item.icon;
            }

            get
            {
                return m_item;
            }
        }

        public ItemUI ui
        {
            set
            {
                //Remove the image from the old UI
                if (m_itemUI != null)
                    m_itemUI.image = null;

                //Set the new item UI
                m_itemUI = value;

                //Set the image for the new UI
                if (m_itemUI != null && m_item != null)
                    m_itemUI.image = m_item.icon;
            }
            get
            {
                return m_itemUI;
            }
        }

        #endregion
    }

}