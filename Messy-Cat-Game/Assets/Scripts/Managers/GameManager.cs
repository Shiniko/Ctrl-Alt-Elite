using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject background;        //reference to flash background 
    [SerializeField] private Color backgroundColor;        //color value of flash background
    [SerializeField] private SpawnManager spawnManager;    //reference to spawn manager
    [SerializeField] private AudioManager audioManager;    //reference to audio manager
    [SerializeField] private PlayerPreferenceManager playerPrefsManager;       //reference to player preference manager
    [SerializeField] private ProgressionManager progressionManager;       //reference to progression manager
    [SerializeField] private LevelManager levelManager;       //reference to progression manager
    [SerializeField] private DevLevelSelect devLevelSelect;     //reference to level select manager

    [Header("Player References")]
    [SerializeField] private GameObject player;            //reference to player
    [SerializeField] private MakeShiftCatController catController;            //reference to player controller
    [SerializeField] private PlayerHealth playerHealth;    //reference to playerhealth
    [SerializeField] private AnimHandler playerAnim;       //reference to Animator Handler script, attached to player
    [SerializeField] private GameObject pausePanel;        //reference to UI panel when paused

    [Header("Functional Params")]
    [SerializeField] private float quitDelay;              //delay, in float seconds, when exiting or back to main menu
    [SerializeField] private float winDelay;               //delay, in float seconds, when win condition happens, and before pausing game, for purposes of showing UI panmel, or playing SFX, before continue 
    public bool isPaused;                                  //main bool to determine if game is paused, therefore don't update certain things that check for game pause
    public bool gameReady;                                 //bool to determine when game is ready for things, like spawning the player

    [Header("Engage Params")]
    public bool isEngaged;                                  //bool to determine when player has agro, used for animation purposes, or check if engaged in combat
    [SerializeField] private int tempAgro;                  //int determine check if or how many things has agro against the player
    [SerializeField] private int currentAgro;               //int determine how many things has agro against the player
    public GameObject[] agroItems;                          //array of things that have agro on player

    [Header("Respawn Params")]
    [SerializeField] private bool isDead;                  //bool used to check if player dead, changed from playerhealth script or respawn
    public bool setNewSpawnPoint;                              //bool used to check if new spawnpoint
    public bool hasSpawnedPlayer;
    public bool isRespawning;                               //bool used to check if player done respawning, so now can do things
    [SerializeField] private float respawnCounter;         //float, in seconds, that counts up the duration player is respawning
    [SerializeField] private float respawnCD;              //float, in seconds, that determines the full duration to delay for purposes of allowing animation of respawn, instantiation, audio, etc., before calling it good to change bools etc.
    public int lastPlayerWaypoint;                         //location of last waypoint the player reached, for purposes of respawning

    [Header("UI Health")]
    [SerializeField] private GameObject playerPanel;       //UI panel to display player specific things, like health, or status, or other indicators
    [SerializeField] private GameObject healEffect;        //Visual and/or audio effect to instantiate when healed
    [SerializeField] private Image currentPlayerHP;        //UI filled image 'bar', value set filled by current HP of player
    [SerializeField] private Image oldPlayerHP;            //UI filled image 'bar', value set filled by previous HP of player
    [SerializeField] private Image newPlayerHP;            //UI filled image 'bar', value set filled by new HP of player

                                                                //Player receiving Damage
    [SerializeField] private float shortenCounter;         //float, in seconds, of the counted duration the player's old HP is shrinking down to current HP
    [SerializeField] private float shortenCD;              //float, in seconds, of the total duration it takes the player's old HP to shrink down to current HP
    [SerializeField] private float shortenRate;            //float, to adjust rate it takes the player's old HP to shrink down to current HP

                                                                //Player receiving Heals
    [SerializeField] private float growCounter;            //float, in seconds, of the counted duration the player's new HP is growing up to current HP
    [SerializeField] private float growCD;                 //float, in seconds, of the total duration it takes the player's new HP to grow up to current HP
    [SerializeField] private float growRate;               //float, to adjust rate it takes the player's new HP to grow up to current HP

    [Header("UI Params")]
    public bool isOverUI;                                  //bool to set from UI elements, which when hovered, we want to know in order to prevent player input among other things 
    [SerializeField] private float durationLimit;               //float for max duration in milliseconds, for example, 999 hours 59 minutes 59 seconds 999 milliseconds is 3,599,999,999 milliseconds

    [Header("UI References")]
    [SerializeField] private GameObject progressPanel;      //Progression panel reference, to set active for levels, and inactive for level select or credits
    [SerializeField] private TextMeshProUGUI durationTextHour;      //TMP reference, to set level duration timer text to
    [SerializeField] private TextMeshProUGUI durationTextMinute;      //TMP reference, to set level duration timer text to
    [SerializeField] private TextMeshProUGUI durationTextSecond;      //TMP reference, to set level duration timer text to
    [SerializeField] private TextMeshProUGUI durationTextMillisecond;      //TMP reference, to set level duration timer text to
    [SerializeField] private GameObject durationPanel;      //Level Duration panel reference, to set active for levels, and inactive for level select or credits

    [Header("Preference Params")]
    public bool hasSetPreferences;                         //bool for scripts to check if preferences have indeed benn loaded already, and so this script doesnt do it again
    [SerializeField] private bool hasLoadedPrefs;          //bool for this script to check if it has loaded prefs, so it doesnt again, and to check if game ready etc.

    void Awake()
    {
        //ResetPlayerPrefs();  //comment in when you need to clear/refresh player prefs for dev purposes 
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();  //for dev purposes, sometimes you need to test things or need a fresh player prefs because of changes etc.
    }

    void Start()
    {
        //shortenCounter = shortenCD;

        if (PlayerPrefs.HasKey("DoneTutorialCode"))  //chosen key is arbitrary, but should be something a person will have even if they did not interact with settings/options, so we know if we need to set initial keys, or load keys from from previous play
        {
            if (!hasLoadedPrefs)  //no need to load prefs if you have already
            {
                LoadPlayerPrefs();  //loads player prefs and fades out flash BG
            }
        }
        else
        {
            SetPlayerPrefs();  //set player prefs and fades out flash BG
        }

        if (audioManager == null)  //just in case reference was removed or forgotten, and first attempt in web build to reference
        {
            audioManager = FindFirstObjectByType<AudioManager>();
        }
    }

    public void FlashBG()  //this function triggers the fade out for background, if the background image has the flashimage script
    {
        if (background != null)
        {
            if (background.GetComponent<FlashImage>() != null)
            {
                background.GetComponent<FlashImage>().StartFlash(1, 1, backgroundColor);
            }
        }
    }

    void SetPlayerPrefs()  //sets player prefs if first play
    {
        if(playerPrefsManager != null)
        {
            hasLoadedPrefs = playerPrefsManager.hasSetPrefs;
        }

        FlashBG();
    }

    void LoadPlayerPrefs()  //loads player prefs if not first play
    {
        if (playerPrefsManager != null)
        {
            hasLoadedPrefs = playerPrefsManager.hasSetPrefs;
        }

        FlashBG();

        //Debug.Log("Loading prefs instead of setting prefs");
    }

    void SavePlayerPrefs()  //function to be able to call a save to player prefs generally from within this script
    {
        //PlayerPrefs.SetInt("ChosenPlayer", chosenPlayerNumber);
    }

    public void PlaySceneTheme(string theme)  //function to call special audio music theme to 'swap' from main theme
    {
        if (audioManager != null)
        {
            if (audioManager.playingMainTheme)
            {
                audioManager.SwapMusic(theme);
            }
        }
    }

    public void PlayMainTheme(string previousTheme)  //function to call when switching back to main music theme
    {
        if (audioManager != null)
        {
            if (!audioManager.playingMainTheme)
            {
                audioManager.MainTheme(previousTheme);
            }
        }
    }

    public void DisplayPlayerPanels(bool active)  //dynamicaly set the player panels by passing bool, true sets active, false sets de-active
    {
        if (playerPanel != null)
        {
            playerPanel.SetActive(active);
        }
    }

    void Update()
    {
        HandleRespawn();  // calls everyframe to count duration to wait for respawn animations, etc., and spawns player if conditions are met, currently set to check if preferences set, but may need a different condition to spawn for ours

        HandlePlayer();     //calls everyframe to do player specific things, however empty atm so if its determined we dont need can remove this call and its function

        HandleCounters();  //calls everyframe to adjust time on counters for various things

        if (playerPrefsManager != null)
        {
            hasLoadedPrefs = playerPrefsManager.hasSetPrefs;
        }
    }

    public void FailLevel()
    {
        devLevelSelect.ActivateFailMenuButtons();

        GamePausedEsc();
    }

    public void PlayFailAudio()
    {
        // Play SFX
        audioManager.Play("LevelFailed");
    }

    public void VictoryLevel()
    {
        devLevelSelect.ActivateVictoryMenuButtons();

        GamePausedEsc();
    }

    public void PlayVictoryAudio()
    {
        // Play SFX
        audioManager.Play("LevelVictory");
    }

    public void SetGameReady(bool isReady)
    {
        gameReady = isReady;
    }

    public void SetIsRespawning(bool isSpawning)
    {
        isRespawning = isSpawning;
    }

    public void SetRespawnCounter(float duration)
    {
        respawnCounter = duration;
    }

    public void SetNewRespawnPoint(int newPointIndex)
    {
        //index refers to the playerSpawners of Spawn Manager, 0 is center while 1 through 4 are left sides, left to right, and 5 through 8 are right side, left to right

        if (newPointIndex < 0)
        {
            newPointIndex = 0;                                                      //setting the index to first of the array if less than zero
        }

        if (spawnManager != null)
        {
            if (newPointIndex > (spawnManager.playerSpawners.Length - 1))
            {
                newPointIndex = (spawnManager.playerSpawners.Length - 1);           //setting the index to max index if greater than the array
            }

            SpawnPlayer newSpawner = spawnManager.playerSpawners[newPointIndex]; //Spawn Player is a script, grabbing the new Spawn Player index within SpawnManager array of Spawn Players

            if (newSpawner != null)
            {
                spawnManager.ChangeSpawnPoint(newSpawner);                      //if not null, calling a Change of spawning point to the new Spawn Player - these spawn players at their position
            }
        }
    }

    private void HandleRespawn()  //explained in update
    {
        if (isRespawning)
        {
            if (respawnCounter < respawnCD)
            {
                respawnCounter += Time.deltaTime;
            }
            else
            {
                respawnCounter = respawnCD;

                if (hasSetPreferences)
                {
                    if (setNewSpawnPoint)
                    {
                        Debug.Log("new spawnPoint has been set in GM before SpawnCat is called");
                    }

                isRespawning = false;

                    if (levelManager != null)
                    {
                        SpawnCat();
                    }
                }
            }

            if (!hasSetPreferences)
            {
                if (hasLoadedPrefs)
                {
                    //chosenPlayerNumber = PlayerPrefs.GetInt("ChosenPlayer");

                    hasSetPreferences = true;
                }
            }
        }
    }

    private void SpawnCat()  //function to trigger a spawn of the player via SpawnManager
    {
        if (!hasSpawnedPlayer)
        {
            if (spawnManager != null)
            {
                spawnManager.SpawnPlayer();

                hasSpawnedPlayer = true;
            }
            else
            {
                Debug.Log("spawn manager is null");
            }
        }
    }

    public void DeSpawnPlayer()  //function to trigger a spawn of the player via SpawnManager
    {
        if (hasSpawnedPlayer)
        {
            hasSpawnedPlayer = false;

            SetGameReady(false);
            SetRespawnCounter(0f);
            SetIsRespawning(false);

            if(player != null)
            {
                Destroy(player, 0.1f);
            }
        }
    }

    public void ButtonReSpawnPlayer()  //function to trigger a spawn of the player via SpawnManager
    {
        if (!hasSpawnedPlayer)
        {
            hasSpawnedPlayer = false;

            SetGameReady(true);
            SetRespawnCounter(0f);
            SetIsRespawning(true);
        }
    }

    private void HandlePlayer()  //called every frame if needed
    {
        if(player == null)
        {
            hasSpawnedPlayer = false;

            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }

        if(player != null)
        {
            hasSpawnedPlayer = true;
        }
    }

    private void HandleCounters()
    {
        if (shortenCounter < shortenCD)
        {
            shortenCounter += Time.deltaTime;
        }
        else
        {
            shortenCounter = shortenCD;

            if (currentPlayerHP != null && oldPlayerHP != null)
            {
                if (currentPlayerHP.fillAmount < oldPlayerHP.fillAmount)
                {
                    float shortenAmount = shortenRate * Time.deltaTime;
                    oldPlayerHP.fillAmount -= shortenAmount;
                }
                else
                {
                    if (currentPlayerHP.fillAmount > oldPlayerHP.fillAmount)
                    {
                        oldPlayerHP.fillAmount = currentPlayerHP.fillAmount;
                    }
                }
            }
        }

        if (growCounter < growCD)
        {
            growCounter += Time.deltaTime;
        }
        else
        {
            growCounter = growCD;

            if (currentPlayerHP != null && newPlayerHP != null)
            {
                if (currentPlayerHP.fillAmount < newPlayerHP.fillAmount)
                {
                    float growAmount = growRate * Time.deltaTime;
                    currentPlayerHP.fillAmount += growAmount;
                }
                else
                {
                    if (currentPlayerHP.fillAmount > newPlayerHP.fillAmount)
                    {
                        newPlayerHP.fillAmount = currentPlayerHP.fillAmount;
                    }
                }
            }
        }
    }

    public void SetEngage()  //function to determine if player has drawn agro from anything, for the purposes of setting isEngaged bool, this bool is checked by AnimHandler script, to change animation states to 'in-combat' modes
    {
        tempAgro = 0;

        if (currentAgro >= 0 && currentAgro < agroItems.Length)
        {
            for (int i = 0; i < agroItems.Length; i++)
            {
                if (agroItems[i] != null)
                {
                    tempAgro++;
                }
            }

            currentAgro = tempAgro;
        }

        if (currentAgro > 0)
        {
            isEngaged = true;
        }
        else
        {
            isEngaged = false;
        }
    }

    public void AdjustHealthUIDamage(float currentHP, float maxHP)  //function to adjust player current health bar, and begin shorten duration
    {
        if (currentHP < 0f)
        {
            currentHP = 0f;
        }

        if (maxHP != 0f)
        {
            currentPlayerHP.fillAmount = (currentHP / maxHP);
            newPlayerHP.fillAmount = currentPlayerHP.fillAmount;
        }

        //do the damage set
        shortenCounter = 0f;
    }

    public void AdjustHealthUIHeal(float currentHP, float maxHP)  //function to adjust player current health bar, and begin grow duration
    {
        if (currentHP < 0f)
        {
            currentHP = 0f;
        }

        if (maxHP != 0f)
        {
            newPlayerHP.fillAmount = (currentHP / maxHP);
        }

        //do the damage set
        growCounter = 0f;
    }

    public void AdjustDurationUI(float currentDuration)  //function to adjust duration player has been in level
    {
        if (currentDuration < 0f)
        {
            currentDuration = 0f;
        }

        if (currentDuration > durationLimit)
        {
            currentDuration = durationLimit;
        }

        //do the magic

        float milliFloat = currentDuration;
        float secondFloat = currentDuration * 0.001f;
        float minuteFloat = (currentDuration * 0.001f) / 60f;
        float hourFloat = (currentDuration * 0.001f) / 3600f;
        string durationFormatted = "" + currentDuration;

        int milli = Mathf.FloorToInt(milliFloat);
        int second = 0;
        int minute = 0;
        int hour = 0;

        if (milliFloat > 999f)
        {
            while (milliFloat > 999f)
            {
                second++;

                if(second >= 60)
                {
                    second = 0;
                }

                milliFloat -= 999f;
            }
        }

        milli = Mathf.FloorToInt(milliFloat);

        if (secondFloat > 59f)
        {
            while (secondFloat > 59f)
            {
                minute++;

                if (minute >= 60)
                {
                    minute = 0;
                }

                secondFloat -= 59f;
            }
        }

        if (minuteFloat > 59f)
        {
            while (minuteFloat > 59f)
            {
                hour++;

                minuteFloat -= 59f;
            }
        }

        string hourForm = "" + hour;
        string minuteForm = "" + minute;
        string secondForm = "" + second;
        string milliForm = "" + milli;

        if (hour > 99.999)
        {
            hourForm = "" + hour;
        }
        else if (hour > 9.999)
        {
            hourForm = "0" + hour;
        }
        else
        {
            hourForm = "00" + hour;
        }

        if (minute > 9.999)
        {
            minuteForm = "" + minute;
        }
        else
        {
            minuteForm = "0" + minute;
        }

        if (second > 9.999)
        {
            secondForm = "" + second;
        }
        else
        {
            secondForm = "0" + second;
        }

        if (milli > 99.999)
        {
            milliForm = "" + milli;
        }
        else if (milli > 9.999)
        {
            milliForm = "0" + milli;
        } else
        {
            milliForm = "00" + milli;
        }

        /*
        durationFormatted = hourForm + " : " + minuteForm + " : " + secondForm + " : " + milliForm;           

        durationText.text = durationFormatted;
        */

        if (durationTextHour != null)
        {
            durationTextHour.text = hourForm; 
        }

        if (durationTextMinute != null)
        {
            durationTextMinute.text = minuteForm;
        }

        if (durationTextSecond != null)
        {
            durationTextSecond.text = secondForm;
        }

        if (durationTextMillisecond != null)
        {
            durationTextMillisecond.text = milliForm;
        }
    }

    public void PlayHealEffect()  //function to instantiate heal effect
    {
        if (healEffect != null)
        {
            //instantiate
        }
    }

    public void ApplyDeath() //called from playerhealth to apply changes, reset respawn timers, etc.
    {
        if (catController != null)
        {
            catController.ApplyDeath();
        }

        //deadbody is part of playercontroller
        respawnCounter = 0f;

        hasSpawnedPlayer = false;

        isDead = true;

        isRespawning = true;  //accepted continue assumed, but if not comment this in
        respawnCD = 5.25f;
    }

    public void GamePausedEsc()  //function to pause game via player pressing escape or menu button
    {
        isPaused = true;

        OpenPanel();

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        if (Time.timeScale != 0f)
        {
            Time.timeScale = 0f;
        }
    }

    public void UnPauseGameButton()  //function to call, from UI pause panel, via 'Resume' button
    {
        isPaused = false;

        ClosePanel();

        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }
    }

    public void OverUI()  //function to call from UI elements to determine if hovering the UI, therefore dont allow things, like movement or attack inputs
    {
        isOverUI = true;

        if (catController != null)
        {
            catController.isOverUI = true;
        }
    }

    public void OpenPanel()  //function to call when opening panels from UI elements to set isOverUI as if hovering the UI
    {
        isOverUI = true;

        if (catController != null)
        {
            catController.isOverUI = true;
        }
    }

    public void NotOverUI()   //function to call from UI elements to determine if exiting a hover from the UI, therefore re-allow things, like movement or attack inputs
    {
        isOverUI = false;

        if (catController != null)
        {
            catController.isOverUI = false;
        }
    }

    public void ClosePanel()   //function to call when closing panels from UI elements to set isOverUI as if NOT hovering the UI
    {
        isOverUI = false;

        if (catController != null)
        {
            catController.isOverUI = false;
        }
    }

    public void QuitGame()  //function to call from UI exit game or quit game buttons
    {
        //Debug.Log("QUIT");
        StartCoroutine(LoadDelayQG(quitDelay));
    }

    public void SetProgressPanel(bool showPanel)
    {
        if (progressPanel != null)
        {
            progressPanel.SetActive(showPanel);
        }
    }

    public bool CheckDurationCompletion(float durationToCheck, int level)       //checked by Level Manager to determine if duration is a new record
    {
        if(durationToCheck < 0f)
        {
            durationToCheck = 0f;
        }

        if (level < 1 || level > 35)
        {
            Debug.Log("returned false because level was not correct");

            return false;
        }

        if (playerPrefsManager != null)
        {
            bool checkPrefsForRecord = playerPrefsManager.CheckCompletionTime(level, durationToCheck);

            //Debug.Log("checked Record from prefs is " + checkPrefsForRecord);

            if (checkPrefsForRecord)
            {
                return true;
            }
            else
            {
                return false;

                //Debug.Log("returned false because check for prefs was false");
            }
        }
        else
        {
            return false;

            Debug.Log("returned false because PlayerPrefs Manager was null");
        }
    }

    public void SaveNewStar(int level, int slot)
    {
        if (playerPrefsManager != null)
        {
            playerPrefsManager.SaveNewStar(level, slot);
        }
    }

    IEnumerator LoadDelayQG(float delay)  //set game time to normal if paused, then delay applying quit, to play audio, before quitting
    {
        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }

        yield return new WaitForSeconds(delay);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
        Application.Quit();
#elif (UNITY_WEBGL)
        //Application.OpenURL("about:blank");
        Application.ExternalEval("window.open('" + "https://calcoa.itch.io/ctrl-alt-elite-messy-game" + "','_self')");
#endif

        Application.Quit();
    }

    //optional

    public void ApplyWinCon()   //call win condition, audio or effects, then coroutine to delay pause
    {
        //you win
        if (audioManager != null)
        {
            audioManager.Play("Win");
        }

        StartCoroutine(LoadDelayWG(winDelay));
    }

    IEnumerator LoadDelayWG(float delay)  //delay for pausing game after win condition met, loading win condition panels if not handled by another script
    {
        yield return new WaitForSeconds(delay);

        GamePausedEsc();
    }

}
