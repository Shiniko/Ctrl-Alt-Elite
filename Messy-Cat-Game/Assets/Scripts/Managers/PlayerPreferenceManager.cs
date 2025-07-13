using UnityEngine;

public class PlayerPreferenceManager : MonoBehaviour
{
    [SerializeField] public int setPrefsCount;
    [SerializeField] public bool hasSetPrefs;
    [SerializeField] public bool hasTriggeredPrefs;

    [Header("Music Prefs")]
    [SerializeField] private float musicVolume;
    [SerializeField] private float sfxVolume;

    [Header("Level Codes")]
    [SerializeField] private int doneTutorialCode;

    [Header("Level Group Strings")]
    [SerializeField] private string levelCompleteGroup1;
    [SerializeField] private string levelCompleteGroup2;
    [SerializeField] private string levelCompleteGroup3;
    [SerializeField] private string levelCompleteGroup4;
    [SerializeField] private string levelCompleteGroup5;

    [Header("Level Bools")]
    [SerializeField] private bool hasDoneTutorial;

    [Header("Level Stars")]
    [SerializeField] private int numberOfLevels;
    [SerializeField] private int[] levelStars;

    [Header("Level Best Times")]
    [SerializeField] private float[] levelCompleteDurations;

    void Start()
    {
        levelStars = new int[numberOfLevels];
    }

    void Update()
    {
        if (!hasTriggeredPrefs)
        {
            hasTriggeredPrefs = true;

            if (PlayerPrefs.HasKey("DoneTutorialCode"))
            {
                LoadPlayerPrefs();
            }
            else
            {
                SetPlayerPrefs();
            }
        }

        if(setPrefsCount >= 3)
        {
            hasSetPrefs = true;
        }
    }

    private void LoadPlayerPrefs()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume");

        doneTutorialCode = PlayerPrefs.GetInt("DoneTutorialCode");

        levelCompleteGroup1 = PlayerPrefs.GetString("LevelCompleteGroup1");
        levelCompleteGroup2 = PlayerPrefs.GetString("LevelCompleteGroup2");
        levelCompleteGroup3 = PlayerPrefs.GetString("LevelCompleteGroup3");
        levelCompleteGroup4 = PlayerPrefs.GetString("LevelCompleteGroup4");
        levelCompleteGroup5 = PlayerPrefs.GetString("LevelCompleteGroup5");

        CheckCodes();

        LoadStars();

        LoadCompletionDurations();
    }

    private void SetPlayerPrefs()
    {
        PlayerPrefs.SetInt("DoneTutorialCode", doneTutorialCode);

        PlayerPrefs.SetString("LevelCompleteGroup1", "A0000000");
        PlayerPrefs.SetString("LevelCompleteGroup2", "B0000000");
        PlayerPrefs.SetString("LevelCompleteGroup3", "C0000000");
        PlayerPrefs.SetString("LevelCompleteGroup4", "D0000000");
        PlayerPrefs.SetString("LevelCompleteGroup5", "E0000000");

        hasSetPrefs = true;
        setPrefsCount = 0;
    }

    private void CheckCodes()
    {
        if(doneTutorialCode == 1234567)
        {
            hasDoneTutorial = true;
        }

        setPrefsCount++;
    }

    private void LoadStars()
    {
        for (int i = 0; i < levelCompleteGroup1.Length; i++)
        {
            if (i > 0)
            {
                char currentChar = levelCompleteGroup1[i];
                int currentValue = levelCompleteGroup1[i];

                if (int.TryParse(currentChar.ToString(), out currentValue))
                {
                    levelStars[i-1] = currentValue;
                }
                else
                {
                    Debug.Log("Found a non numerical value when expecting integer");
                }
            }
        }

        for (int i = 0; i < levelCompleteGroup2.Length; i++)
        {
            if (i > 0)
            {
                char currentChar = levelCompleteGroup2[i];
                int currentValue = levelCompleteGroup2[i];

                if (int.TryParse(currentChar.ToString(), out currentValue))
                {
                    levelStars[i + 6] = currentValue;
                }
                else
                {
                    Debug.Log("Found a non numerical value when expecting integer");
                }
            }
        }

        for (int i = 0; i < levelCompleteGroup3.Length; i++)
        {
            if (i > 0)
            {
                char currentChar = levelCompleteGroup3[i];
                int currentValue = levelCompleteGroup3[i];

                if (int.TryParse(currentChar.ToString(), out currentValue))
                {
                    levelStars[i + 13] = currentValue;
                }
                else
                {
                    Debug.Log("Found a non numerical value when expecting integer");
                }
            }
        }

        for (int i = 0; i < levelCompleteGroup4.Length; i++)
        {
            if (i > 0)
            {
                char currentChar = levelCompleteGroup4[i];
                int currentValue = levelCompleteGroup4[i];

                if (int.TryParse(currentChar.ToString(), out currentValue))
                {
                    levelStars[i + 20] = currentValue;
                }
                else
                {
                    Debug.Log("Found a non numerical value when expecting integer");
                }
            }
        }

        for (int i = 0; i < levelCompleteGroup5.Length; i++)
        {
            if (i > 0)
            {
                char currentChar = levelCompleteGroup5[i];
                int currentValue = levelCompleteGroup5[i];

                if (int.TryParse(currentChar.ToString(), out currentValue))
                {
                    levelStars[i + 27] = currentValue;
                }
                else
                {
                    Debug.Log("Found a non numerical value when expecting integer");
                }
            }
        }

        setPrefsCount++;
    }

    private void LoadCompletionDurations()
    {
        //to do: add way to load completion times

        setPrefsCount++;
    }

    public void SaveTutotrialAsComplete()
    {
        doneTutorialCode = 1234567;

        PlayerPrefs.SetInt("DoneTutorialCode", doneTutorialCode);
    }
}
