using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Accesible Params")]
    public bool levelActive;
    public bool catHidden;
    public bool dogSeenCat;
    public bool humanSeenCat;

    public int currentLevel;

    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DevLevelSelect devLevelSelect;
    public LevelDetails levelDetails;
    public Transform spawnPoint;

    [Header("Triggers and Checks")]
    [SerializeField] private bool triggerFail;
    [SerializeField] private bool triggerVictory;
    [SerializeField] private bool triggerAvoidStarLoss;
    [SerializeField] private bool triggerMessGain;
    [SerializeField] private bool triggerMessStar;
    [SerializeField] private bool triggerHiddenStar;

    [SerializeField] private bool hiddenFound;
    [SerializeField] private bool messMade;
    [SerializeField] private bool exitRevealed;

    [Header("Level Duration Params")]
    public float levelDuration;
    [SerializeField] private bool countDuration;

    void Update()
    {
        if (gameManager == null)
        {
            if (GameObject.FindGameObjectWithTag("GameController") != null)
            {
                gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            }
        }

        if (devLevelSelect != null)
        {
            currentLevel = devLevelSelect.currentLevel;
        }

        if (levelActive)
        {
            if (countDuration)
            {
                levelDuration += Time.deltaTime * 1000f;

                if (gameManager != null)
                {
                    gameManager.AdjustDurationUI(levelDuration);
                }
            }
        }
    }

    public void LevelVictory()
    {
        if(gameManager != null)
        {
            gameManager.VictoryLevel();
        }
    }

}
