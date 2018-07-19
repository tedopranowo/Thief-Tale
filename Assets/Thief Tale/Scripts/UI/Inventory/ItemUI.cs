//ItemUI.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ThiefTale
{
    [RequireComponent(typeof(Image))]
    public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region fields=============================================================================
        //Note: this class has a custom inspector
        [SerializeField] private int m_universalIndex;
        
        private ItemContainer m_itemContainer;
        #endregion

        #region properties=========================================================================
        public Sprite image
        {
            set
            {
                GetComponent<Image>().sprite = value;
            }
        }

        public Item item
        {
            get
            {
                return m_itemContainer.content;
            }
        }

       
        #endregion

        #region MonoBehaviours=====================================================================
        private void Start()
        {
            m_itemContainer = PlayerController.instance.GetComponent<Inventory>().GetItemContainer(m_universalIndex);
            m_itemContainer.ui = this;
        }
        #endregion

        #region Interfaces=========================================================================
        public void OnPointerEnter(PointerEventData eventData)
        {
            ItemDetailUI.instance.ShowDetail(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ItemDetailUI.instance.RemoveDetail();
        }
        #endregion
    }
}