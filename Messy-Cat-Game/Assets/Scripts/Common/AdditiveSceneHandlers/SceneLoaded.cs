using UnityEngine;

public class SceneLoaded : MonoBehaviour
{
    [SerializeField] private bool sceneLoaded;
    [SerializeField] private bool triggeredLoad;
    [SerializeField] private SceneLoadManager slm;

    [SerializeField] private bool isLevelSelect;

    void Awake()
    {
        LoadedScene();
    }

    private void LoadedScene()
    {
        sceneLoaded = true;
    }

    void Update()
    {
        if(slm == null)
        {
            if(GameObject.FindGameObjectWithTag("SceneLoadManager") != null)
            {
                slm = GameObject.FindGameObjectWithTag("SceneLoadManager").GetComponent<SceneLoadManager>();
            }
        }

        if(slm != null)
        {
            if (!triggeredLoad)
            {
                slm.SetSceneLoaderLevelSelect(isLevelSelect);  

                if (sceneLoaded)
                {
                    slm.LoadedScene();

                    triggeredLoad = true;
                }        
            }
        }
    }
}
