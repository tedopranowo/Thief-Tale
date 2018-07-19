using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThiefTale
{
	public class PuzzlePiece : Interaction
	{
		protected Puzzle m_Puzzle;
		public Puzzle Puzzle
		{
			set
			{
				m_Puzzle = value;
			}
		}

		[SerializeField] protected bool m_IsReady;
		public bool IsReady
		{
			get
			{
				return m_IsReady;
			}
		}

		/// <summary>
		/// Put interaction logic for puzzle piece here. Be sure to set m_IsReady when puzzle piece is in the correct state.
		/// </summary>
		/// <param name="unit"></param>
		public override void TriggerInteraction(Character unit)
		{
			
		}
	}
}
