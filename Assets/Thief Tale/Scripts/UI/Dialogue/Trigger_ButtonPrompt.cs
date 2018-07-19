using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThiefTale;

public class Trigger_ButtonPrompt : MonoBehaviour
{

    private ButtonPrompt
        m_buttonPrompt;

    [SerializeField]
    private string
        m_button,
        m_action;

    [SerializeField]
    private KeyCode
        m_keyCode;

    [SerializeField]
    private bool
        m_isDialogueSystem,
        m_deactivateOnExecution,
        m_swapOnExecution,
        m_hasCondition,
        m_triggerOnInput;

    [SerializeField]
    private string
        m_buttonSwap,
        m_actionSwap;

    private bool
        m_canInteract,
        m_deactivated;

    private Dialogue_System
        m_dialogueSys;


	void Start ()
    {
        m_buttonPrompt = Dialogue_Variables.s_buttonPromt;

        if (m_isDialogueSystem)
        {
            m_dialogueSys = GetComponent<Dialogue_System>();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!m_hasCondition && m_canInteract)
        {
            if (Input.GetKeyDown(m_keyCode) && m_buttonPrompt.m_isActive)
            {
                if (m_deactivateOnExecution)
                {
                    m_buttonPrompt.HidePrompt();
                }

                if (m_swapOnExecution)
                {
                    m_buttonPrompt.InitiatePrompt(m_buttonSwap, m_actionSwap);
                }

                if (m_triggerOnInput)
                {
                    OutsideTrigger();
                }
            }

            if (m_isDialogueSystem && m_dialogueSys.m_deativateTrigger && !m_deactivated)
            {
                m_buttonPrompt.HidePrompt();
                m_deactivated = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null && !m_hasCondition)
        {
            if (m_isDialogueSystem)
            {
                if (!m_dialogueSys.m_deativateTrigger)
                {
                    m_buttonPrompt.InitiatePrompt(m_button, m_action);
                }
            }
            else
            {
                m_buttonPrompt.InitiatePrompt(m_button, m_action);
            }

            m_canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null && !m_hasCondition)
        {
            m_buttonPrompt.HidePrompt();
            m_canInteract = false;
        }
    }

    private void OnDisable()
    {
        m_buttonPrompt.HidePrompt();
    }


    public void OutsideTrigger()
    {
        m_hasCondition = false;

        if (m_isDialogueSystem)
        {
            if (!m_dialogueSys.m_deativateTrigger)
            {
                m_buttonPrompt.InitiatePrompt(m_button, m_action);
            }
        }
        else
        {
            m_buttonPrompt.InitiatePrompt(m_button, m_action);
        }

        m_canInteract = true;
    }
}
