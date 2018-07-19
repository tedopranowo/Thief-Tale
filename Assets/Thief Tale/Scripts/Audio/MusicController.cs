using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] m_musicArray;

    [SerializeField]
    private AudioSource 
        m_defaultMusic;

    [Range(0.0f, 1.0f)]
    public float
        m_maxVolume;

    private AudioSource
        m_currentlyPlaying,
        m_transitionAudio;

    private float
        m_volume;

    private bool
        m_fadeIn = true;

    // Use this for initialization
    void Start ()
    {
        m_currentlyPlaying = m_defaultMusic;
        m_transitionAudio = m_defaultMusic;
        FadeInCurrent();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_fadeIn)
        {
            if (m_volume < m_maxVolume)
            {
                m_volume += Time.deltaTime;
            }
        }
        else
        {
            if (m_volume > 0)
            {
                m_volume -= Time.deltaTime;
            }
        }

        m_currentlyPlaying.volume = m_volume;
        
        if (m_transitionAudio.volume >= 0.0f)
        {
            m_transitionAudio.volume -= Time.deltaTime;
        }
    }

    public void FadeInCurrent()
    {
        m_fadeIn = true;
    }

    public void FadeOutCurrent()
    {
        m_fadeIn = false;
    }

    public void Transition(AudioSource l_transitionTo)
    {
        if (l_transitionTo != m_currentlyPlaying)
        {
            m_transitionAudio = m_currentlyPlaying;
            m_currentlyPlaying = l_transitionTo;
            m_volume = 0f;
            FadeInCurrent();
        }
    }
}
