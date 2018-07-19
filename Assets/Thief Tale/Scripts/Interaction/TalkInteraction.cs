using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThiefTale
{
    public class TalkInteraction : Interaction
    {
        [SerializeField] private Dialogue_System m_dialogueSystem;

        public override void TriggerInteraction(Character unit)
        {
            m_dialogueSystem.InitiateDialog();
        }

        private void Reset()
        {
            m_interactionName = "Talk";
        }
    }
}
