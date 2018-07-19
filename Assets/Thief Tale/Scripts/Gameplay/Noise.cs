//Noise.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale
{
    [System.Serializable]
    public class Noise
    {
        #region field==============================================================================
        [Tooltip("The audio that's going to be played")]
        [SerializeField]
        AudioClip m_audio;

        [Tooltip("How far the sound can be heard by the AI")]
        [SerializeField]
        float m_distance = 3.0f;

        [Tooltip("The volume of the audio played")]
        [SerializeField]
        float m_volume = 1.0f;

        #endregion

        #region methods============================================================================
        /// <summary>
        /// Play the audio
        /// </summary>
        /// <param name="position"> The position where the audio is played </param>
        public void Play(Vector3 position)
        {
            //Don't do anything if there is no audio clip
            if (m_audio == null)
                return;

            //Play the audio clip
            AudioSource.PlayClipAtPoint(m_audio, position, m_volume);

            //Notify all ai controller within certain distance that they heard a noise
            Collider[] hitColliders = Physics.OverlapSphere(position, m_distance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

            foreach(Collider collider in hitColliders)
            {
                collider.GetComponent<AiController>().NotifyNoise(position);
            }
        }
        #endregion
    }

}