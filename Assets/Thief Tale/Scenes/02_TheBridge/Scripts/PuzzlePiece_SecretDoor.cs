using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThiefTale;

public class PuzzlePiece_SecretDoor : PuzzlePiece
{
	private Transform m_SnapPoint;

	private Trigger_ButtonPrompt m_ButtonPrompt;

	private void Start()
	{
		m_SnapPoint = transform.GetChild(0);
		m_ButtonPrompt = GetComponent<Trigger_ButtonPrompt>();
	}

	public override void TriggerInteraction(Character unit)
	{
		if(unit.carriedObject != null && unit.carriedObject.name.Contains("Key"))
		{
			Transform key = unit.carriedObject.transform;

			unit.carriedObject = null;

			key.parent = m_SnapPoint;
			key.position = m_SnapPoint.position;
			key.rotation = m_SnapPoint.rotation;

			m_IsReady = true;
		}

		m_Puzzle.CheckForSolution();
	}
}
