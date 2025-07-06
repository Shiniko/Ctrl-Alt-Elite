using UnityEngine;

public class DevLevelSelect : MonoBehaviour
{
    [SerializeField] private SceneLoader sl;
    [SerializeField] private string levelSelect;
    [SerializeField] private bool _triggered;
    [SerializeField] private bool _loaded;

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
            }
        }
    }
}
