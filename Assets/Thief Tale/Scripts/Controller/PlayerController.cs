//PlayerController.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale
{
    [RequireComponent(typeof(Character))]
    public class PlayerController : MonoBehaviour
    {
        #region fields=============================================================================
        private static PlayerController s_instance;

        private Character m_character;
        #endregion

        #region properties=========================================================================
        /// <summary>
        /// Return the PlayerController at index 0
        /// </summary>
        public static PlayerController instance
        {
            get
            {
                return s_instance;
            }
        }

        #endregion

        #region methods============================================================================
        /// <summary>
        /// Handle jumping input
        /// </summary>
        private void HandleJump()
        {
            if (Input.GetButtonDown(Constant.Button.kJump))
                m_character.Jump();

            if (Input.GetButtonUp(Constant.Button.kJump))
                m_character.FastFall();
        }

        /// <summary>
        /// Handle movement input
        /// </summary>
        private void HandleMovement()
        {
            float horizontal = Input.GetAxisRaw(Constant.Button.kHorizontalMovement);
            float vertical = Input.GetAxisRaw(Constant.Button.kVerticalMovement);
            bool isRunning = Input.GetButton(Constant.Button.kRun);

            if (m_character.state == Character.State.kClimbing)
            {
                m_character.Climb(new Vector2(horizontal, vertical));
            }
            else
            {
                m_character.movementType = isRunning ? Character.MovementType.kRunning : Character.MovementType.kWalking;
                m_character.Move(new Vector2(horizontal, vertical));
            }
        }

        /// <summary>
        /// Handle interaction input
        /// </summary>
        private void HandleInteraction()
        {
            if (Input.GetButtonDown(Constant.Button.kInteract))
                m_character.Interact();
        }
        
        /// <summary>
        /// Handle fire input
        /// </summary>
        private void HandleUsingWeapon()
        {
            //If the fire button is held down
            if (Input.GetButton(Constant.Button.kFire) || Input.GetButtonUp(Constant.Button.kFire))
            {
                //Convert mouse position from screenspace to world space
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 mouseTarget = new Vector3(0, 0, 0);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                {
                    mouseTarget = hit.point;

                    if (Input.GetButton(Constant.Button.kFire))
                        m_character.Aim(mouseTarget);

                    if (Input.GetButtonUp(Constant.Button.kFire))
                    {
                        m_character.Aim(mouseTarget, false);
                        m_character.Shoot(mouseTarget);
                    }
                }
                else
                {
                    m_character.Aim(mouseTarget, false);
                }
            }
        }

        #endregion

        #region MonoBehaviour======================================================================
        private void Awake()
        {
            //If there is already a PlayerController in the game, destroy this object
            if (s_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            //Set this object as the singleton
            s_instance = this;

            //Set this object layer as Player
            gameObject.layer = LayerMask.NameToLayer("Player");
            
            //Set field references
            m_character = GetComponent<Character>();
        }

        private void Update()
        {
            HandleJump();
            HandleMovement();
            HandleInteraction();
            HandleUsingWeapon();
        }

        private void OnDestroy()
        {
            s_instance = null;
        }
        #endregion
    }
}
