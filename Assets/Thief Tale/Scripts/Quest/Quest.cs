using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
	public static Quest instance;

	private void Awake()
	{
		if(instance != null)
		{
			Destroy(this);
		}
		else
		{
			instance = this;
		}
	}
}
