using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempCtrlPanel : MonoBehaviour
{

    [SerializeField]
    private GameObject
        m_pausePanel;

    [SerializeField]
    private bool
        m_paused = true,
        m_changingScene = false;

    // Update is called once per frame
    void Update()
    {
        if (!m_changingScene)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_paused = !m_paused;
            }

            if (m_paused)
            {
                if (Time.timeScale == 1)
                {
                    m_pausePanel.SetActive(true);

                    //Cursor.visible = true;
                    //Cursor.lockState = CursorLockMode.None;
                }
                Time.timeScale = 0;
            }
            else
            {
                if (Time.timeScale == 0)
                {
                    m_pausePanel.SetActive(false);

                    //Cursor.visible = false;
                    //Cursor.lockState = CursorLockMode.Locked;
                }

                Time.timeScale = 1;
            }
        }
    }


    public void Resume()
    {
        TogglePaused();
        m_pausePanel.SetActive(false);
    }

    public void MainMenu()
    {
        m_changingScene = true;        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1;

        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void TogglePaused()
    {
        m_paused = !m_paused;
    }

}
