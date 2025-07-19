using UnityEngine;

public class SceneLoaded : MonoBehaviour
{
    [SerializeField] private bool sceneLoaded;
    [SerializeField] private bool triggeredLoad;
    [SerializeField] private bool triggeredReady;

    [Header("References")]
    [SerializeField] private SceneLoadManager slm;
    [SerializeField] private GameManager gm;

    [SerializeField] private bool isLevelSelect;
    [SerializeField] private bool isCredits;

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

        if (gm == null)
        {
            if (GameObject.FindGameObjectWithTag("GameController") != null)
            {
                gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            }
        }

        if (slm != null)
        {
            if (!triggeredLoad)
            {
                slm.SetSceneLoaderLevelSelect(isLevelSelect);
                slm.SetSceneLoaderCredits(isCredits);

                if (sceneLoaded)
                {
                    slm.LoadedScene();

                    triggeredLoad = true;
                }        
            }
        }

        if (gm != null)
        {
            if (!triggeredReady)
            {
                triggeredReady = true;

                if (!isLevelSelect && !isCredits)
                {
                    gm.gameReady = true;
                    gm.isRespawning = true;

                    gm.SetProgressPanel(true);
                }


                if (isLevelSelect || isCredits)
                {
                    gm.gameReady = false;

                    gm.SetProgressPanel(false);
                }
            }
        }
    }
}
