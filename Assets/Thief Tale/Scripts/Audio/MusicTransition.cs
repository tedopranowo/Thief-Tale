using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThiefTale;

public class MusicTransition : MonoBehaviour
{
    [SerializeField]
    private MusicController
        m_musicCtrl;

    [SerializeField]
    private AudioSource
        m_transitionAudio;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            m_musicCtrl.Transition(m_transitionAudio);
        }
    }
}
