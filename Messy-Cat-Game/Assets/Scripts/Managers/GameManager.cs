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
    [SerializeField] private GameObject player;            //reference to player

    [SerializeField] private PlayerHealth playerHealth;    //reference to playerhealth
    [SerializeField] private CatController catController;  //reference to player controller
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
    [SerializeField] private bool isRespawning = true;     //bool used to check if player is currently respawning, so wait for now
    [SerializeField] private bool hasSpawnedPlayer;        //bool used to check if player done respawning, so now can do things
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

    [Header("Preference Params")]
    public bool hasSetPreferences;                         //bool for scripts to check if preferences have indeed benn loaded already, and so this script doesnt do it again
    [SerializeField] private bool hasLoadedPrefs;          //bool for this script to check if it has loaded prefs, so it doesnt again, and to check if game ready etc.

    void Awake()
    {
        //ResetPlayerPrefs();  //comment in when you need to clear/refresh player prefs for dev purposes 
    }

    void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();  //for dev purposes, sometimes you need to test things or need a fresh player prefs because of changes etc.
    }

    void Start()
    {
        //shortenCounter = shortenCD;

        if (PlayerPrefs.HasKey("ChosenPlayer"))  //chosen key is arbitrary, but should be something a person will have even if they did not interact with settings/options, so we know if we need to set initial keys, or load keys from from previous play
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
        //PlayerPrefs.SetInt("LastWayPoint", lastPlayerWaypoint);

        hasLoadedPrefs = true;

        FlashBG();

        Debug.Log("Setting prefs instead of loading prefs");
    }

    void LoadPlayerPrefs()  //loads player prefs if not first play
    {
        //lastPlayerWaypoint = PlayerPrefs.GetInt("LastWayPoint");

        //Debug.Log("lastPlayerWayPoint is loaded as " + lastPlayerWaypoint);

        hasLoadedPrefs = true;

        FlashBG();

        Debug.Log("Loading prefs instead of setting prefs");
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

                isRespawning = false;

                if (hasSetPreferences)
                {
                    SpawnPlayer();
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

    private void SpawnPlayer()  //function to trigger a spawn of the player via SpawnManager
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

    private void HandlePlayer()  //called every frame if needed
    {

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

    public void GamePausedEsc()  //function to pause game via player pressing escape
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

    IEnumerator LoadDelayQG(float delay)  //set game time to normal if paused, then delay applying quit, to play audio, before quitting
    {
        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }

        yield return new WaitForSeconds(delay);

#if (UNITY_STANDALONE) 
        Application.Quit();
#elif (UNITY_WEBGL)
        Application.OpenURL("about:blank");
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
