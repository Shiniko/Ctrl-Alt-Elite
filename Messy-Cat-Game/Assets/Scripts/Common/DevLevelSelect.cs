using UnityEngine;

public class DevLevelSelect : MonoBehaviour
{
    [SerializeField] private SceneLoader sl;
    [SerializeField] private SceneLoadManager slm;
    [SerializeField] private GameObject levelSelectPanel;

    [SerializeField] private string levelSelect;
    [SerializeField] private bool _triggered;
    [SerializeField] private bool _levelSelected;
    [SerializeField] private bool _loaded;

    public bool isLevelSelect;

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
    }

    public void SelectSceneAndLoad(int levelSuffix)
    {
        levelSelect = "Level_" + levelSuffix;

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
    }
}
