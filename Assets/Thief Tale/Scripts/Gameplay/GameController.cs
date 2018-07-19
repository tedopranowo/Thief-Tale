//GameManager.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale
{
    public class GameController : MonoBehaviour
    {
        #region fields=============================================================================
        static GameController s_instance;

        [SerializeField] private GameObject m_pauseIndicatorUI;
        [SerializeField] private GameObject m_gameOverUI;
        [SerializeField] private GameObject m_inventoryUI;
        #endregion

        #region properties=========================================================================
        private bool pause
        {
            set
            {
                if (m_pauseIndicatorUI != null)
                {
                    if (value == true)
                    {
                        Time.timeScale = 0.0f;
                    }
                    else
                    {
                        Time.timeScale = 1.0f;
                    }

                    m_pauseIndicatorUI.SetActive(value);
                }
            }
        }

        #endregion

        #region methods============================================================================
        public static void GameOver()
        {
            if (s_instance.m_gameOverUI != null)
                s_instance.m_gameOverUI.SetActive(true);
        }
        #endregion

        #region MonoBehaviour======================================================================
        private void Awake()
        {
            //If there is already an instance of GameController, destroy this object
            if (s_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            //Set up the fields
            s_instance = this;

            if (m_gameOverUI != null)
                m_gameOverUI.SetActive(false);

            if (m_pauseIndicatorUI != null)
                m_pauseIndicatorUI.SetActive(false);
        }

        private void Update()
        {
            if (m_inventoryUI != null && Input.GetButtonDown(Constant.Button.kMenu))
            {
                m_inventoryUI.SetActive(true);
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            pause = !hasFocus;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            pause = pauseStatus;
        }

        #endregion

    }
}