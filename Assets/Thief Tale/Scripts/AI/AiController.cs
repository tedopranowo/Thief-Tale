//AiController.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace ThiefTale
{
    using AI;

    [RequireComponent(typeof(Character))]
    public class AiController : MonoBehaviour
    {
        #region fields=============================================================================
        private static HashSet<AiController> m_aiControllerList = new HashSet<AiController>();

        private Animator m_animator;
        private Character m_character;
        [SerializeField] private Vision m_vision;
        [SerializeField] private Route m_route;
        [SerializeField] private RuntimeAnimatorController m_aiBehaviour;

        [Tooltip("If this is true, game over will be triggered when the AI collides with the player and the player is within vision")]
        [SerializeField]
        private bool m_canCatchPlayer = true;

        Dictionary<string, object> m_exposedVariables = new Dictionary<string, object>();
        

        //---------------------------------------
        // Movement
        //---------------------------------------
        private NavMeshPath m_path;
        private Vector3 m_destination;
        private float m_nextPathUpdate = 0.0f;
        private int m_nextPathIndex;

        #endregion

        #region properties=========================================================================
        public Vision vision
        {
            get { return m_vision; }
        }

        public Route route
        {
            get { return m_route; }
        }

        public Dictionary<string, object> exposedVariables
        {
            get { return m_exposedVariables; }
        }

        public Vector3 destination
        {
            set
            {
                //If we have new destination, set the destination and recalculate the path
                if (m_destination != value)
                {
                    m_destination = value;
                    UpdatePath();
                }                
            }
            get
            {
                return m_destination;
            }
        }

        public Character controlledUnit
        {
            get
            {
                return m_character;
            }
        }
        #endregion

        #region methods============================================================================

        /// <summary>
        /// Set a new destination for the AI to move to
        /// </summary>
        /// <param name="destination"> The new AI destination </param>
        /// <returns> return true if the destination is valid </returns>
        public bool SetDestination(Vector3 destination)
        {
            if (m_destination != destination)
            {
                Vector3 oldDestination = m_destination;
                m_destination = destination;
                m_animator.SetBool(AiBehaviour.GetId(AiBehaviour.Parameter.kIsIdle), false);

                //If no path to the new destination is found
                if (!UpdatePath())
                {
                    m_destination = oldDestination;
                    return false;
                }
            }

            //Since the path 
            return true;
        }

        /// <summary>
        /// Recalculate the path for the AI to move to
        /// </summary>
        /// <returns> Return true if a path to the destination is found. Otherwise, return false </returns>
        private bool UpdatePath()
        {
            if (NavMesh.CalculatePath(transform.position, m_destination, NavMesh.AllAreas, m_path))
            {
                m_nextPathUpdate = Time.time + Config.kPathUpdateInterval;
                m_nextPathIndex = 0;
                return true;
            }

            //Path is not found
            return false;
        }

        /// <summary>
        /// Perform movement
        /// </summary>
        private void Move()
        {
            if (m_animator.GetBool(AiBehaviour.GetId(AiBehaviour.Parameter.kIsIdle)))
                return;

            //If we reach the time limit to update path, update the path
            if (Time.time > m_nextPathUpdate)
                UpdatePath();

            //If the next path index doesn't exist, don't move
            if (m_nextPathIndex >= m_path.corners.Length)
                return;

            //If we are close enough to the next path
            while ((m_path.corners[m_nextPathIndex] - transform.position).GetHorizontal().sqrMagnitude < Config.kUpdatePathDistanceLimit)
            {
                ++m_nextPathIndex;
                if (m_nextPathIndex >= m_path.corners.Length)
                {
                    m_animator.SetBool(AiBehaviour.GetId(AiBehaviour.Parameter.kIsIdle), true);
                    return;
                }
            }

            //Perform movement
            Vector2 movementDirection = (m_path.corners[m_nextPathIndex] - transform.position).GetHorizontal();

            //Process character avoidance
            //The algorithm for character avoidance is based on this article:
            //https://gamedevelopment.tutsplus.com/tutorials/understanding-steering-behaviors-collision-avoidance--gamedev-7777
            Vector2 ahead = transform.position.GetHorizontal() + movementDirection.normalized;
            foreach(AiController aiController in m_aiControllerList)
            {
                if (aiController == this)
                    continue;

                Vector2 otherPosition = aiController.transform.position.GetHorizontal();
                if ((ahead - otherPosition).sqrMagnitude < 1.0f)
                {
                    Debug.Log("Applying force");
                    //Apply forces
                    Vector2 forces = (ahead - otherPosition).normalized;

                    movementDirection += forces;
                }
            }

            m_character.Move(movementDirection);
            m_animator.SetBool(AiBehaviour.GetId(AiBehaviour.Parameter.kIsIdle), false);
        }

        /// <summary>
        /// Notify that AI that he heard a noise
        /// </summary>
        /// <param name="noiseLocation"> The location where the noise is heard</param>
        public void NotifyNoise(Vector3 noiseLocation)
        {
            m_animator.SetBool(AiBehaviour.GetId(AiBehaviour.Parameter.kHeardNoise), true);
        }

        #endregion  

        #region MonoBehaviours=====================================================================
        private void Awake()
        {
            //Add this AI to the list
            m_aiControllerList.Add(this);

            m_character = GetComponent<Character>();
            m_path = new NavMeshPath();

            //Create AI Mekanim as a child object
            GameObject AiMekanimGO = new GameObject("AI Mekanim");
            AiMekanimGO.transform.parent = transform;
            m_animator = AiMekanimGO.AddComponent<Animator>();
            m_animator.runtimeAnimatorController = m_aiBehaviour;

            //Initialize exposed variable
            m_exposedVariables.Add("Route", route);

            foreach (AiStateMachineBehaviour state in m_animator.GetBehaviours<AiStateMachineBehaviour>())
            {
                state.UpdateExposedVariables(m_exposedVariables);
            }
        }

        private void Update()
        {
            //Vision checking
            if (m_vision.IsPlayerSighted())
            {
                m_animator.SetBool(AiBehaviour.GetId(AiBehaviour.Parameter.kIsPlayerInSight), true);
                m_exposedVariables["LastPlayerSightedLocation"] = PlayerController.instance.transform.position
                    + PlayerController.instance.transform.forward; //We add the player forward so that the AI will go to where the player was moving to
                GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                m_animator.SetBool(AiBehaviour.GetId(AiBehaviour.Parameter.kIsPlayerInSight), false);
                GetComponent<Renderer>().material.color = Color.green;
            }

            //Check if the player is within caught distance
            float sqrDistanceToPlayer = (transform.position - PlayerController.instance.transform.position).sqrMagnitude;
            m_animator.SetFloat(AiBehaviour.GetId(AiBehaviour.Parameter.kSqrDistanceToPlayer), sqrDistanceToPlayer);

            //Perform movement
            Move();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (m_canCatchPlayer)
            {
                if (collision.gameObject.GetComponent<PlayerController>() != null && m_vision.IsPlayerSighted())
                {
                    GameController.GameOver();
                }
            }
        }

        private void OnDestroy()
        {
            m_aiControllerList.Remove(this);
        }

        #endregion
    }
}
