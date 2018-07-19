using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThiefTale;

[RequireComponent(typeof(Animation))]
public class Temp_Chest : Interaction
{
	private Animation m_Anim;
	private ParticleSystem m_Particle;

	private void Start()
	{
		m_Anim = GetComponent<Animation>();
		m_Particle = GetComponent<ParticleSystem>();
	}

	public override void TriggerInteraction(Character unit)
	{
		m_Anim.Play();
		m_Particle.Play();
	}
}
