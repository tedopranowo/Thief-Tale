using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThiefTale
{
	public class Puzzle : MonoBehaviour
	{
		[Tooltip("Place each PuzzlePiece GameObject here.")]
		[SerializeField] protected PuzzlePiece[] m_PuzzlePieces;
		[Tooltip("Select QuestEvent to trigger when the puzzle has been completed.")]
		[SerializeField] protected QuestEvent m_CompletionEvent;
		[Tooltip("If unchecked, external force must trigger OnSolution().")]
		[SerializeField] protected bool m_AutoTrigger = true;

		protected bool m_SolutionReady = false;
		[SerializeField] protected bool m_SolutionTriggered = false;

		protected void Awake()
		{
			foreach(PuzzlePiece piece in m_PuzzlePieces)
			{
				piece.Puzzle = this;
			}
		}

		public void CheckForSolution()
		{
			if(!m_SolutionTriggered)
			{
				bool puzzlePiecesReady = true;

				for (int i = 0; i < m_PuzzlePieces.Length; i++)
				{
					if (m_PuzzlePieces[i].IsReady == false)
					{
						puzzlePiecesReady = false;
					}
				}

				if (puzzlePiecesReady)
				{
					m_SolutionReady = true;
					if (m_AutoTrigger == true)
					{
						OnSolution();
					}
				}
			}
		}

		protected virtual void OnSolution()
		{
			m_SolutionTriggered = true;
			m_CompletionEvent.EventTrigger();
		}
	}
}
