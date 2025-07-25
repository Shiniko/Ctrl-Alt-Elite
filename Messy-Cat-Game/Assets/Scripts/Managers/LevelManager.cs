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

    [Header("Making Messes")]
    public int currentMesses;
    public GameObject exitPortal;                                                   //reference to exit portal, the portal can be anything, like a door or window, but essential turns on the interact part of it

    [Header("References")]
    [SerializeField] private LevelMusicController levelMusicController;         //refernce to level music controller
    [SerializeField] private ProgressionManager progressionManager;            //reference to Progression Manager script
    [SerializeField] private GameManager gameManager;                            //reference to Game Manager script
    [SerializeField] private DevLevelSelect devLevelSelect;                     //reference to Dev Level Select script
    public LevelDetails levelDetails;                                           //reference to Level Details script
    [SerializeField] private GameObject player;                                //reference to player object
    [SerializeField] private MakeShiftCatController catController;            //reference to player controller

    [SerializeField] private GameObject durationPanel;
    [SerializeField] private GameObject newRecordText;

    [SerializeField] private GameObject victoryStarPanel;
    [SerializeField] private GameObject messVictoryStar;
    [SerializeField] private GameObject avoidVictoryStar;
    [SerializeField] private GameObject hiddenVictoryStar;

    [Header("Triggers and Checks")]
    [SerializeField] private bool triggerDuration;
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

            if (catController == null)
            {
                {
                    if (player.GetComponent<MakeShiftCatController>() != null)
                    {
                        catController = player.GetComponent<MakeShiftCatController>();
                    }
                }
            }
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

            if (!triggerDuration)
            {
                if (durationPanel != null)
                {
                    durationPanel.SetActive(true);
                }

                triggerDuration = true;
            }

            if (countDuration)
            {
                levelDuration += Time.deltaTime * 1000f;

                if (gameManager != null)
                {
                    gameManager.AdjustDurationUI(levelDuration);
                }

                if(triggerDuration && durationPanel != null && !durationPanel.activeInHierarchy)
                {
                    triggerDuration = false;
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
                //for testing
                AddMessStar();

                //for testing
                AddHiddenStar();

                //for testing
                LevelVictory();
            }
        }
        else
        {
            if (durationPanel != null)
            {
                durationPanel.SetActive(false);
            }
        }
    }

    //accessible functions

    public void MakeAMess()
    {
        if (!triggerMessGain)
        {
            triggerMessGain = true;

            currentMesses++;

            if (progressionManager != null)
            {
                progressionManager.AddMess();

                if (currentMesses >= totalMessNeeded)
                {
                    currentMesses = totalMessNeeded;

                    AddMessStar();

                    if (!exitRevealed)
                    {
                        exitRevealed = true;

                        RevealExit();
                    }
                }
            }

            triggerMessGain = false;
        }
    }

    //other functions

    private void RevealExit()
    {
        if(exitPortal != null)
        {
            exitPortal.SetActive(true);
        }
    }

    private void AddMessStar()
    {
        if (!triggerMessStar)  //saves from trying to add more than one
        {
            messMade = true;

            triggerMessStar = true;  //after first time, no more

            hasMessStar = true;         //sets the star in level manager as true

            if (progressionManager != null)         //simple check for progression manager
            {
                progressionManager.AddStarForMessComplete();        //adds the star in progression manager, Visual UI
            }
        }
    }

    public void AddHiddenStar()
    {
        if (!triggerHiddenStar)  //saves from trying to add more than one
        {
            triggerHiddenStar = true;  //after first time, no more

            hasHiddenStar = true;         //sets the star in level manager as true

            if (progressionManager != null)         //simple check for progression manager
            {
                progressionManager.AddStarForSpecialItem();        //adds the star in progression manager, Visual UI
            }
        }
    }

    public void DogSeesCat()
    {
        if (!triggerAvoidStarLoss)  //saves from trying to remove more than one
        {
            triggerAvoidStarLoss = true;  //after first time, no more

            hasAvoidStar = false;           //sets the star in level manager as false

            if (progressionManager  != null)         //simple check for progression manager
            {
                progressionManager.RemoveStarForDog();        //removes the star in progression manager, Visual UI
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
        triggerDuration = false;

        triggerFail = false;
        triggerVictory = false;
        triggerAvoidStarLoss = false;
        triggerMessGain = false;
        triggerMessStar = false;
        triggerHiddenStar = false;

        hiddenFound = false;
        messMade = false;
        exitRevealed = false;

        DeactivateNewRecordText();

        if (progressionManager != null)
        {
            progressionManager.ResetProgress();
        }

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
        if (!triggerFail)  //saves from running victory if already failed, see LevelFail
        {
            if (!triggerVictory)  //saves from trying to do more victories
            {
                triggerVictory = true;  //after first time, no more, and also prevents a fail if victory in progress, see LevelFail function

                if(gameManager != null)
                {
                    gameManager.PlayVictoryAudio();
                }

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
    }

    public void LevelFail()
    {
        if (!triggerVictory)
        {
            triggerFail = true;  // prevents a victory if fail in progress, see LevelVictory function

            if (gameManager != null)
            {
                gameManager.PlayFailAudio();
            }

            //set active to true, if want to see time even if fail
            if (durationPanel != null)
            {
                durationPanel.SetActive(false);
            }

            //set active to true, if want to see stars even if fail, note that the coroutine display stars is what activates them on delay for each, for visuals
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

        if (progressionManager != null)
        {
            progressionManager.AddStarForDog();
        }

        if(levelMusicController != null)
        {
            levelMusicController.StartLevelMusic();
        }
    }

    public void DeactivateNewRecordText()
    {
        if (newRecordText != null)
        {
            newRecordText.SetActive(false);
        }
    }

    private void CheckDurationOfCompletion()
    {
        bool newRecord = false;

        if (gameManager != null)
        {
            newRecord = gameManager.CheckDurationCompletion(levelDuration, currentLevel);
        }

        //Debug.Log("New Record is " + newRecord);

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
                gameManager.PlayVictoryAudio();

                if (hasMessStar)
                {
                    gameManager.SaveNewStar(currentLevel, 1);
                }

                if (hasAvoidStar)
                {
                    gameManager.SaveNewStar(currentLevel, 2);
                }

                if (hasHiddenStar)
                {
                    gameManager.SaveNewStar(currentLevel, 3);
                }

                // Debug.Log("About to call SaveStars coroutine");

                StartCoroutine(DisplayStars(winDelay));

                //comment this part in if you want the pause menu to appear before and during the star animation, instead of after in coroutine, but pause may prevent the coroutine to resolve, meaning no star animations for you
                /*
                if (gameManager != null)  //call this last as it pauses game
                {
                    gameManager.VictoryLevel();
                }
                */
            }
        }        
    }

    IEnumerator DisplayStars(float delay)  //delay for animating Stars
    {
        //Debug.Log("Starting SavesStars Coroutine");

        yield return new WaitForSeconds(delay * 0.1f);

        if (hasMessStar)
        {
            messVictoryStar.SetActive(true);
        }

        //Debug.Log("about to wait for avoid star");

        yield return new WaitForSeconds(delay * 0.35f);

        if (hasAvoidStar)
        {
            avoidVictoryStar.SetActive(true);
        }

        //Debug.Log("about to wait for hidden star");

        yield return new WaitForSeconds(delay * 0.35f);

        if (hasHiddenStar)
        {
            hiddenVictoryStar.SetActive(true);

            gameManager.SaveNewStar(currentLevel, 3);
        }

        //Debug.Log("about to wait for pause");  //comment this back in if you want the menu buttons to wait until after stars populate
      
        yield return new WaitForSeconds(delay * 0.35f);

        if (gameManager != null)  //call this last as it pauses game
        {
            if (triggerVictory)
            {
                gameManager.VictoryLevel();
            }
            else
            {
                if (triggerFail)
                {
                    gameManager.FailLevel();
                }
            }
        }
        
    }
}
