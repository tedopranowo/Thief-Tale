//ClimbInteraction.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)

using UnityEngine;
using System.Collections.Generic;

namespace ThiefTale
{
    public class ClimbInteraction : Interaction
    {
        #region Interactable Override==============================================================
        public override void TriggerInteraction(Character unit)
        {
            unit.ToggleClimb(this);
        }

        protected override void OnUnitExitInteractionArea(Character unit)
        {
            if (unit.climbedObject == this)
            {
                unit.ToggleClimb(this);
            }
        }
        #endregion

        #region MonoBehaviour======================================================================
        private void Reset()
        {
            m_interactionName = "Climb";
        }
        #endregion
    }

}
