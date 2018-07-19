//ChangeMovementTypeSMB.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale.AI
{
    public class ChangeMovementTypeSMB : AiStateMachineBehaviour
    {
        #region fields=============================================================================
        [SerializeField] private Character.MovementType m_movementType;
        #endregion

        #region AiStateMachineBehaviour============================================================
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            m_aiController.controlledUnit.movementType = m_movementType;
        }
        #endregion
    }

}
