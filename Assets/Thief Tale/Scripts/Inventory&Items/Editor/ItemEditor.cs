//ItemEditor.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;
using UnityEditor;

namespace ThiefTale
{
    [CustomEditor(typeof(Item))]
    public class ItemEditor : Editor
    {
        #region fields=============================================================================
        [SerializeField]
        private Item m_item;

        #endregion

        #region Editor=============================================================================
        private void OnEnable()
        {
            m_item = (Item)target;
        }

        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            return m_item.icon.texture;
        }

        #endregion  
    }

}
