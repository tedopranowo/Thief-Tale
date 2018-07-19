//ItemDetailUI.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;
using UnityEngine.UI;

namespace ThiefTale
{
    public class ItemDetailUI : MonoBehaviour
    {
        #region fields=============================================================================
        [SerializeField]
        private static ItemDetailUI s_instance;
        
        [SerializeField]
        private Image m_itemIconImage;

        [SerializeField]
        private Text m_itemNameText;

        [SerializeField]
        private Text m_itemDescriptionText;
        #endregion

        #region properties=========================================================================
        public static ItemDetailUI instance
        {
            get { return s_instance; }
        }

        private Sprite icon
        {
            set
            {
                m_itemIconImage.sprite = value;
                //If the sprite is null, set the image to be not visible
                if (value == null)
                    m_itemIconImage.color = Color.clear;
                else
                    m_itemIconImage.color = Color.white;
            }
        }

        private string name
        {
            set
            {
                m_itemNameText.text = value;
            }
        }

        private string description
        {
            set
            {
                m_itemDescriptionText.text = value;
            }
        }


        #endregion

        #region methods============================================================================
        public void ShowDetail(ItemUI itemUI)
        {
            if (itemUI.item == null)
            {
                RemoveDetail();
            }
            else
            {
                icon = itemUI.item.icon;
                name = itemUI.item.name;
                description = itemUI.item.description;
            }
        }

        public void RemoveDetail()
        {
            icon = null;
            name = "";
            description = "";
        }
        #endregion

        #region MonoBehaviours=====================================================================
        public void Awake()
        {
            if (s_instance != null)
            {
                Debug.LogError("Multiple instance of ItemDetailUI detected");
                Destroy(gameObject);
                return;
            }

            s_instance = this;
        }
        #endregion
    }

}