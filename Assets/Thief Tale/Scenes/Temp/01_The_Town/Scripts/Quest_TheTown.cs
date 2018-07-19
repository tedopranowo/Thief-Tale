using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Quest_TheTown : Quest
{
    [SerializeField]
    private GameObject[] m_objectsOff;

    [SerializeField]
    private GameObject[] m_objectsOn;

    [SerializeField]
    private GameObject
        m_introCamera,
        m_dialogue03,
        m_dialogue04,
        m_dialogueChance,
        m_ball;

    [SerializeField]
    private Dialogue_System
        m_motherSonIGC,
        m_deliverymanIGC,
        m_chance;

    [SerializeField]
    private PlayableDirector
        m_climbDown,
        m_leave,
        m_knockOverCart;

    [SerializeField]
    private ParticleSystem
        m_dust;

    [SerializeField]
    private AudioSource
        m_plankSqueak;

    private bool
        m_played;

    [SerializeField]
    private Animator
        m_fadePanelAnim;

    public void IntroCamTransition()
    {
        StartCoroutine(CameraFadeCut());
    }

    IEnumerator CameraFadeCut()
    {
        m_fadePanelAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.5f);
        m_introCamera.SetActive(false);        
        m_fadePanelAnim.SetTrigger("FadeIn");
    }


    public void SwapMotherSon()
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

    public void MotherSonIGC()
    {
        m_motherSonIGC.InitiateDialog();
    }

    public void MotherSonClimbingDown()
    {
        m_climbDown.Play();
        m_dialogue03.SetActive(true);
    }

    public void TurnOffChanceBox()
    {
        //Used for setup with the delveriveryman slingshot
        m_dialogueChance.SetActive(false);
    }

    public void PickUpBall()
    {
        m_ball.SetActive(false);
        m_dialogue03.SetActive(false);
        m_dialogue04.SetActive(true);
        //Play pick up audio
    }

    public void MotherSonLeave()
    {
        m_leave.Play();
        m_objectsOn[1].GetComponent<Animator>().runtimeAnimatorController = null;
    }

    public void DeliverymanIntroIGC()
    {
        m_deliverymanIGC.InitiateDialog();
    }

    public void KnockOverCart()
    {
        if (!m_played)
        {
            m_knockOverCart.Play();
            StartCoroutine(DeliverymanChanceIGC());
            m_played = true;
            m_dialogueChance.SetActive(true);
            //m_chance.InitiateDialog();
        }
    }

    IEnumerator DeliverymanChanceIGC()
    {
        yield return new WaitForSeconds(1.0f);
        m_chance.InitiateDialog();
    }

    public void DropDust()
    {
        m_dust.Play();
        m_plankSqueak.Play();
    }
}
