using UnityEngine;

public class LevelDetails : MonoBehaviour
{
    public int totalMessesNeeded;

    [SerializeField] private ProgressionManager progressionManager;
    [SerializeField] private bool pmSet;
    public Transform spawnPoint;

    [SerializeField] private LevelManager levelManager;
    [SerializeField] private bool lmSet;

    void Update()
    {
        if (progressionManager == null)
        {
            if (GameObject.FindGameObjectWithTag("ProgressionManager") != null)
            {
                progressionManager = GameObject.FindGameObjectWithTag("ProgressionManager").GetComponent<ProgressionManager>();
            }
        }
        else
        {
            if (!pmSet)
            {
                pmSet = true;

                SetProgressManager(); 
            }
        }

        if (levelManager == null)
        {
            if (GameObject.FindGameObjectWithTag("LevelManager") != null)
            {
                levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
            }
        }
        else
        {
            if (!lmSet)
            {
                lmSet = true;

                SetLevelManager();
            }
        }
    }

    private void SetProgressManager()
    {
        if (progressionManager != null)
        {
            if (totalMessesNeeded > 1)
            {
                progressionManager.totalMessAmount = totalMessesNeeded;
            }
            else
            {
                totalMessesNeeded = 1;

                progressionManager.totalMessAmount = totalMessesNeeded;
            }

            progressionManager.AddStarForDog();
        }
    }

    private void SetLevelManager()
    {
        if (levelManager != null)
        {
            //do stuff to set
            if (spawnPoint != null)
            {
                levelManager.spawnPoint = spawnPoint;
            }

            levelManager.levelDetails = this;
        }
    }
}
