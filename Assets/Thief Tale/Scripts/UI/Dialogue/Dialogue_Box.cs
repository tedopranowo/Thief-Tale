using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue_Box
{
    public Sprite m_chracter;
    public string m_name = string.Empty;
    public bool m_right;
    public AudioClip m_audio;
    public Camera m_camera;
    public QuestEvent m_questEventScript;
}
