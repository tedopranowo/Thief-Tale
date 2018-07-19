using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using ThiefTale;

public class Quest_TheBridge : Quest
{
	

	#region Fisherman Bridge
	[Header("Event - Fisherman Bridge")]
	[SerializeField] private PlayableDirector m_CutScene_FishermanBridge;
	[SerializeField] private Animator m_Fisherman_Bridge_Anim;
	[SerializeField] private GameObject m_BridgeTrigger;
	#endregion

	#region Obtain Tackle Box
	[Header("Event - Obtain Tackle Box")]
	[SerializeField] private GameObject m_Guard_Courtyard;
	[SerializeField] private GameObject m_Fisherman_Courtyard;
	#endregion

	#region Give Tackle Box
	[Header("Event - Give Tackle Box")]
	[SerializeField] private GameObject m_Tacklebox;
	#endregion

	#region SolveFountainPuzzle
	[Header("Event - Solve Fountain Puzzle")]
	[SerializeField] private PlayableDirector m_FountainCutscene;
	#endregion

	#region
	[Header("Event - Open Secret Door")]
	[SerializeField] private PlayableDirector m_SecretDoorCutscene;
	#endregion

	#region Complete Level
	[Header("Event - Complete Dountain Puzzle")]
	[SerializeField] private GameObject m_TBC_Panel;
	[SerializeField] private TempCtrlPanel m_CtrlPanel;
	#endregion

	public void QuestEventFishermanBridge()
	{
		m_CutScene_FishermanBridge.Play();
		m_Fisherman_Bridge_Anim.gameObject.GetComponent<AnimController>().enabled = false;
		m_Fisherman_Bridge_Anim.SetBool("Male_Idle", false);
		m_Fisherman_Bridge_Anim.SetBool("Male_Running", true);
		m_BridgeTrigger.GetComponent<Collider>().enabled = true;
	}

	public void QuestEventObtainTackleBox()
	{
		m_Guard_Courtyard.SetActive(true);
		m_Fisherman_Courtyard.SetActive(true);
	}

	public void QuestEventGiveTackleBox()
	{
		PlayerController.instance.GetComponent<Character>().carriedObject = null;
		m_Tacklebox.SetActive(false);
	}

	public void QuestEventSolveFountainPuzzle()
	{
		m_FountainCutscene.Play();
	}

	public void QuestEventUnlockSecretDoor()
	{
		m_SecretDoorCutscene.Play();
	}

	public void QuestEventCompleteLevel()
	{
		m_CtrlPanel.TogglePaused();
		m_TBC_Panel.SetActive(true);
	}
}
