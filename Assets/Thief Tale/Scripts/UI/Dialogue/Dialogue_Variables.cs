using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue_Variables : MonoBehaviour
{
    [SerializeField]
    public static Text
        s_dialogueText,
        s_nameText;

    public static Image
        s_portrait,
        s_nameTag,
        s_ePrompt;

    public static GameObject
        s_dialoguePanel;

    public static ButtonPrompt
        s_buttonPromt;

    public static AudioSource
        s_audioSource;

    [SerializeField]
    private Text
        m_dialogueText,
        m_nameText;

    [SerializeField]
    private Image
        m_portrait,
        m_nameTag,
        m_ePrompt;

    [SerializeField]
    private GameObject
    m_dialoguePanel;

    [SerializeField]
    private ButtonPrompt
        m_buttonPromt;

    [SerializeField]
    private AudioSource
        m_audioSource;

    private void Awake()
    {
        s_dialogueText = m_dialogueText;
        s_nameText = m_nameText;
        s_portrait = m_portrait;
        s_nameTag = m_nameTag;
        s_ePrompt = m_ePrompt;
        s_dialoguePanel = m_dialoguePanel;
        s_buttonPromt = m_buttonPromt;
        s_audioSource = m_audioSource;
    }
}
