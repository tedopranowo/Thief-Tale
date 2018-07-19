using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestEvent : MonoBehaviour
{
	[SerializeField] private UnityEvent m_Event;

	public virtual void EventTrigger()
	{
		m_Event.Invoke();
	}
}
