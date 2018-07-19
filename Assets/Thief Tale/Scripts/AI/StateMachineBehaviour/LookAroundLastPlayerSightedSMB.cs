//LookAroundLastPlayerSightedSMB.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale.AI
{
    public class LookAroundLastPlayerSightedSMB : AiStateMachineBehaviour
    {
        #region fields=============================================================================
        [SerializeField] private float m_searchRadius;
        [SerializeField] private float m_changePathInterval;
        private Vector3 m_targetLocation;
        private float m_nextUpdateTime;
        #endregion

        #region StateMachineBehaviour==============================================================
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            m_nextUpdateTime = Time.time;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (m_nextUpdateTime < Time.time)
            {
                //Find a random position within player last sighted position
                Vector3 offsetFromCenter = new Vector3(Random.Range(0, m_searchRadius), 0, Random.Range(0, m_searchRadius));

                while (!m_aiController.SetDestination((Vector3)exposedVariables["LastPlayerSightedLocation"] + offsetFromCenter))
                {
                    offsetFromCenter = new Vector3(Random.Range(0, m_searchRadius), 0, Random.Range(0, m_searchRadius));
                }

                m_nextUpdateTime = Time.time + m_changePathInterval;
            }
        }

        #endregion
    }

}
