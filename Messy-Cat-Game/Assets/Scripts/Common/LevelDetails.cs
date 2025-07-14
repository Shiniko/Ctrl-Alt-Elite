using UnityEngine;

public class LevelDetails : MonoBehaviour
{
    public int totalMessesNeeded;

    [SerializeField] private ProgressionManager progressionManager;
    [SerializeField] private bool pmSet;

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
}
