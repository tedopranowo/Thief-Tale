using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThiefTale;

public class Temp_Destructable : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<Projectile>() != null)
		{
			Destroy(gameObject);
		}
	}
}
