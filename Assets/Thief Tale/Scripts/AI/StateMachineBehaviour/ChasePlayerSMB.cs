//ChasePlayerSMB.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;
using UnityEngine.AI;

namespace ThiefTale.AI
{
    public class ChasePlayerSMB : AiStateMachineBehaviour
    {
        #region StateMachineBehaviour==============================================================
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            m_aiController.SetDestination(PlayerController.instance.transform.position);
        }
        #endregion
    }

}   