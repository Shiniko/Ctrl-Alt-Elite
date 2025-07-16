using UnityEngine;
using UnityEngine.UI;

public class DevLevelSelect : MonoBehaviour
{
    public int currentLevel;

    [SerializeField] private SceneLoader sl;
    [SerializeField] private SceneLoadManager slm;
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private PlayerPreferenceManager ppm;
    [SerializeField] private StarGrabber starGrabber;

    [SerializeField] private string levelSelect;
    [SerializeField] private bool _triggered;
    [SerializeField] private bool _levelSelected;
    [SerializeField] private bool _loaded;

    [SerializeField] private int[] finishStars;                           //level star for mess completion value to 1 or 0, 1 is earned, 0 is not
    [SerializeField] private int[] avoidStars;                           //level star for dog avoidance value to 1 or 0, 1 is earned, 0 is not
    [SerializeField] private int[] hiddenStars;                          //level star for get hidden item value to 1 or 0, 1 is earned, 0 is not

    [SerializeField] private Image[] finishStarImages;                           //level star image for mess completion value to 1 or 0, 1 is earned, 0 is not
    [SerializeField] private Image[] avoidStarImages;                           //level star image for dog avoidance value to 1 or 0, 1 is earned, 0 is not
    [SerializeField] private Image[] hiddenStarImages;                          //level star image for get hidden item value to 1 or 0, 1 is earned, 0 is not

    [SerializeField] private Color darkColor;
    [SerializeField] private Color lightColor;

    public bool isLevelSelect;
    public bool hasLoadedStars = true;
    public bool hasLoadedStarImages;

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
    }

    public void SelectSceneAndLoad(int levelSuffix)
    {
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

            slm.LoadScene();
        }
        else
        {
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
    }

    public void ActivateLevelSelectPanel()
    {
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(true);
        }

        ResetLoadStars();
    }

    private void ResetLoadStars()
    {
        hasLoadedStars = false;

        Debug.Log("Reset Level Select Stars bool for trigger");
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
}
