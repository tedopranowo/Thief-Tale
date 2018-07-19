using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPrompt : MonoBehaviour
{
    [SerializeField]
    private GameObject
        m_promptPanel;

    [SerializeField]
    private Text
        m_button,
        m_action;

    public bool
        m_isActive;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void InitiatePrompt(string l_button, string l_action)
    {
        m_button.text = l_button;
        m_action.text = l_action;
        m_promptPanel.SetActive(true);
        m_isActive = true;
    }

    public void HidePrompt()
    {
        if (m_promptPanel != null)
        {
            m_promptPanel.SetActive(false);
            m_isActive = false;
        }
    }
}
