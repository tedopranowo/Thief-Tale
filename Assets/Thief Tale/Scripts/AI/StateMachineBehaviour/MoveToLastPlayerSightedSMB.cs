//MoveToLastPlayerSightedSMB.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale.AI
{
    public class MoveToLastPlayerSightedSMB : AiStateMachineBehaviour
    {
        #region AiStateMachineBehaviour============================================================
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            m_aiController.SetDestination((Vector3)exposedVariables["LastPlayerSightedLocation"]);
            Debug.DrawLine(m_aiController.transform.position, (Vector3)exposedVariables["LastPlayerSightedLocation"], Color.red, 1.0f);
        }

        #endregion
    }

}
