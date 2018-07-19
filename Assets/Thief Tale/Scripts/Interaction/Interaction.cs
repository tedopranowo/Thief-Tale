//Interactable.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale
{
    [RequireComponent(typeof(Collider))]
    public abstract class Interaction : MonoBehaviour
    {
        #region fields=============================================================================
        [SerializeField] protected string m_interactionName = "Interact";
        #endregion

        #region properties=========================================================================
        public string interactionName
        {
            get { return m_interactionName; }
        }
        #endregion

        #region methods============================================================================
        /// <summary>
        /// This function is called when a unit interacts with this object
        /// </summary>
        /// <param name="unit"> The unit which interacts with this object</param>
        public abstract void TriggerInteraction(Character unit);
        protected virtual void OnUnitEnterInteractionArea(Character unit) { }
        protected virtual void OnUnitExitInteractionArea(Character unit) { }

        #endregion

        #region MonoBehaviour======================================================================
        protected virtual void Awake()
        {
            //Make sure this object have a collider with trigger
            if (Debug.isDebugBuild)
            {
                Collider[] colliders = GetComponents<Collider>();

                for (int i=0; i<colliders.Length; ++i)
                {
                    if (colliders[i].isTrigger)
                    {
                        if (i == 0 && colliders.Length > 1)
                            Debug.LogError("The first collider of an object should have isTrigger false");
                        
                        return;
                    }
                }

                Debug.LogError("\"" + name + "\" has no trigger for interactable!");
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            Character otherUnit = other.gameObject.GetComponent<Character>();
            if (otherUnit != null)
            {
                otherUnit.RegisterInteractable(this);
                OnUnitEnterInteractionArea(otherUnit);
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            Character otherUnit = other.gameObject.GetComponent<Character>();
            if (otherUnit != null)
            {
                otherUnit.UnregisterInteractable(this);
                OnUnitExitInteractionArea(otherUnit);
            }
        }
        #endregion
    }
}

