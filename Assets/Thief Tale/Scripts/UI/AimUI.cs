//AimUI.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale
{
    [RequireComponent(typeof(LineRenderer))]
    public class AimUI : MonoBehaviour
    {
        #region fields=============================================================================
        static private AimUI s_instance;

        private LineRenderer m_lineRenderer;
        #endregion

        #region properties========================================================================
        static public AimUI instance
        {
            get
            {
                if (s_instance == null)
                    Debug.LogWarning("AimUI instance is not found");

                return s_instance;
            }
        }
        public bool show
        {
            set
            {
                m_lineRenderer.enabled = value;
            }
            get
            {
                return m_lineRenderer.enabled;
            }
        }
        #endregion

        #region methods============================================================================
        public void SetTarget(Vector3 from, Vector3 to)
        {
            //Set the starting position as the position of the player
            m_lineRenderer.SetPosition(0, from);

            //Set the end position as the position specified
            m_lineRenderer.SetPosition(1, to);
        }
        #endregion

        #region MonoBehaviour======================================================================
        private void Awake()
        {
            //If the singleton already exist, destroy this game object
            if (s_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            //Set up the singleton
            s_instance = this;

            //Initialize line renderer reference
            m_lineRenderer = GetComponent<LineRenderer>();

            //Hide the line
            show = false;
        }

        private void OnDestroy()
        {
            s_instance = null;
        }
        #endregion
    }

}
