//Inventory.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)

using System.Collections.Generic;
using UnityEngine;

namespace ThiefTale
{
    public class Inventory : MonoBehaviour
    {
        #region fields=============================================================================
        [SerializeField]
        private ItemContainer[] m_questItemContainers = new ItemContainer[Constant.Inventory.kMaxQuestItemCount];

        [SerializeField]
        private ItemContainer[] m_toolContainers = new ItemContainer[Constant.Inventory.kMaxToolCount];

        private Character m_owner;
        private Slingshot m_slingshot;
        #endregion

        #region properties=========================================================================
        public Slingshot slingshot
        {
            get
            {
                return m_slingshot;
            }
        }

        #endregion

        #region methods============================================================================
        /// <summary>
        /// Add a tool to the inventory
        /// </summary>
        /// <param name="tool"> The tool to be added to inventory</param>
        private void Add(Tool tool)
        {
            //Note: Normally, doing O(n) is not what we want. But, in this case, it is not a big deal
            //      because there are only 8 slots for tool and the loop doesn't do heavy process.
            foreach(ItemContainer toolContainer in m_toolContainers)
            {
                if (toolContainer.content == tool)
                    break;

                if (toolContainer.content == null)
                {
                    toolContainer.content = tool;
                    break;
                }
            }

            if (tool is Slingshot)
                m_slingshot = tool as Slingshot;

            tool.OnObtained(m_owner);
        }

        /// <summary>
        /// Add a quest item to the inventory
        /// </summary>
        private void Add(QuestItem item)
        {
            //Note: Normally, doing O(n) is not what we want. But, in this case, it is not a big deal
            //      because there are only 8 slots for quest items and the loop doesn't do heavy process.
            foreach(ItemContainer questItemContainer in m_questItemContainers)
            {
                if (questItemContainer.content == null)
                {
                    questItemContainer.content = item;
                    break;
                }
            }
        }

        /// <summary>
        /// Get an item container at index
        /// Index 0 until (toolContainerCount - 1) return an item containers for quest item
        /// Index (toolContainerCount) until (toolContainerCount + questItemContainerCount - 1) return an item containers for tool
        /// </summary>
        public ItemContainer GetItemContainer(int index)
        {
            Debug.Assert(index >= 0);

            if (index >= Constant.Inventory.kMaxQuestItemCount)
                return m_toolContainers[index - Constant.Inventory.kMaxQuestItemCount];
            else
                return m_questItemContainers[index];
        }
        #endregion

        #region MonoBehaviour======================================================================
        private void Awake()
        {
            m_owner = GetComponent<Character>();

            //Apply the tools On obtained effect
            foreach(ItemContainer toolContainer in m_toolContainers)
            {
                Tool tool = toolContainer.content as Tool;

                if (tool != null)
                {
                    if (tool is Slingshot)
                        m_slingshot = tool as Slingshot;

                    tool.OnObtained(m_owner);
                }
            }
        }
        #endregion
    }
}