using UnityEngine;

public class LevelDetails : MonoBehaviour
{
    [Header("Accessible Params")]
    public int totalMessesNeeded;
    public int spawnIndex;

    private ProgressionManager progressionManager;
    private bool pmSet;

    private LevelManager levelManager;
    private bool lmSet;

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
                SetProgressManager();

                pmSet = true;
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
                SetLevelManager();

                lmSet = true;
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

            //progressionManager.AddStarForDog(); //chnaging this to be set in level manager for new level set, to control when it is allowed and not opverwritten by a level reset
        }
    }

    private void SetLevelManager()
    {
        if (levelManager != null)
        {
            //do stuff to set

            levelManager.spawnIndex = spawnIndex;
            levelManager.levelDetails = this;
            levelManager.totalMessNeeded = totalMessesNeeded;

            levelManager.NewLevelSet();
        }
    }
}
