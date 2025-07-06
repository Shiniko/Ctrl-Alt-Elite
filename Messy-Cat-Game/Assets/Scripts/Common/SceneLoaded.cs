using UnityEngine;

public class SceneLoaded : MonoBehaviour
{
    [SerializeField] private bool sceneLoaded;
    [SerializeField] SceneLoadManager slm;

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
            if (sceneLoaded)
            {
                slm.LoadedScene();
            }
        }
    }
}
