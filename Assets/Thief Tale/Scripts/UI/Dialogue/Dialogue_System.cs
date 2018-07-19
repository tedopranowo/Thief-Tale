using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ThiefTale;

public class Dialogue_System : MonoBehaviour
{
    private Text
        m_dialogueText,
        m_nameText;

    private GameObject
        m_dialoguePanel;

    private Image
        m_portraitImage,
        m_nameTag,
        m_ePrompt;

    public TextAsset
        m_textFile;

    public int
        m_repeatLine = 1;

    private float
        m_typingSpeed = 0.03f;

    public List<Dialogue_Box> m_changeList = new List<Dialogue_Box>();
    
    public string[] m_textLines;
    private int 
        m_currentLine,
        m_lastLine;

    private bool
        m_isTyping = false,
        m_cancelTyping = false,        
        m_isInTrigger = false;

    public bool
        m_autoTrigger,
        m_deativateTrigger = false;

    private Camera
        m_mainCamera,
        m_previousCamera;

    private PlayerController
        m_playCtrl;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && m_isInTrigger)
        {
            if (!m_deativateTrigger)
            {
                InitiateDialog();
            }
        }
    }


    // Use this for initialization
    void Start ()
    {
        m_mainCamera = Camera.main;
        m_textLines = (m_textFile.text.Split('\n'));
        m_lastLine = m_textLines.Length - 1;

        m_dialogueText = Dialogue_Variables.s_dialogueText;
        m_nameText = Dialogue_Variables.s_nameText;
        m_dialoguePanel = Dialogue_Variables.s_dialoguePanel;
        m_portraitImage = Dialogue_Variables.s_portrait;
        m_nameTag = Dialogue_Variables.s_nameTag;
        m_ePrompt = Dialogue_Variables.s_ePrompt;
    }
	

    public void InitiateDialog()
    {       
        if (!m_isTyping)
        {
            if (m_currentLine > m_lastLine)
            {
                m_dialoguePanel.SetActive(false);
                m_playCtrl.enabled = true;

                if (m_previousCamera != null)
                {
                    m_previousCamera.enabled = false;
                }


                if (m_repeatLine == 99)
                {
                    //Disable the dialog box
                    //gameObject.SetActive(false);
                    m_deativateTrigger = true;
                }
                else
                {
                    m_currentLine = m_repeatLine - 2;
                }
                
            }
            else
            {
                m_dialoguePanel.SetActive(true);
                m_playCtrl.enabled = false;

                StartCoroutine(TextScroll(m_textLines[m_currentLine]));

                // If portrait image changes, update sprite
                if (m_changeList[m_currentLine].m_chracter != null)
                {
                    m_portraitImage.sprite = m_changeList[m_currentLine].m_chracter;
                }

                // If name changes, update name
                if (m_changeList[m_currentLine].m_name != "")
                {
                    m_nameText.text = m_changeList[m_currentLine].m_name;
                }

                // If has audio, play auidio
                if (m_changeList[m_currentLine].m_audio != null)
                {
                    //m_portraitImage.sprite = m_changeList[m_currentLine].m_chracter;
                    Dialogue_Variables.s_audioSource.PlayOneShot(m_changeList[m_currentLine].m_audio);
                }

                // If camera change, change camera
                if (m_changeList[m_currentLine].m_camera != null)
                {
                    if (m_previousCamera != null)
                    {
                        m_previousCamera.enabled = false;
                    }
                    m_changeList[m_currentLine].m_camera.enabled = true;
                    m_previousCamera = m_changeList[m_currentLine].m_camera;

                }

                // If right is true, moves image to right
                if (m_changeList[m_currentLine].m_right)
                {
                    m_portraitImage.rectTransform.anchoredPosition = new Vector2(260, 88);
                    m_portraitImage.rectTransform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    m_nameTag.rectTransform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    m_ePrompt.rectTransform.anchoredPosition = new Vector2(-240, -35);
                }
                else
                {
                    m_portraitImage.rectTransform.anchoredPosition = new Vector2(-260, 88);
                    m_portraitImage.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    m_nameTag.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    m_ePrompt.rectTransform.anchoredPosition = new Vector2(240, -35);
                }

                // If right is true, moves image to right
                if (m_changeList[m_currentLine].m_questEventScript != null)
                {
                    m_changeList[m_currentLine].m_questEventScript.EventTrigger();
                }

            }

            m_currentLine++;
            //Debug.Log("Current Line is: " + m_currentLine);
        }
        else if (m_isTyping && !m_cancelTyping)
        {
            m_cancelTyping = true;
        }
    }

    private IEnumerator TextScroll(string lineOfText)
    {
        int letter = 0;
        m_dialogueText.text = "";
        m_isTyping = true;
        m_cancelTyping = false;
        //m_scribble.Play();
        while (m_isTyping && !m_cancelTyping && (letter < lineOfText.Length - 1))
        {
            if (lineOfText[letter] == '#')
            {
                m_dialoguePanel.SetActive(false);
                m_playCtrl.enabled = true;

                if (m_previousCamera != null)
                {
                    m_previousCamera.enabled = false;
                }
                break;
            }
			else if(lineOfText[letter] == '<')
			{
				int endBracketCount = 0;
				while(endBracketCount < 2)
				{
					if(lineOfText[letter] == '>')
					{
						endBracketCount += 1;
					}
					letter += 1;
				}
			}
            else
            {
                m_dialogueText.text += lineOfText[letter];
                letter += 1;
            }
            yield return new WaitForSeconds(m_typingSpeed);
        }
        //m_scribble.Stop();
        m_dialogueText.text = lineOfText;
        m_isTyping = false;
        m_cancelTyping = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            if (m_playCtrl == null)
            {
                m_playCtrl = other.GetComponent<PlayerController>();
            }

            if (m_autoTrigger)
            {
                InitiateDialog();
            }

            m_isInTrigger = true;
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            m_isInTrigger = false;
            m_playCtrl.enabled = true;
        }
    }
}
