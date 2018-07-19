//ItemUIEditor.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;
using UnityEditor;

namespace ThiefTale
{
    [CustomEditor(typeof(ItemUI))]
    [CanEditMultipleObjects]
    public class ItemUIEditor : Editor
    {
        #region enum===============================================================================
        private enum ItemType
        {
            kQuestItem,
            kTool
        }
        #endregion

        #region fields=============================================================================
        private SerializedProperty m_universalIndexProperty;
        #endregion

        #region properties=========================================================================
        private int universalIndex
        {
            get { return m_universalIndexProperty.intValue; }
        }
        private int index
        {
            get
            {
                int localUniversalIndex = universalIndex;

                switch(itemType)
                {
                    case ItemType.kQuestItem:
                        return localUniversalIndex;
                    case ItemType.kTool:
                        return localUniversalIndex - Constant.Inventory.kMaxQuestItemCount;
                }
                return universalIndex;
            }
        }

        private ItemType itemType
        {
            get
            {
                int localUniversalIndex = universalIndex;

                Debug.Assert(localUniversalIndex >= 0);

                if (localUniversalIndex >= Constant.Inventory.kMaxQuestItemCount)
                    return ItemType.kTool;
                else
                    return ItemType.kQuestItem;
            }
        }
        #endregion

        #region Editor override====================================================================
        private void OnEnable()
        {
            m_universalIndexProperty = serializedObject.FindProperty("m_universalIndex");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            #region Universal Index UI
            ItemType newItemType = (ItemType)EditorGUILayout.EnumPopup("Item Type", itemType);
            int newIndex = EditorGUILayout.IntField("Index", index);

            int universalIndex = newIndex;
            if (newItemType == ItemType.kTool)
                universalIndex += Constant.Inventory.kMaxQuestItemCount;

            m_universalIndexProperty.intValue = universalIndex;
            #endregion

            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }

}
