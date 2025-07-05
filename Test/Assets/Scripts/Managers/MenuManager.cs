using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private float quitDelay;
    [SerializeField] private bool quiting;

    [SerializeField] private float playDelay;
    [SerializeField] private bool playing;

    [SerializeField] private float backDelay;
    [SerializeField] private bool backing;

    [SerializeField] private bool setting;

    public string rootSceneName;
    public string mainMenu;

    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private GameObject[] selectors;
    [SerializeField] private GameObject[] covers;
    public int selectionNumber;

    [SerializeField] private bool hasSetPreference;
    [SerializeField] private Button startGame;

    private void Start()
    {
        if (audioManager == null)
        {
            if (FindFirstObjectByType<AudioManager>() != null)
            {
                audioManager = FindFirstObjectByType<AudioManager>();
            }
        }

        if(selectionNumber <1 || selectionNumber >= selectors.Length)
        {
            selectionNumber = 1;
        }

        if (startGame != null)
        {
            startGame.interactable = false;
        }
    }

    public void SelectCharacter(int num)
    {
        if (num < 1 || num > selectors.Length)
        {
            num = 1;
        }

        for (int i = 0; i < selectors.Length; i++)
        {
            if (i == num - 1)
            {
                selectors[i].SetActive(true);
                covers[i].SetActive(false);
            }
            else
            {
                selectors[i].SetActive(false);
                covers[i].SetActive(true);
            }
        }

        selectionNumber = num;

        SetCharacterPreference();

    }

    public void SetCharacterPreference()
    {
        PlayerPrefs.SetInt("ChosenPlayer", selectionNumber);

        hasSetPreference = true;

        if (hasSetPreference)
        {
            if(startGame != null)
            {
                startGame.interactable = true;
            }
        }
    }

    public void BackToMain()
    {
        if (!backing && !quiting && !playing)
        {
            backing = true;

            StartCoroutine(LoadDelayMM(backDelay));
        }
    }

    IEnumerator LoadDelayMM(float delay)
    {
        yield return new WaitForSeconds(delay);
        //start game

        SceneManager.LoadScene(mainMenu);
    }

    public void OpenSettings()
    {
        if (!quiting && !playing && !setting)
        {
            setting = true;

            //opening settings panel

            if (settingsPanel != null)
            {
                settingsPanel.SetActive(true);
            }
        }
    }

    public void CloseSettings()
    {
        if (!quiting && !playing)
        {
            setting = false;

            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }
        }
    }

    public void PlayGame()
    {
        if (!quiting && !setting && !playing)
        {
            playing = true;

            StartCoroutine(LoadDelaySG(playDelay));
        }
    }

    public void QuitGame()
    {
        if (!quiting)
        {
            quiting = true;

            StartCoroutine(LoadDelayQG(quitDelay));
        }
    }

    IEnumerator LoadDelaySG(float delay)
    {
        yield return new WaitForSeconds(delay);
        //start game

        SceneManager.LoadScene(rootSceneName);
    }

    IEnumerator LoadDelayQG(float delay)
    {
        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }

        yield return new WaitForSeconds(delay);

#if (UNITY_STANDALONE)
        Application.Quit();
#elif (UNITY_WEBGL)
        Application.OpenURL("about:blank");
#endif

        Application.Quit();
    }
}
