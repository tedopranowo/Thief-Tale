using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThiefTale;

public class Temp_TackleBox : CarryInteraction
{
	[SerializeField] private QuestEvent m_QuestEvent;
	public override void TriggerInteraction(Character unit)
	{
		base.TriggerInteraction(unit);
		m_QuestEvent.EventTrigger();
	}
}
