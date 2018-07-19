using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThiefTale;

public class PuzzlePiece_FountainPedestal : PuzzlePiece
{
	//Component References-----------------------------------------------------//
	private Transform m_SnapPoint;

	//Object references--------------------------------------------------------//
	private Transform m_Statue;
	private Trigger_ButtonPrompt m_buttonPrompt;

	//Config-------------------------------------------------------------------//
	[SerializeField] private int m_CorrectRotationIndex = 0;
	[Range(2, 8)]
	[SerializeField] int m_RotationSteps = 4;
	[SerializeField] float m_RotationSeconds = 0.5f;

	//Stats--------------------------------------------------------------------//
	[SerializeField] private bool m_HasStatue = false;
	private bool m_CanInteract = true;
	[SerializeField] private int m_CurrentRotationIndex = 0;

	private void Start()
	{
		m_SnapPoint = transform.GetChild(0);
		m_buttonPrompt = GetComponent<Trigger_ButtonPrompt>();
	}

	public override void TriggerInteraction(Character unit)
	{
		if (m_Statue != null)
		{
			if (m_CanInteract)
			{
				StartCoroutine(LerpRotation());
				if (m_CurrentRotationIndex < m_RotationSteps - 1)
				{
					m_CurrentRotationIndex++;
				}
				else
				{
					m_CurrentRotationIndex = 0;
				}
				//m_buttonPrompt.OutsideTrigger();
			}
		}
		else if (unit.carriedObject != null && unit.carriedObject.name.Contains("Statue"))
		{
			m_Statue = unit.carriedObject.transform;
			unit.carriedObject.enabled = false;
			unit.carriedObject = null;

			m_Statue.parent = m_SnapPoint;
			m_Statue.localPosition = Vector3.zero;
			m_Statue.rotation = transform.rotation;

			m_HasStatue = true;
		}

		if (m_CurrentRotationIndex == m_CorrectRotationIndex)
		{
			m_IsReady = true;
		}
		m_Puzzle.CheckForSolution();
	}

	private IEnumerator LerpRotation()
	{
		m_CanInteract = false;

		Quaternion startRot = m_Statue.rotation;
		Quaternion targetRot = Quaternion.Euler(0, startRot.eulerAngles.y + (360 / m_RotationSteps), 0);

		float timer = 0;

		while (timer < m_RotationSeconds)
		{
			timer += Time.deltaTime;

			m_Statue.rotation = Quaternion.Lerp(startRot, targetRot, timer / m_RotationSeconds);

			yield return null;
		}

		m_CanInteract = true;
	}

	protected override void OnUnitEnterInteractionArea(Character unit)
	{
		if(unit.carriedObject != null)
		{
			m_HasStatue = true;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.GetComponent<PlayerController>())
		{
			if (m_HasStatue)
			{
				m_buttonPrompt.OutsideTrigger();

			}
		}
	}
}
