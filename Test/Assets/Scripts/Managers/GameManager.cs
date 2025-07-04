using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject background;
    [SerializeField] private Color backgroundColor;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameObject player;
    //[SerializeField] private GameObject[] playerChoices;
    //public int chosenPlayerNumber;
    public GameObject chosenPlayer;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private CatController catController;
    [SerializeField] private AnimHandler playerAnim;
    [SerializeField] private GameObject pausePanel;

    [Header("Functional Params")]
    [SerializeField] private float quitDelay;
    [SerializeField] private float winDelay;
    public bool isPaused;

    [Header("Engage Params")]
    public bool isEngaged;
    [SerializeField] private int tempAgro;
    [SerializeField] private int currentAgro;
    public GameObject[] agroItems;

    [Header("Respawn Params")]
    [SerializeField] private bool isDead;
    [SerializeField] private bool isRespawning = true;
    [SerializeField] private bool hasSpawnedPlayer;
    [SerializeField] private float respawnCounter;
    [SerializeField] private float respawnCD;
    public int lastPlayerWaypoint;

    [Header("UI Health")]
    [SerializeField] private GameObject playerPanel;
    [SerializeField] private GameObject healEffect;
    [SerializeField] private Image currentPlayerHP;
    [SerializeField] private Image oldPlayerHP;
    [SerializeField] private Image newPlayerHP;
    [SerializeField] private float shortenCounter;
    [SerializeField] private float shortenCD;
    [SerializeField] private float shortenRate;
    [SerializeField] private float growCounter;
    [SerializeField] private float growCD;
    [SerializeField] private float growRate;

    [Header("UI Params")]
    [SerializeField] private GameObject attackPanel;
    public bool isOverUI;
    public int attackSelection;
    public bool activeAttackCountdown;
    [SerializeField] private GameObject attackDisable;
    public TextMeshProUGUI[] attackDisplayCounters;
    public Image[] currentAttackCDs;
    public Image[] attackIcons;
    public GameObject[] attackIconSelectors;
    public float attackUICounter;
    public float attackUICD;
    [SerializeField] private int lastAttackCounterAmount;

    [Header("Preference Params")]
    public bool hasSetPreferences;
    [SerializeField] private bool hasLoadedPrefs;

    //optional
    [Header("References")]
    [SerializeField] private GameObject[] playerChoices;
    public int chosenPlayerNumber;

    public string bossName;
    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private GameObject bossPanel;
    [SerializeField] private GameObject bossHealEffect;
    [SerializeField] private Image currentBossHP;
    [SerializeField] private Image oldBossHP;
    [SerializeField] private Image newBossHP;
    [SerializeField] private float bossShortenCounter;
    [SerializeField] private float bossShortenCD;
    [SerializeField] private float bossShortenRate;
    [SerializeField] private float bossGrowCounter;
    [SerializeField] private float bossGrowCD;
    [SerializeField] private float bossGrowRate;

    [Header("Bonus Params")]
    public bool hasSetBonuses;
    [SerializeField] private int playerLevel;
    [SerializeField] private float bonusMaxHP;
    [SerializeField] private float bonusRegen;
    [SerializeField] private float bonusArmor;
    [SerializeField] private float bonusMaxHPRate;
    [SerializeField] private float bonusRegenRate;
    [SerializeField] private float bonusArmorRate;
    [SerializeField] private int playerJumpCountBonus;
    [SerializeField] private bool hasGlide;
    [SerializeField] private bool hasSlide;
    [SerializeField] private bool hasWallGrab;
    [SerializeField] private bool hasGrapple;
    [SerializeField] private int levelCode;
    [SerializeField] private int glideCode;
    [SerializeField] private int slideCode;
    [SerializeField] private int jumpCode;

    void Awake()
    {
        //ResetPlayerPrefs();
    }

    void Start()
    {
        //shortenCounter = shortenCD;

        if (PlayerPrefs.HasKey("ChosenPlayer"))
        {
            if (!hasLoadedPrefs)
            {
                LoadPlayerPrefs();
            }
        }
        else
        {
            SetPlayerPrefs();
        }

        //attackSelection = 1;

        //ChangeAttackSelection(attackSelection);

        if (audioManager == null)
        {
            audioManager = FindFirstObjectByType<AudioManager>();
        }
    }

    public void FlashBG()
    {
        if (background != null)
        {
            if (background.GetComponent<FlashImage>() != null)
            {
                background.GetComponent<FlashImage>().StartFlash(1, 1, backgroundColor);
            }
        }
    }

    void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    void SetPlayerPrefs()
    {
        //PlayerPrefs.SetInt("ChosenPlayer", chosenPlayerNumber);
        //PlayerPrefs.SetInt("LastWayPoint", lastPlayerWaypoint);

        hasLoadedPrefs = true;

        FlashBG();

        Debug.Log("Setting prefs instead of getting prefs");
    }

    void LoadPlayerPrefs()
    {
        //chosenPlayerNumber = PlayerPrefs.GetInt("ChosenPlayer");
        //lastPlayerWaypoint = PlayerPrefs.GetInt("LastWayPoint");

        Debug.Log("lastPlayerWayPoint is loaded as " + lastPlayerWaypoint);

        /*
        if (chosenPlayerNumber > 0 && chosenPlayerNumber <= playerChoices.Length)
        {
            chosenPlayer = playerChoices[chosenPlayerNumber - 1];

            if (spawnManager != null)
            {
                spawnManager.playerToSpawn = chosenPlayer;
                spawnManager.SetSpawners();
                spawnManager.SetInitialSpawner(lastPlayerWaypoint);
            }
        }
        */

        hasLoadedPrefs = true;

        FlashBG();
    }

    void SavePlayerPrefs()
    {
        //PlayerPrefs.SetInt("ChosenPlayer", chosenPlayerNumber);

        /*
        if (prefSaver != null)
        {
            prefSaver.SavePrefs();
        }
        */
    }

    public void SwitchPlayer(int num)
    {
        /*
        if (num >= 0 && num < playerChoices.Length)
        {
            chosenPlayerNumber = num;
            chosenPlayer = playerChoices[num];

            if (spawnManager != null)
            {
                spawnManager.playerToSpawn = chosenPlayer;
                spawnManager.SetSpawners();
            }

            PlayerPrefs.SetInt("ChosenPlayer", chosenPlayerNumber);
        }
        */
    }

    public void PlaySceneTheme(string theme)
    {
        if (audioManager != null)
        {
            if (audioManager.playingMainTheme)
            {
                audioManager.SwapMusic(theme);
            }
        }
    }

    public void PlayMainTheme(string previousTheme)
    {
        if (audioManager != null)
        {
            if (!audioManager.playingMainTheme)
            {
                audioManager.MainTheme(previousTheme);
            }
        }
    }

    public void DisplayPlayerPanels(bool active)
    {
        if (playerPanel != null)
        {
            playerPanel.SetActive(active);
        }

        if (attackPanel != null)
        {
            attackPanel.SetActive(active);
        }
    }

    void Update()
    {
        HandleRespawn();

        HandlePlayer();
    }

    private void HandleRespawn()
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

    private void SpawnPlayer()
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

    private void HandlePlayer()
    {

    }

    public void SetEngage()
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

    public void AdjustHealthUIDamage(float currentHP, float maxHP)
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

    public void AdjustHealthUIHeal(float currentHP, float maxHP)
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

    public void PlayHealEffect()
    {
        if (healEffect != null)
        {

        }
    }

    public void ApplyDeath() //called from playerhealth
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

    public void GamePausedEsc()
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

    public void UnPauseGameButton()
    {
        isPaused = false;

        ClosePanel();

        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }
    }

    public void OverUI()
    {
        isOverUI = true;

        if (catController != null)
        {
            catController.isOverUI = true;
        }
    }

    public void OpenPanel()
    {
        isOverUI = true;

        if (catController != null)
        {
            catController.isOverUI = true;
        }
    }

    public void NotOverUI()
    {
        isOverUI = false;

        if (catController != null)
        {
            catController.isOverUI = false;
        }
    }

    public void ClosePanel()
    {
        isOverUI = false;

        if (catController != null)
        {
            catController.isOverUI = false;
        }
    }

    public void QuitGame()
    {
        //Debug.Log("QUIT");
        StartCoroutine(LoadDelayQG(quitDelay));
    }

    IEnumerator LoadDelayQG(float delay)
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

    public void DeActivateBossHealth()
    {
        DisplayBossPanel(false);
    }

    public void ActivateBossHealth(EnemyHealth bossHealth)
    {
        if (bossHealth != null)
        {
            float maxBossHP = bossHealth.adjustedMaxHP;
            float currentBossHP = 0f;

            DisplayBossPanel(true);

            AdjustHealthUIBossDamage(currentBossHP, maxBossHP);

            AdjustHealthUIBossHeal(maxBossHP, maxBossHP);
        }
    }

    public void DisplayBossPanel(bool showBlock)
    {
        if (bossNameText != null)
        {
            bossNameText.text = bossName;
        }

        if (bossPanel != null)
        {
            bossPanel.SetActive(showBlock);
        }
    }

    public void DistributePlayerBonuses()
    {
        playerLevel = 1; //intial set

        //load player prefs
        playerLevel = PlayerPrefs.GetInt("PlayerLevel");
        playerJumpCountBonus = PlayerPrefs.GetInt("BonusJumpCount");

        levelCode = PlayerPrefs.GetInt("code1");
        jumpCode = PlayerPrefs.GetInt("code2");
        glideCode = PlayerPrefs.GetInt("code3");
        slideCode = PlayerPrefs.GetInt("code4");

        // check codes

        if (levelCode == 9000009 && playerLevel != 2)
        {
            playerLevel = 1;
        }

        if (levelCode == 9004009 && playerLevel != 3)
        {
            playerLevel = 1;
        }

        if (jumpCode == 6001006 && playerJumpCountBonus != 1)
        {
            playerJumpCountBonus = 0;
        }

        if (jumpCode == 2144039 && playerJumpCountBonus != 2)
        {
            playerJumpCountBonus = 0;
        }

        if (glideCode == 3079703)
        {
            hasGlide = true;
        }

        if (slideCode == 6149357)
        {
            hasSlide = true;
        }

        if (playerLevel > 1)
        {
            bonusMaxHP = playerLevel * bonusMaxHPRate;
            bonusRegen = playerLevel * bonusRegenRate;
            bonusArmor = playerLevel * bonusArmorRate;
        }
        else
        {
            bonusMaxHP = 0f;
            bonusRegen = 0f;
            bonusArmor = 0f;
        }

        /*
        if (playerController != null)
        {
            playerController.jumpMax = 1 + playerJumpCountBonus;
            playerController.hasGlide = hasGlide;
            playerController.hasGlide = hasSlide;
            playerController.hasWallGrab = hasWallGrab;
            playerController.hasWallGrab = hasGrapple;
        }
        */

        if (playerHealth != null)
        {
            playerHealth.bonusMaxHP = bonusMaxHP;
            playerHealth.bonusRegen = bonusRegen;
            playerHealth.bonusArmor = bonusArmor;
        }

        SetChosenPlayer();

        hasSetBonuses = true;
    }

    private void SetChosenPlayer()
    {
        if (chosenPlayerNumber >= 0 && chosenPlayerNumber < (playerChoices.Length))
        {
            chosenPlayer = playerChoices[chosenPlayerNumber];
        }
    }

    public void AdjustHealthUIBossDamage(float currentBHP, float maxBHP)
    {
        if (currentBHP < 0f)
        {
            currentBHP = 0f;
        }

        if (maxBHP != 0f)
        {
            currentBossHP.fillAmount = (currentBHP / maxBHP);
            newBossHP.fillAmount = currentBossHP.fillAmount;
        }

        //do the damage set
        bossShortenCounter = 0f;
    }

    public void AdjustHealthUIBossHeal(float currentBHP, float maxBHP)
    {
        if (currentBHP < 0f)
        {
            currentBHP = 0f;
        }

        if (maxBHP != 0f)
        {
            newBossHP.fillAmount = (currentBHP / maxBHP);
        }

        //do the damage set
        bossGrowCounter = 0f;
    }

    public void ChangeAttackSelection(int selection)
    {
        if (player != null)
        {
            if (selection <= 0)
            {
                selection = 1;
            }

            if (selection >= 5)
            {
                selection = 4;
            }

            attackSelection = selection;

            if (catController != null)
            {
                catController.ChangeAttackSelection(selection);
            }

            if (attackPanel != null)
            {
                attackPanel.SetActive(true);
            }

            for (int i = 0; i < attackIconSelectors.Length; i++)
            {
                if (attackIconSelectors[i] != null)
                {
                    if (i == (selection - 1))
                    {
                        attackIconSelectors[i].SetActive(true);
                    }
                    else
                    {
                        attackIconSelectors[i].SetActive(false);
                    }
                }
            }
        }

    }

    private void AdjustAttackCountDownAmount()
    {
        if (lastAttackCounterAmount > (Mathf.RoundToInt((attackUICD - attackUICounter))))
        {
            for (int i = 0; i < attackDisplayCounters.Length; i++)
            {
                if (attackDisplayCounters[i] != null)
                {
                    attackDisplayCounters[i].text = (Mathf.RoundToInt((attackUICD - attackUICounter))).ToString();
                }
            }

            lastAttackCounterAmount = Mathf.RoundToInt((attackUICD - attackUICounter));

        }

        for (int i = 0; i < currentAttackCDs.Length; i++)
        {
            if (attackUICD != 0f)
            {
                if (currentAttackCDs[i] != null)
                {
                    currentAttackCDs[i].fillAmount = ((attackUICD - attackUICounter) / attackUICD);
                }
            }
        }
    }

    public void AdjustAttackCountdown(float currentCD, float currentCount)
    {
        attackUICD = currentCD;

        if (currentCount > attackUICD)
        {
            currentCount = attackUICD;
        }

        if (currentCount < 0f)
        {
            currentCount = 0f;
        }

        attackUICounter = currentCount;
    }

    public void StartAttackCounter()
    {
        for (int i = 0; i < attackDisplayCounters.Length; i++)
        {
            if (attackDisplayCounters[i] != null)
            {
                attackDisplayCounters[i].text = (Mathf.RoundToInt((attackUICD - attackUICounter))).ToString();
            }
        }

        lastAttackCounterAmount = Mathf.RoundToInt((attackUICD - attackUICounter));

        if (attackDisable != null)
        {
            attackDisable.SetActive(true);
        }

        activeAttackCountdown = true;
    }

    public void FinishAttackCounter()
    {
        activeAttackCountdown = false;

        if (attackDisable != null)
        {
            attackDisable.SetActive(false);
        }
    }

    public void ApplyWinCon()
    {
        //you win
        if (audioManager != null)
        {
            audioManager.Play("Win");
        }

        StartCoroutine(LoadDelayWG(winDelay));
    }

    IEnumerator LoadDelayWG(float delay)
    {
        yield return new WaitForSeconds(delay);

        GamePausedEsc();
    }

}
