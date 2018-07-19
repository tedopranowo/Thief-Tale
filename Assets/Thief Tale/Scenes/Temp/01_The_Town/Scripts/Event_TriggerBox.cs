using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ThiefTale;

public class Event_TriggerBox : MonoBehaviour
{

    [SerializeField] private UnityEvent m_event;

    [SerializeField]
    private bool
        m_onEnter,
        m_onExit,
        m_requiresAction;

    private bool
        m_inTrigger;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && m_inTrigger)
        {
            m_event.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null && m_onEnter)
        {
            m_event.Invoke();
        }
        m_inTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null && m_onExit)
        {
            m_event.Invoke();
        }
        m_inTrigger = false;
    }


}
