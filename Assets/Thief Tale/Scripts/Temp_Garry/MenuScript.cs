using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {
    public GameObject tutorial, title;
	// Use this for initialization
	void Start () {
        tutorial.SetActive(false);
        title.SetActive(true);
    }
	public void Tutorial()
    {
        tutorial.SetActive(true);
        title.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
