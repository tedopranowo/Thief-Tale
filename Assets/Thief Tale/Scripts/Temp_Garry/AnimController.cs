using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    private Animator m_Anim;

    public string
        a1,
        a2,
        a3,
        a4,
        a5;
    public bool
        b1,
        b2,
        b3,
        b4,
        b5;



    // Use this for initialization
    void Start ()
    {
        m_Anim = this.GetComponent<Animator>();
		m_Anim.Play(m_Anim.GetCurrentAnimatorStateInfo(0).fullPathHash, -1, Random.Range(0.0f, 1.0f));
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (b1)
        {
            m_Anim.SetBool(a1, true);
        }
        else if (b2)
        {
            m_Anim.SetBool(a2, true);
        }
        else if (b3)
        {
            m_Anim.SetBool(a3, true);
        }
        else if (b4)
        {
            m_Anim.SetBool(a4, true);
        }
        else if(b5)
        {
            m_Anim.SetBool(a5, true);
        }
        else
        {
            m_Anim.SetBool("Female_SittingTalking", true);
        }
    }
}
