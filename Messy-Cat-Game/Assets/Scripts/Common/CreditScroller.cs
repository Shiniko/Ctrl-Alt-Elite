using UnityEngine;

public class CreditScroller : MonoBehaviour
{
    [Header("Start Params")]
    public bool activateCredits;

    [Header("UI References")]
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject creditsContainer;
    [SerializeField] private GameObject[] creditsPrefabs;
    private GameObject[] creditsSpawned;

    [Header("Display Params")]
    private int creditsLength;
    private int creditCount;
    [SerializeField] private float creditsTime;
    private float creditsCounter;
    [SerializeField] private float nextCreditTime;
    private float nextCreditCounter;
    private bool endOfCredits;

    [SerializeField] private LevelMusicController levelMusicController;
    [SerializeField] private AudioManager audioManager;

    void Start()
    {
        creditsLength = creditsPrefabs.Length;
        nextCreditCounter = 3f;
        creditsSpawned = new GameObject[creditsLength];
    }

    void Update()
    {
        if (activateCredits)
        {
            InputListen();

            if (creditsCounter < creditsTime)
            {
                creditsCounter += Time.deltaTime;
            }
            else
            {
                if (creditsPanel != null)
                {
                    creditsPanel.SetActive(false);
                }
            }

            if (!endOfCredits)
            {
                if (nextCreditCounter < nextCreditTime)
                {
                    nextCreditCounter += Time.deltaTime;
                }
                else
                {
                    nextCreditCounter = 0f;

                    creditCount++;

                    if (creditCount <= creditsLength)
                    {
                        if (creditsPrefabs[creditCount - 1] != null && creditsContainer != null)
                        {
                            GameObject cp = Instantiate(creditsPrefabs[creditCount - 1], creditsContainer.transform);
                            creditsSpawned[creditCount - 1] = cp;
                        }
                    }
                    else
                    {
                        endOfCredits = true;
                    }
                }
            }
        }
    }

    public void InputListen()
    {
        if (Input.anyKey)
        {
            creditsCounter = creditsTime;
        }
    }

    public void DisableCredits()
    {
        activateCredits = false;

        for (int i = 0; i < creditsSpawned.Length; i++)
        {
            if (creditsSpawned[i] != null)
            {
                Destroy(creditsSpawned[i]);
            }
        }

        if (audioManager != null)
        {
            if (!audioManager.playingMainTheme)
            {
                audioManager.MainTheme("CreditsTheme");
            }
        }
    }

    public void StartCredits()
    {
        if (audioManager != null)
        {
            if (audioManager.playingMainTheme)  //coming from level select
            {
                audioManager.SwapMusic("CreditsTheme");
            }
            else
            {
                //means level music playing
                if (levelMusicController != null)
                {
                    levelMusicController.StopLevelMusic(); //stop leveling music to play credit later
                }

                audioManager.Stop("MutedTheme");

                audioManager.Play("CreditsTheme");
            }
        }

        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true);
        }

        creditCount = 0;
        creditsCounter = 0f;
        nextCreditCounter = 2.5f;
        endOfCredits = false;

        activateCredits = true;
    }
}

