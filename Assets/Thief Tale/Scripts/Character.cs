//Unit.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)

using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace ThiefTale
{
    [RequireComponent(typeof(Rigidbody))]
    public class Character : MonoBehaviour
    {
        #region enum===============================================================================
        public enum State : byte
        {
            kIdle,
            kJumping,
            kFallingDown,
            kClimbing,
            kMoving,
            kCount
        }

        public enum MovementType : byte
        {
            kWalking,
            kRunning
        }
        #endregion

        #region fields=============================================================================
        //----------------------------
        [Header("Vertical movement")]
        //----------------------------
        [Tooltip("The velocity applied to the unit when it jumps")]
        [SerializeField]
        private float m_jumpVelocity = 3.0f;

        [Tooltip("How many jump the character has before touching the ground? Setting it to 0 means" +
            "the character won't be able to jump")]
        [SerializeField]
        private int m_maxJumpCount = 1;

        private int m_jumpCount;

        [Tooltip("Fall speed added to the unit when it is falling down")]
        [SerializeField]
        private float m_fallMultiplier = 1.0f;

        //-----------------------------
        [Header("Horizontal movement")]
        //-----------------------------

        [Tooltip("The maximum walking speed the unit has")]
        [SerializeField]
        private float m_walkingSpeed = 2.0f;

        [Tooltip("The maximum walking speed while carrying an object")]
        [SerializeField]
        private float m_carryingWalkSpeed = 1.0f;

        [Tooltip("The time needed to reach maximum walking speed")]
        [SerializeField]
        private float m_walkLerpTime = 1.0f;

        [Tooltip("The maximum running speed the unit has")]
        [SerializeField]
        private float m_runningSpeed = 4.0f;

        [Tooltip("The time needed to reach maximum running speed")]
        [SerializeField]
        private float m_runLerpTime = 1.0f;

        [Tooltip("The higher this value, the faster character slows down")]
        [SerializeField]
        private float m_decelerateSpeed = 4.0f;

        [Tooltip("Adjust this value to adjust when the unit to clip it's y position. Used mainly to climb stairs")]
        [SerializeField]
        private float m_stepOffset = 0.3f;

        private float m_currentHorizontalSpeed = 0.0f;

        //-----------------------------
        [Header("Climbing")]
        //-----------------------------
        [Tooltip("The unit speed while climbing")]
        [SerializeField]
        private float m_climbingSpeed = 1.0f;

        private ClimbInteraction m_climbedObject = null;

        //-----------------------------
        // Others
        //-----------------------------
        private Rigidbody m_rigidbody;
        private State m_state;
        private MovementType m_movementType;

        private LinkedList<Interaction> m_interactableObjects = new LinkedList<Interaction>();
        private CarryInteraction m_carriedObject = null;
        private Collider m_collider = null;

        private Inventory m_inventory;
        private Animator m_animator;
        #endregion

        #region properties=========================================================================
        private Slingshot slingshot
        {
            get
            {
                return m_inventory ? m_inventory.slingshot : null;
            }
        }
        public State state
        {
            set
            {
                //Don't do anything if the state is the same
                if (m_state == value)
                    return;

                //If the old state is climbing, re-enable gravity
                if (m_state == State.kClimbing)
                {
                    //Enable gravity
                    m_rigidbody.useGravity = true;

                    //Temporary fix: Reset the character rotation
                    Debug.LogWarning("The character is rotated back 90 degree since the animation for climbing isn't correct");
                    transform.Rotate(new Vector3(90, 0, 0));
                }

                //Set the character animation speed
                if (m_animator != null)
                    m_animator.speed = 1.0f;

                //Change to new state
                m_state = value;

                //Set the unit animation
                if (m_animator != null)
                    m_animator.SetInteger("State", (int)value);

                //If the new state is climbing, disable gravity
                if (value == State.kClimbing)
                {
                    //Disable gravity
                    m_rigidbody.useGravity = false;

                    //Make the character look toward the climbed object
                    Vector3 facingDirection = climbedObject.GetComponent<Collider>().ClosestPoint(transform.position);
                    transform.LookAt(facingDirection);

                    //Temporary fix
                    Debug.LogWarning("The character is rotated 90 degree temporarily since the animation for climbing isn't correct");
                    transform.Rotate(new Vector3(-90, 0, 0));
                }
            }
            get
            {
                return m_state;
            }
        }

        public Vector2 horizontalPosition
        {
            get { return transform.position.GetHorizontal(); }
        }

        public ClimbInteraction climbedObject
        {
            get
            {
                return m_climbedObject;
            }
        }

        public CarryInteraction carriedObject
        {
            set
            {
                //If the character has an object being picked up, drop that object
                if (m_carriedObject != null)
                {
                    //Perform dropping carried object
                    m_carriedObject.transform.position = transform.position + (unitRadius + m_carriedObject.objectCollider.bounds.extents.z) * transform.forward;
                    m_carriedObject.transform.parent = null;
                    m_carriedObject.OnBeingDropped();
                }

                m_carriedObject = value;

                //If the new picked up object is not null
                if (m_carriedObject != null)
                {
                    //Set the object rotation to identity
                    m_carriedObject.transform.localRotation = Quaternion.identity;

                    //Set the object position to be on top of the unit
                    Vector3 carriedObjectPosition = carriedObject.transform.position;
                    carriedObjectPosition.x = transform.position.x;
                    carriedObjectPosition.y = carriedObjectPosition.y + (m_collider.bounds.max.y - m_carriedObject.objectCollider.bounds.min.y) + m_carriedObject.objectCollider.contactOffset;
                    carriedObjectPosition.z = transform.position.z;
                    m_carriedObject.transform.position = carriedObjectPosition;

                    //Set the object to be a child of the carrying unit
                    m_carriedObject.transform.parent = transform;
                    
                    //Call event On Being Carried
                    m_carriedObject.OnBeingCarried(this);
                }
            }
            get
            {
                return m_carriedObject;
            }
        }

        private float unitRadius
        {
            get
            {
                return m_collider.bounds.extents.z;
            }
        }

        public float height
        {
            get
            {
                return m_collider.bounds.size.y;
            }
        }

        public bool isGrounded
        {
            get
            {
                return (m_state == State.kIdle || m_state == State.kMoving);
            }
        }

        public bool isOnTheAir
        {
            get
            {
                return (m_state == State.kFallingDown || m_state == State.kJumping);
            }
        }

        public MovementType movementType
        {
            set
            {
                m_movementType = value;
            }

            get
            {
                return m_movementType;
            }
        }
        #endregion

        #region methods============================================================================
        /// <summary>
        /// Perform action jump
        /// </summary>
        public void Jump()
        {
            //If the unit still have jump left and is not carrying an object
            if (m_jumpCount > 0 && carriedObject == null)
            {
                //Perform the jump
                m_rigidbody.velocity = Vector3.up * m_jumpVelocity;
                state = State.kJumping;

                ////Reset the jump animation
                //if (m_jumpCount < m_maxJumpCount)
                //    m_animator.Play(0, -1, 0.0f);

                --m_jumpCount;
            }

        }

        /// <summary>
        /// Set the character state to falling down, in which the fall multiplier is applied
        /// </summary>
        public void FastFall()
        {
            if (state == State.kJumping) 
                state = State.kFallingDown;
        }

        /// <summary>
        /// Perform a horizontal movement
        /// </summary>
        /// <param name="direction"> The direction of the movement </param>
        /// <param name="maxSpeed"> The maximumn speed of the movement </param>
        /// <param name="lerpTime"> The time it takes to reach maximum movement speed from standstill</param>
        private void Move(Vector2 direction, float maxSpeed, float lerpTime)
        {
            //If the direction is unspecified, stop moving
            if (direction.x == 0 && direction.y == 0)
            {
                if (isGrounded == true)
                    state = State.kIdle;

                if (isOnTheAir)
                {
                    m_currentHorizontalSpeed = 0;
                }
                else
                {
                    m_currentHorizontalSpeed = m_currentHorizontalSpeed - m_decelerateSpeed * Time.deltaTime;
                    m_currentHorizontalSpeed = Mathf.Max(0, m_currentHorizontalSpeed);
                    Vector2 horizontalVelocity = m_rigidbody.velocity.GetHorizontal().normalized * m_currentHorizontalSpeed;
                    m_rigidbody.velocity = new Vector3(horizontalVelocity.x, m_rigidbody.velocity.y, horizontalVelocity.y);
                }

                return;
            }

            if (isGrounded == true)
                state = State.kMoving;

            //Calculate the new speed
            m_currentHorizontalSpeed += (maxSpeed / lerpTime) * Time.deltaTime;
            m_currentHorizontalSpeed = Mathf.Clamp(m_currentHorizontalSpeed, 0, maxSpeed);

            //Handle walking on the ground
            Vector2 groundVelocity = direction.normalized * m_currentHorizontalSpeed;
            m_rigidbody.velocity = new Vector3(groundVelocity.x, m_rigidbody.velocity.y, groundVelocity.y);

            //Look toward the direction
            transform.forward = new Vector3(direction.x, 0, direction.y);
        }

        /// <summary>
        /// Move the unit to certain direction
        /// </summary>
        /// <param name="direction"> The direction of the movement</param>
        public void Move(Vector2 direction)
        {
            //If the unit is carrying an object, force movement speed to carrying object speed
            if (carriedObject != null)
            {
                Move(direction, m_carryingWalkSpeed, m_walkLerpTime);
                return;
            }

            //Set the character movement speed depending on it's movement type
            switch(m_movementType)
            {
                case MovementType.kWalking:
                    Move(direction, m_walkingSpeed, m_walkLerpTime);
                    break;
                case MovementType.kRunning:
                    Move(direction, m_runningSpeed, m_runLerpTime);
                    break;
                default:
                    Debug.LogAssertion("Unexpected movement type");
                    break;
            }
        }

        /// <summary>
        /// Climb to the specified direction
        /// </summary>
        /// <param name="direction"> the direction of the climb </param>
        public void Climb(Vector2 direction)
        {
            //Only do this if the unit state is climbing
            if (state == State.kClimbing)
            {
                if (direction == Vector2.zero)
                    m_animator.speed = 0.0f;
                else
                    m_animator.speed = 1.0f;

                Vector3 movementDirection = direction;
                m_rigidbody.velocity = movementDirection.normalized * m_climbingSpeed;
            }
        }

        /// <summary>
        /// Interact with an object
        /// </summary>
        public void Interact()
        {
            //Get the first object in the set
            if (m_interactableObjects.First != null)
            {
                m_interactableObjects.First.Value.TriggerInteraction(this);
            }
            //If there is no interactable object within area, try to drop item being picked up by the player
            else if (carriedObject != null)
            {
                DropCarriedObject();
            }
        }

        /// <summary>
        /// Drop the object carried by the unit if possible
        /// </summary>
        private void DropCarriedObject()
        {
            //If there is a collider blocking th location where the object is dropped, don't drop the carried object
            if (Physics.Raycast(transform.position, transform.forward, unitRadius + m_carriedObject.GetComponent<Collider>().bounds.size.z, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                Debug.LogWarning("Unable to drop an object");
                return;
            }
            
            carriedObject = null;
        }

        /// <summary>
        /// Climb a climbable object
        /// </summary>
        /// <param name="climbedObj"> Object being climbed </param>
        public void ToggleClimb(ClimbInteraction climbedObject)
        {
            //If the unit is not climbing anything, set the object being climbed
            if (m_climbedObject == null)
            {
                m_climbedObject = climbedObject;
                m_currentHorizontalSpeed = 0;
                m_rigidbody.velocity = Vector3.zero;
                state = State.kClimbing;
            }
            else
            {
                m_climbedObject = null;
                state = State.kFallingDown;
            }
        }

        /// <summary>
        /// Aim the slingshot to specified target
        /// </summary>
        /// <param name="targetPosition"> The position of the target </param>
        public void Aim(Vector3 targetPosition, bool showIndicator = true)
        {
            //This function doesn't do anything if there is no AimUI
            if (AimUI.instance == null)
                return;

            //Draw slingshot aim UI if the character has slingshot
            if (slingshot != null && showIndicator)
            {
                AimUI.instance.SetTarget(transform.position, targetPosition);
                AimUI.instance.show = true;
            }
            else
            {
                AimUI.instance.show = false;
            }
        }

        /// <summary>
        /// Use slingshot to shoot at target position
        /// </summary>
        /// <param name="targetPosition"> The position of the target</param>
        public void Shoot(Vector3 targetPosition)
        {
            if (slingshot != null)
            {
                slingshot.Shoot(targetPosition);
            }
        }

        /// <summary>
        /// Indicate that the interactable is within this unit interaction area
        /// </summary>
        public void RegisterInteractable(Interaction interactable)
        {
            //The object shouldn't be already in the list
            Assert.IsFalse(m_interactableObjects.Contains(interactable));

            //Add the object to interactable list
            m_interactableObjects.AddLast(interactable);

            //Show / Hide Interaction UI
            if (m_interactableObjects.First != null)
                Dialogue_Variables.s_buttonPromt.InitiatePrompt("E", interactable.interactionName);
            else
                Dialogue_Variables.s_buttonPromt.HidePrompt();
        }

        /// <summary>
        /// Indicate that the interactable is no longer within this unit interaction area
        /// </summary>
        public void UnregisterInteractable(Interaction interactable)
        {
            //The object should exist in the list
            Assert.IsTrue(m_interactableObjects.Contains(interactable));

            //Remove the object from the interactable list
            m_interactableObjects.Remove(interactable);

            //Show / Hide Interaction UI
            if (m_interactableObjects.First != null)
                Dialogue_Variables.s_buttonPromt.InitiatePrompt("E", interactable.interactionName);
            else
                Dialogue_Variables.s_buttonPromt.HidePrompt();
        }


        #endregion

        #region MonoBehaviour======================================================================
        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            m_inventory = GetComponent<Inventory>();
            m_collider = GetComponent<Collider>();
            m_animator = gameObject.GetComponent<Animator>();

            m_jumpCount = m_maxJumpCount;

        }

        private void FixedUpdate()
        {
            //If the character is falling down, set the state to FallingDown
            if (m_rigidbody.velocity.y < -0.1f && state == State.kJumping)
                state = State.kFallingDown;

            //Apply fall multiplier to the unit
            if (state == State.kFallingDown)
                m_rigidbody.velocity += Vector3.up * Physics.gravity.y * (m_fallMultiplier - 1) * Time.deltaTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (isOnTheAir)
            {
                foreach (ContactPoint contactPoint in collision.contacts)
                {
                    Vector3 normal = contactPoint.normal;

                    //Calculate the angle between the normal of contact point and the normal of a horizontal ground
                    float cosAngle = Vector3.Dot(normal, Vector3.up);

                    if (cosAngle > 0 && cosAngle <= 1)
                    {
                        state = State.kIdle;

                        //Set the character jump count
                        m_jumpCount = m_maxJumpCount;
                    }
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (state == State.kMoving)
            {
                if (collision.contacts.Length > 0)
                {
                    //Handle unit step offset
                    float highestContactOffset = m_collider.bounds.min.y;

                    foreach (ContactPoint contactPoint in collision.contacts)
                    {
                        highestContactOffset = Mathf.Max(highestContactOffset, contactPoint.point.y);
                    }

                    float offsetDifference = highestContactOffset - m_collider.bounds.min.y;

                    if (offsetDifference < m_stepOffset)
                    {
                        Vector3 position = transform.position;
                        position.y += offsetDifference;

                        transform.position = position;
                    }
                }
            }
        }

        #endregion

    }
}