//NavMeshRoute.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;
using UnityEngine.AI;

namespace ThiefTale
{
    public class NavMeshRoute : Route
    {
        #region enum===============================================================================
        private enum Type
        {
            kLoop,
            kPingPong
        }
        #endregion

        #region fields=============================================================================
        [SerializeField] private Vector3[] m_points = new Vector3[0];
        [SerializeField] private Type m_type;
        private NavMeshPath m_path;
        #endregion

        #region properties=========================================================================
        public Vector3[] points
        {
            get
            {
                return m_points;
            }
            set
            {
                m_points = value;
            }
        }
        #endregion

        #region MonoBehaviours=====================================================================
        private void Awake()
        {
            m_path = new NavMeshPath();
        }
        #endregion  

        #region Route==============================================================================
        public override int GetLength()
        {
            switch(m_type)
            {
                case Type.kLoop:
                    return m_points.Length;
                case Type.kPingPong:
                    return (m_points.Length - 1) * 2;
            }

            return 0;
        }

        public override Vector3 GetPoint(int index)
        {
            Vector3 localPosition = Vector3.zero;
            switch(m_type)
            {
                case Type.kLoop:
                    localPosition = m_points[index];
                    break;
                case Type.kPingPong:
                    if (index < m_points.Length)
                        localPosition = m_points[index];
                    else
                        localPosition = m_points[2 * m_points.Length - index - 2];
                    break;
            }

            return transform.TransformPoint(localPosition);
        }
        #endregion
    }

}
