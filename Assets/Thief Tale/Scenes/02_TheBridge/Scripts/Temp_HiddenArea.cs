using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ThiefTale;

public class Temp_HiddenArea : MonoBehaviour
{
	[SerializeField] private Image m_Vignette;

	private void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<Character>() != null && other.GetComponent<PlayerController>() != null)
		{
			StartCoroutine(LerpVignette(0.8f));
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<Character>() != null && other.GetComponent<PlayerController>() != null)
		{
			StartCoroutine(LerpVignette(0));
		}
	}

	private IEnumerator LerpVignette(float targetAlpha)
	{
		float time = 0.25f;
		float timer = 0;

		Color startColor = m_Vignette.color;
		Color targetColor = new Color(m_Vignette.color.r, m_Vignette.color.g, m_Vignette.color.b, targetAlpha);

		while(timer < time)
		{
			timer += Time.deltaTime;

			m_Vignette.color = Color.Lerp(startColor, targetColor, timer / time);

			yield return null;
		}
	}
}
