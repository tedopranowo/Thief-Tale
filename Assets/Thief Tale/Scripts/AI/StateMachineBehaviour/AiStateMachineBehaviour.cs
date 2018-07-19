//AiStateMachineBehaviour.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;
using System.Collections.Generic;

namespace ThiefTale.AI
{
    public abstract class AiStateMachineBehaviour : StateMachineBehaviour
    {
        #region fields====================================================================================
        protected AiController m_aiController;
        #endregion

        #region properties================================================================================
        protected Dictionary<string, object> exposedVariables
        {
            get
            {
                return m_aiController.exposedVariables;
            }
        }
        #endregion  

        #region methods===================================================================================
        public virtual void UpdateExposedVariables(Dictionary<string, object> dictionary) { }

        public virtual void GetExposedVariables(Dictionary<string, System.Type> dictionary) { }
        #endregion

        #region StateMachineBehaviour=====================================================================
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_aiController = animator.gameObject.GetComponentInParent<AiController>();
        }
        #endregion
    }

}
