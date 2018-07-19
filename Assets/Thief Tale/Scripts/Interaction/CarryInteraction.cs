//CarryInteraction.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarryInteraction : Interaction
    {
        #region fields=============================================================================
        private Rigidbody m_rigidbody;
        private bool m_defaultKinematicStatus;

        // NOTE:
        // - We keep track of the trigger because we want to set if the object is interactable or not
        // - We keep track of the collider because we want to know where the object located while
        //   being carried by character
        private Collider m_trigger;     //The last collider with isTrigger true
        private Collider m_collider;    //The last collider with isTrigger false

        private LayerMask m_originalLayer;
        #endregion

        #region properties=========================================================================
        public Collider objectCollider
        {
            get
            {
                return m_collider;
            }
        }
        #endregion

        #region methods============================================================================
        /// <summary>
        /// This function should be called when this object is being picked up
        /// </summary>
        /// <param name="unit"> The unit which carry this object</param>
        public void OnBeingCarried(Character unit)
        {
            m_rigidbody.isKinematic = true;
            m_trigger.enabled = false;

            gameObject.layer = LayerMask.NameToLayer("Player");

            //Note: 
            // We call OnTriggerExit() manually because Unity doesn't call OnTriggerExit() 
            // when a trigger is disabled. Though, for some reason Unity calls OnTriggerEnter()
            // when the trigger is enabled
            OnTriggerExit(unit.GetComponent<Collider>());
        }

        /// <summary>
        /// This function should be called when this object stop being picked up
        /// </summary>
        public void OnBeingDropped()
        {
            m_rigidbody.isKinematic = m_defaultKinematicStatus;
            m_trigger.enabled = true;

            gameObject.layer = m_originalLayer;
        }
        #endregion

        #region Interactable Override==============================================================
        public override void TriggerInteraction(Character unit)
        {
            //Make the unit pick up this object
            unit.carriedObject = this;
        }
        #endregion

        #region MonoBehaviour=====================================================================
        protected override void Awake()
        {
            base.Awake();

            //Get the rigidbody
            m_rigidbody = GetComponent<Rigidbody>();

            //Get the trigger collider
            foreach(Collider collider in GetComponents<Collider>())
            {
                if (collider.isTrigger)
                    m_trigger = collider;
                else
                    m_collider = collider;
            }

            m_defaultKinematicStatus = m_rigidbody.isKinematic;
            m_originalLayer = gameObject.layer;
        }

        private void Reset()
        {
            m_interactionName = "Carry";
        }
        #endregion
    }

}
