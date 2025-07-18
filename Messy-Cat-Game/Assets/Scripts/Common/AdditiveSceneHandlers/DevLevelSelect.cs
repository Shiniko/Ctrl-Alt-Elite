using UnityEngine;
using UnityEngine.UI;

public class DevLevelSelect : MonoBehaviour
{
    [Header("Accesible Params")]
    public int currentLevel;
    public bool isLevelSelect;
    public bool isCredits;
    public bool hasLoadedStars = true;
    public bool hasLoadedStarImages;
    public bool hasLoadedLevelButtons = true;

    [Header("References")]
    [SerializeField] private SceneLoader sl;
    [SerializeField] private SceneLoadManager slm;
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private PlayerPreferenceManager ppm;
    [SerializeField] private StarGrabber starGrabber;

    [Header("Button References")]
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private GameObject retryButton;
    [SerializeField] private GameObject levelSelectButton;
    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject creditsButton;

    [Header("UI References")]
    [SerializeField] private GameObject victoryText;
    [SerializeField] private GameObject failText;
    [SerializeField] private GameObject creditsPanel;

    [Header("Load and Unload params")]
    [SerializeField] private string levelSelect;
    [SerializeField] private bool _triggered;
    [SerializeField] private bool _levelSelected;
    [SerializeField] private bool _loaded;

    [Header("Stars Params")]
    [SerializeField] private int[] finishStars;                           //level star for mess completion value to 1 or 0, 1 is earned, 0 is not
    [SerializeField] private int[] avoidStars;                           //level star for dog avoidance value to 1 or 0, 1 is earned, 0 is not
    [SerializeField] private int[] hiddenStars;                          //level star for get hidden item value to 1 or 0, 1 is earned, 0 is not

    [SerializeField] private Image[] finishStarImages;                           //level star image for mess completion value to 1 or 0, 1 is earned, 0 is not
    [SerializeField] private Image[] avoidStarImages;                           //level star image for dog avoidance value to 1 or 0, 1 is earned, 0 is not
    [SerializeField] private Image[] hiddenStarImages;                          //level star image for get hidden item value to 1 or 0, 1 is earned, 0 is not

    [SerializeField] private Color darkColor;
    [SerializeField] private Color lightColor;

    [Header("Levels Params")]
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private bool hasSetLevelButtons;

    void Start()
    {
        ActivateLevelSelectMenuButtons();
    }

    void Update()
    {
        if (_triggered) //after triggering, want to wait a frame in order to set LevelSelect scene for loading
        {
            if (!_loaded)
            {
                _loaded = true;

                sl.LoadScenes();
            }
        }

        if (!_triggered)
        {
            if(sl != null)
            {
                //setting scenesToLoad array as single levelSelect string
                string[] levelSelectScene = new string[1];
                levelSelectScene[0] = levelSelect;
                sl.SetScenesToLoad(levelSelectScene);

                _triggered = true;

                ActivateLevelSelectPanel();
            }
        }

        if (!hasLoadedStars)
        {
            LoadStars();
        }

        if (!hasLoadedLevelButtons)
        {
            CheckLevels();
        }
    }

    public void SelectSceneAndLoad(int levelSuffix)
    {
        if(levelSuffix < 0 || levelSuffix > 35)
        {
            return;
        }

        levelSelect = "Level_" + levelSuffix;
        currentLevel = levelSuffix;

        if (!_levelSelected)
        {
            _levelSelected = true;

            if (sl != null)
            {
                //setting scenesToLoad array as single level string
                string[] levelSelectScene = new string[1];
                levelSelectScene[0] = levelSelect;
                sl.SetScenesToLoad(levelSelectScene);

                DeActivateLevelSelectPanel();

                DeActivateCreditsPanel();

                ActivateNewLevelMenuButtons();

                slm.LoadScene();

                //setting scenesToUnload array as single levelSelect string
                string[] levelUnloadScene = new string[1];
                levelUnloadScene[0] = "LevelSelect";
                sl.SetScenesToUnLoad(levelUnloadScene);

                slm.UnLoadScene();
            }
        }
    }

    public void LoadLevelSelectScene()
    {
        //setting scenesToLoad array as single level string
        string[] levelSelectScene = new string[1];
        levelSelectScene[0] = "LevelSelect";
        sl.SetScenesToLoad(levelSelectScene);

        if (!isLevelSelect)
        {
            ActivateLevelSelectPanel();

            ActivateLevelSelectMenuButtons();

            slm.LoadScene();
        }
        else
        {

            ActivateNewLevelMenuButtons();

            slm.LoadLevelSelectScene();

        }

        //setting scenesToUnload array as single levelSelect string

        if (!isLevelSelect)
        {
            string[] levelUnloadScene = new string[1];
            levelUnloadScene[0] = levelSelect;
            sl.SetScenesToUnLoad(levelUnloadScene);

            slm.UnLoadScene();
        }
        else
        {
            slm.DeActivateLoadPanel();
        }
    }

    public void LoadCreditsScene()
    {
        //setting scenesToLoad array as single level string
        string[] levelSelectScene = new string[1];
        levelSelectScene[0] = "Credits";
        sl.SetScenesToLoad(levelSelectScene);

        if (!isCredits)
        {
            ActivateCreditsPanel();

            ActivateCreditsMenuButtons();

            slm.LoadScene();
        }

        //Setting Unload array

        if (!isCredits)
        {
            string[] levelUnloadScene = new string[1];
            levelUnloadScene[0] = levelSelect;
            sl.SetScenesToUnLoad(levelUnloadScene);

            slm.UnLoadScene();
        }
        else
        {
            slm.DeActivateLoadPanel();
        }
    }

    public void LevelLoaded()
    {
        _levelSelected = false;
    }

    public void ReloadSameLevel()
    {
        int level = currentLevel;

        if(level <1 || level > 35)
        {
            return;
        }

        if (sl != null)
        {
            levelSelect = "Level_" + level;
            //setting scenesToLoad array as single level string
            string[] levelSelectScene = new string[1];
            levelSelectScene[0] = levelSelect;
            sl.SetScenesToLoad(levelSelectScene);
            sl.SetScenesToUnLoad(levelSelectScene);

            ActivateNewLevelMenuButtons();

            //make sure gameunpause and close pause menu and loading panel activated

            sl.UnloadSameLevel(level);
        }
    }

    public void DeActivateLevelSelectPanel()
    {
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(false);
        }

        ResetLoadStars();
    }

    public void ActivateLevelSelectPanel()
    {
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(true);
        }

        ResetLoadStars();
    }

    public void DeActivateCreditsPanel()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }

        ResetLoadStars();
    }

    public void ActivateCreditsPanel()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true);
        }

        ResetLoadStars();
    }

    private void ResetLoadStars()
    {
        hasLoadedStars = false;
        hasLoadedLevelButtons = false;

        Debug.Log("Reset Level Select Stars and Level bool for trigger");
    }

    public void LoadStars()
    {
        if(ppm != null)
        {
            if (ppm.hasSetPrefs)
            {
                if (hasLoadedStarImages)
                {
                    finishStars = ppm.finishStars;
                    avoidStars = ppm.avoidStars;
                    hiddenStars = ppm.hiddenStars;

                    //first sett all to dark color

                    for (int i = 0; i < finishStarImages.Length; i++)
                    {
                        if (finishStarImages[i] != null)
                        {
                            if (finishStars[i] == 0)
                            {
                                finishStarImages[i].color = darkColor;
                            }
                            else
                            {
                                finishStarImages[i].color = lightColor;
                            }
                        }
                    }

                    for (int i = 0; i < avoidStarImages.Length; i++)
                    {
                        if (avoidStarImages[i] != null)
                        {
                            if (avoidStars[i] == 0)
                            {
                                avoidStarImages[i].color = darkColor;
                            }
                            else
                            {
                                avoidStarImages[i].color = lightColor;
                            }
                        }
                    }

                    for (int i = 0; i < hiddenStarImages.Length; i++)
                    {
                        if (hiddenStarImages[i] != null)
                        {
                            if (hiddenStars[i] == 0)
                            {
                                hiddenStarImages[i].color = darkColor;
                            }
                            else
                            {
                                hiddenStarImages[i].color = lightColor;
                            }
                        }
                    }

                    hasLoadedStars = true;
                }
                else
                {
                    LoadStarImages();
                }
            }
            else
            {
                Debug.Log("ppm not has set prefs");
            }
        }
    }

    private void CheckLevels()
    {
        if (starGrabber != null)
        {
            if (starGrabber.levelsPopulated)
            {
                levelButtons = starGrabber.levelButtons;

                hasSetLevelButtons = true;
            }
        }

        if (hasSetLevelButtons)
        {
            if (levelButtons.Length != finishStars.Length)
            {
                Debug.Log("returning out due to mismatch of finish stars and level button lengths");

                return;
            }

            for (int i = 0; i < levelButtons.Length; i++)
            {
                if (levelButtons[i] != null)
                {
                    if (i > 0)          //skipping first level index cause level one is always playable, also checking for previous level, so 0-1 will be out of array bounds
                    {
                        if (finishStars[i-1] > 0)
                        {
                            levelButtons[i].interactable = true;
                        }
                        else
                        {
                            levelButtons[i].interactable = false;
                        }
                    }
                }
            }

            hasLoadedLevelButtons = true;
        }
    }

    public void LoadStarImages()
    {
        if(starGrabber != null)
        {
            if (starGrabber.starsPopulated)
            {
                finishStarImages = starGrabber.finishStarImages;
                avoidStarImages = starGrabber.avoidStarImages;                          
                hiddenStarImages = starGrabber.hiddenStarImages;

                hasLoadedStarImages = true;
            }
        }
    }

    public void ActivateNewLevelMenuButtons()              //menu button set for fresh load of level
    {
        if(resumeButton != null)
        {
            resumeButton.SetActive(true);
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.SetActive(false);
        }

        if (retryButton != null)
        {
            retryButton.SetActive(true);
        }

        if (levelSelectButton != null)
        {
            levelSelectButton.SetActive(true);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.SetActive(true);
        }

        if (menuButton != null)
        {
            menuButton.SetActive(true);
        }

        if (creditsButton != null)
        {
            creditsButton.SetActive(true);
        }

        if (failText != null)
        {
            failText.SetActive(false);
        }

        if (victoryText != null)
        {
            victoryText.SetActive(false);
        }

        Debug.Log("activated new level buttons");
    }

    public void ActivateFailMenuButtons()              //menu button set for fail level
    {
        if (resumeButton != null)
        {
            resumeButton.SetActive(false);
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.SetActive(false);
        }

        if (retryButton != null)
        {
            retryButton.SetActive(true);
        }

        if (levelSelectButton != null)
        {
            levelSelectButton.SetActive(true);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.SetActive(true);
        }

        if (menuButton != null)
        {
            menuButton.SetActive(false);
        }

        if (creditsButton != null)
        {
            creditsButton.SetActive(false);
        }

        if (failText != null)
        {
            failText.SetActive(true);
        }

        if (victoryText != null)
        {
            victoryText.SetActive(false);
        }

        Debug.Log("activated fail level buttons");
    }

    public void ActivateVictoryMenuButtons()              //menu button set for victory of level
    {
        if (resumeButton != null)
        {
            resumeButton.SetActive(false);
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.SetActive(true);
        }

        if (retryButton != null)
        {
            retryButton.SetActive(true);
        }

        if (levelSelectButton != null)
        {
            levelSelectButton.SetActive(true);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.SetActive(true);
        }

        if (menuButton != null)
        {
            menuButton.SetActive(false);
        }

        if (creditsButton != null)
        {
            creditsButton.SetActive(false);
        }

        if (failText != null)
        {
            failText.SetActive(false);
        }

        if (victoryText != null)
        {
            victoryText.SetActive(true);
        }

        Debug.Log("activated win level buttons");
    }

    public void ActivateLevelSelectMenuButtons()              //menu button set level select scene
    {
        if (resumeButton != null)
        {
            resumeButton.SetActive(true);
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.SetActive(false);
        }

        if (retryButton != null)
        {
            retryButton.SetActive(false);
        }

        if (levelSelectButton != null)
        {
            levelSelectButton.SetActive(false);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.SetActive(true);
        }

        if (menuButton != null)
        {
            menuButton.SetActive(true);
        }

        if (creditsButton != null)
        {
            creditsButton.SetActive(true);
        }

        if (failText != null)
        {
            failText.SetActive(false);
        }

        if (victoryText != null)
        {
            victoryText.SetActive(false);
        }

        Debug.Log("activated level select buttons");
    }

    public void ActivateCreditsMenuButtons()              //menu button set level select scene
    {
        if (resumeButton != null)
        {
            resumeButton.SetActive(true);
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.SetActive(false);
        }

        if (retryButton != null)
        {
            retryButton.SetActive(false);
        }

        if (levelSelectButton != null)
        {
            levelSelectButton.SetActive(true);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.SetActive(true);
        }

        if (menuButton != null)
        {
            menuButton.SetActive(true);
        }

        if (creditsButton != null)
        {
            creditsButton.SetActive(false);
        }

        if (failText != null)
        {
            failText.SetActive(false);
        }

        if (victoryText != null)
        {
            victoryText.SetActive(false);
        }

        Debug.Log("activated credits buttons");
    }
}
