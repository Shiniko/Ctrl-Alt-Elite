using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Duration Params")]
    public float levelDuration;
    [SerializeField] private bool countDuration;

    [Header("Accesible Params")]
    public bool levelActive;
    public bool catHidden;
    public bool dogSeenCat;
    public bool humanSeenCat;

    public int currentLevel;        //pulled from DevSelectLevel
    public int spawnIndex;          //pulled from level details within level scene

    public int totalMessNeeded;     //pulled from level details within level scene

    public bool readyForNext;       //check when done with victory, so display menu
    public float winDelay;          //delay needed to save stars and duration

    [Header("References")]
    [SerializeField] private ProgressionManager progressionManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DevLevelSelect devLevelSelect;
    public LevelDetails levelDetails;
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject durationPanel;
    [SerializeField] private GameObject newRecordText;

    [SerializeField] private GameObject victoryStarPanel;
    [SerializeField] private GameObject messVictoryStar;
    [SerializeField] private GameObject avoidVictoryStar;
    [SerializeField] private GameObject hiddenVictoryStar;

    [Header("Triggers and Checks")]
    [SerializeField] private bool triggerFail;
    [SerializeField] private bool triggerVictory;
    [SerializeField] private bool triggerAvoidStarLoss;
    [SerializeField] private bool hasAvoidStar;
    [SerializeField] private bool triggerMessGain;
    [SerializeField] private bool triggerMessStar;
    [SerializeField] private bool hasMessStar;
    [SerializeField] private bool triggerHiddenStar;
    [SerializeField] private bool hasHiddenStar;

    [SerializeField] private bool hiddenFound;
    [SerializeField] private bool messMade;
    [SerializeField] private bool exitRevealed;

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

        if (player == null)
        {
            levelActive = false;

            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }
        else
        {
            levelActive = true;
        }

        if (levelActive)
        {
            if (gameManager != null)
            {
                if (!triggerFail && !triggerVictory && !gameManager.isPaused)
                {
                    countDuration = true;
                }
                else
                {
                    countDuration = false;
                }
            }

            if (countDuration)
            {
                levelDuration += Time.deltaTime * 1000f;

                if (gameManager != null)
                {
                    gameManager.AdjustDurationUI(levelDuration);
                }
            }

            // Debug for testing level failed
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!triggerFail)
                {
                    triggerFail = true;

                    LevelFail();
                }
            }

            // Debug for testing level victory
            if (Input.GetKeyDown(KeyCode.W))
            {
                LevelVictory();
            }
        }
    }

    public void DogSeesCat()
    {
        if (!triggerAvoidStarLoss)
        {
            triggerAvoidStarLoss = true;

            hasAvoidStar = false;

            if(progressionManager  != null)
            {
                progressionManager.RemoveStarForDog();
            }
        }
    }

    public void HumanSeesCat()
    {
        if (!triggerFail)
        {
            triggerFail = true;

            LevelFail();
        }
    }

    public void ResetLevel()
    {
        currentLevel = 0;

        levelActive = false;
        catHidden = false;
        dogSeenCat = false;
        humanSeenCat = false;

        levelDuration = 0f;
        countDuration = false;

        triggerFail = false;
        triggerVictory = false;
        triggerAvoidStarLoss = false;
        triggerMessGain = false;
        triggerMessStar = false;
        triggerHiddenStar = false;

        hiddenFound = false;
        messMade = false;
        exitRevealed = false;

        if (durationPanel != null)
        {
            durationPanel.SetActive(false);
        }

        if (victoryStarPanel != null)
        {
            victoryStarPanel.SetActive(false);
        }
    }

    public void LevelVictory()
    {
        if (!triggerVictory)
        {
            triggerVictory = true;

            if (durationPanel != null)
            {
                durationPanel.SetActive(true);
            }

            CheckDurationOfCompletion();

            if (victoryStarPanel != null)
            {
                victoryStarPanel.SetActive(true);

                messVictoryStar.SetActive(false);
                avoidVictoryStar.SetActive(false);
                hiddenVictoryStar.SetActive(false);
            }

            CheckVictoryStars();
        }
    }

    public void LevelFail()
    {
        if (!triggerVictory)
        {
            if (durationPanel != null)
            {
                durationPanel.SetActive(false);
            }

            if (victoryStarPanel != null)
            {
                victoryStarPanel.SetActive(false);
            }

            if (gameManager != null)  //call this last as it pauses game
            {
                gameManager.FailLevel();
            }
        }
    }

    public void NewLevelSet()
    {
        ResetLevel();

        if(gameManager != null)
        {
            gameManager.SetNewRespawnPoint(spawnIndex);  //index refers to the playerSpawners of Spawn Manager, 0 is center while 1 through 4 are left sides, left to right, and 5 through 8 are right side, left to right
        }

        hasAvoidStar = true;
    }

    private void CheckDurationOfCompletion()
    {
        bool newRecord = false;

        if (gameManager != null)
        {
            newRecord = gameManager.CheckDurationCompletion(levelDuration, currentLevel);
        }

        Debug.Log("New Record is " + newRecord);

        if (newRecordText != null)
        {
            if (newRecord)
            {
                newRecordText.SetActive(true);
            }
            else
            {
                newRecordText.SetActive(false);
            }
        }
    }

    private void CheckVictoryStars()
    {
        if (gameManager != null)
        {
            if (hasMessStar || hasAvoidStar || hasHiddenStar)
            {
                Debug.Log("About to call SaveStars coroutine");

                StartCoroutine(SaveStars(winDelay));
            }
        }        
    }

    IEnumerator SaveStars(float delay)  //delay for animating Stars
    {
        yield return new WaitForSeconds(delay * 0.1f);

        if (hasMessStar)
        {
            messVictoryStar.SetActive(true);

            gameManager.SaveNewStar(currentLevel, 1);
        }

        yield return new WaitForSeconds(delay * 0.35f);

        if (hasAvoidStar)
        {
            avoidVictoryStar.SetActive(true);

            gameManager.SaveNewStar(currentLevel, 2);
        }

        yield return new WaitForSeconds(delay * 0.35f);

        if (hasHiddenStar)
        {
            hiddenVictoryStar.SetActive(true);

            gameManager.SaveNewStar(currentLevel, 3);
        }

        yield return new WaitForSeconds(delay * 0.35f);

        if (gameManager != null)  //call this last as it pauses game
        {
            gameManager.VictoryLevel();
        }
    }
}
