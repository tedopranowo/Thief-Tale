//PatrolSMB.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using System.Collections.Generic;
using UnityEngine;

namespace ThiefTale.AI
{
    public class PatrolSMB : AiStateMachineBehaviour
    {
        #region fields=============================================================================
        private Route m_route;
        [SerializeField] private int m_routeIndex;
        #endregion

        #region AiStateMachineBehaviour============================================================
        public override void UpdateExposedVariables(Dictionary<string, object> dictionary)
        {
            base.UpdateExposedVariables(dictionary);

            m_route = dictionary["Route"] as Route;
        }
        public override void GetExposedVariables(Dictionary<string, System.Type> dictionary)
        {
            base.GetExposedVariables(dictionary);

            dictionary.Add("Route", typeof(Route));
        }
        #endregion

        #region StateMachineBehaviour==============================================================
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            animator.SetBool("IsIdle", false);
            m_aiController.SetDestination(m_route.GetPoint(m_routeIndex));
        }
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            //Walking through the route
            Vector3 nextGoal = m_route.GetPoint(m_routeIndex);
            Vector3 unitPosition = m_aiController.transform.position;

            //If the unit is idle
            if (animator.GetBool("IsIdle") == true)
            {
                ++m_routeIndex;
                if (m_routeIndex >= m_route.GetLength())
                    m_routeIndex = 0;

                m_aiController.SetDestination(m_route.GetPoint(m_routeIndex));
            }
        }
        #endregion

    }

}
