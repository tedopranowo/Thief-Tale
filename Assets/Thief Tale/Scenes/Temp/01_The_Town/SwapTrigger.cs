using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_objectsOff;

    [SerializeField]
    private GameObject[] m_objectsOn;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {

        for (int i = 0; i < m_objectsOn.Length; i++)
        {
            m_objectsOn[i].SetActive(true);
        }

        for (int i = 0; i < m_objectsOff.Length; i++)
        {
            m_objectsOff[i].SetActive(false);
        }
    }
}
