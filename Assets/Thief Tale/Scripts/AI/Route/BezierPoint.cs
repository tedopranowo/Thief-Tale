//BezierPoint.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale
{
    [System.Serializable]
    public class BezierPoint
    {
        #region enums==============================================================================
        public enum Section
        {
            kLocalPosition,
            kStartTangent,
            kEndTangent,
            kCount,
            kNone = 0xff
        }
        #endregion

        #region fields=============================================================================
        [SerializeField] private Vector3 m_localPosition;
        [SerializeField] private Vector3 m_startTangent;
        [SerializeField] private Vector3 m_endTangent;
        #endregion

        #region properties=========================================================================
        public Vector3 localPosition
        {
            set { m_localPosition = value; }
            get { return m_localPosition; }
        }

        public Vector3 startTangent
        {
            set { m_startTangent = value; }
            get { return m_startTangent; }
        }

        public Vector3 endTangent
        {
            set { m_endTangent = value; }
            get { return m_endTangent; }
        }
        #endregion

        #region methods============================================================================
        public BezierPoint()
        {
            m_localPosition = Vector3.zero;
            m_startTangent = Vector3.down;
            m_endTangent = Vector3.up;
        }

        /// <summary>
        /// Constructor which copies all the element from the target
        /// </summary>
        /// <param name="other"> target to copy from </param>
        public BezierPoint(BezierPoint other)
        {
            Init();
            CopyFrom(other);
        }

        /// <summary>
        /// Copy all elements from a different BezierNode
        /// </summary>
        /// <param name="other"> The target to copy from </param>
        public void CopyFrom(BezierPoint other)
        {
            m_localPosition = other.m_localPosition;
            m_startTangent = other.m_startTangent;
            m_endTangent = other.m_endTangent;
        }

        /// <summary>
        /// Initialize the content of this object if it is empty
        /// </summary>
        public void Init()
        {
            Debug.LogWarning("To Be Removed");
        }

        /// <summary>
        /// Get the local position of a point
        /// </summary>
        /// <param name="section"> The section of the point</param>
        /// <returns> The local position of the point </returns>
        public Vector3 Get(Section section)
        {
            switch(section)
            {
                case Section.kLocalPosition:
                    return localPosition;
                case Section.kStartTangent:
                    return startTangent;
                case Section.kEndTangent:
                    return endTangent;
                default:
                    Debug.Log("Error: Section not found");
                    return Vector3.zero;
            }
        }

        /// <summary>
        /// Set the local position of a point
        /// </summary>
        /// <param name="section"> The section of the point to be set </param>
        /// <param name="value"> The local position of the point </param>
        public void Set(Section section, Vector3 value)
        {
            switch(section)
            {
                case Section.kLocalPosition:
                    localPosition = value;
                    break;
                case Section.kStartTangent:
                    startTangent = value;
                    break;
                case Section.kEndTangent:
                    endTangent = value;
                    break;
                default:
                    Debug.Log("Error: Section not found");
                    break;
            }
        }
        #endregion

    }

}