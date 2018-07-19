using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThiefTale;

public class Temp_Pickup : MonoBehaviour
{
	[SerializeField] private ParticleSystem m_Particle;

	private void OnTriggerEnter(Collider other)
	{
		PlayerController player = other.GetComponent<PlayerController>();
		if(player != null)
		{
			Instantiate(m_Particle, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}