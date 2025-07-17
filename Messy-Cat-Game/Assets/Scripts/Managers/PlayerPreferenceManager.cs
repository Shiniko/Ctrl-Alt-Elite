using UnityEngine;

public class PlayerPreferenceManager : MonoBehaviour
{
    public int setPrefsCount;                               //count of number of functions / topics to set before calling preferences good and set
    public bool hasSetPrefs;                                //bool to announce that prefs has been set, other scripts will use this to carry on
    public bool hasTriggeredPrefs;                           //bool set so only triggers prefs once

    [Header("Music Prefs")]
    [SerializeField] private float musicVolume;             //stores musicVolume - not used for setting, is used only load/to see value / debug
    [SerializeField] private float sfxVolume;               //stores musicVolume - not used for setting, is used only load/to see value / debug

    [Header("Level Codes")]
    [SerializeField] private int doneTutorialCode;          //arbitrary code to hard check whether someone has indeed finsihed the tutorial, weakly secure, but something

    [Header("Level Bools")]
    [SerializeField] private bool hasDoneTutorial;  // bool assigned by whatever we determine a player has done to finsh learning game

    [Header("Level Stars")]
    [SerializeField] private int numberOfLevels;                //total number of levels
    [SerializeField] private int[] levelStars;                  //total numbers of stars per level
    [SerializeField] private string[] levelCompleteStars;       //string code for keeping track of star slots

    public int[] finishStars;                           //level star for mess completion value to 1 or 0, 1 is earned, 0 is not
    public int[] avoidStars;                           //level star for dog avoidance value to 1 or 0, 1 is earned, 0 is not
    public int[] hiddenStars;                          //level star for get hidden item value to 1 or 0, 1 is earned, 0 is not

    [Header("Level Best Times")]
    [SerializeField] private float[] levelCompleteDurations;            //float to be saved and loaded as duration player took to complete a level

    void Start()
    {
        levelStars = new int[numberOfLevels];                    //initialize the array with number of levels
        levelCompleteDurations = new float[numberOfLevels];      //initialize the array with number of levels
        levelCompleteStars = new string[numberOfLevels];         //initialize the array with number of levels
        finishStars = new int[numberOfLevels];                   //initialize the array with number of levels
        avoidStars = new int[numberOfLevels];                    //initialize the array with number of levels
        hiddenStars = new int[numberOfLevels];                   //initialize the array with number of levels
    }

    void Update()
    {
        
        if (!hasTriggeredPrefs)  //only set or load preferences once as initialization
        {
            hasTriggeredPrefs = true;  //once triggered no more set or load from update

            if (PlayerPrefs.HasKey("DoneTutorialCode")) //arbitrary key player should have if player prefs previously set
            {
                LoadPlayerPrefs();  //load prefs cause previously set

                Debug.Log("loading instead of setting");
            }
            else
            {
                SetPlayerPrefs();  //set prefs because first time

                Debug.Log("setting instead of loading");
            }
        }

        if(setPrefsCount >= 3)  //a count of all three pref types then set hasSet as true so other scripts can check
        {
            hasSetPrefs = true;
        }
        
    }

    private void LoadPlayerPrefs()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume");

        doneTutorialCode = PlayerPrefs.GetInt("DoneTutorialCode");

        //Debug.Log("About to call checkcodes");
        CheckCodes();

        //Debug.Log("About to call loadstars");
        LoadStars();

        //Debug.Log("About to call loadcompletion durations");
        LoadCompletionDurations();
    }

    private void SetPlayerPrefs()
    {
        PlayerPrefs.SetInt("DoneTutorialCode", doneTutorialCode);

        for (int i = 0; i < levelCompleteStars.Length; i++)
        {
            PlayerPrefs.SetString("LevelStarsCompleted" + (i + 1), "9000");
        }

        for (int i = 0; i < levelCompleteDurations.Length; i++)
        {
            PlayerPrefs.SetFloat("BestDuration"+(i+1), 0f);
        }

        hasSetPrefs = true;
        setPrefsCount = 0;

        LoadPlayerPrefs();
    }

    private void CheckCodes()
    {
        if(doneTutorialCode == 1234567)
        {
            hasDoneTutorial = true;
        }

        setPrefsCount++;  //add a pref count after checking codes, see Update() function for use
    }

    private void LoadStars()
    {
        for (int i = 0; i < levelCompleteStars.Length; i++)                          //iterate through strings in player prefs, then assign variable for parsing
        {
            levelCompleteStars[i] = PlayerPrefs.GetString("LevelStarsCompleted" + (i + 1));  //get string from player prefs
            string starNums = levelCompleteStars[i];                                 //set temp string to pares through
            int starsCounted = 0;

            for (int j = 0; j < starNums.Length; j++)                               //iterate through characters in string to parse, if a number then assign value in an array of types of stars
            {
                if (j > 0)                                                          //skip first j index, always 9, on purpose so int value not lost for rest of ints
                {
                    char currentChar = starNums[j];                                 //set a temporary char to indexed string character
                    int currentValue = starNums[j];                                 //set a temporary value to indexed string character 
                          

                    if (int.TryParse(currentChar.ToString(), out currentValue))     //check if number
                    {
                        if (j == 1)
                        {
                            finishStars[i] = currentValue;                           //set level star for mess completion value to 1 or 0, 1 is earned, 0 is not

                            if(currentValue == 1)
                            {
                                starsCounted++;
                            }

                        }

                        if (j == 2)
                        {
                            avoidStars[i] = currentValue;                           //set level star for dog avoidance value to 1 or 0, 1 is earned, 0 is not

                            if (currentValue == 1)
                            {
                                starsCounted++;
                            }
                        }

                        if (j == 3)
                        {
                            hiddenStars[i] = currentValue;                           //set level star for get hidden item value to 1 or 0, 1 is earned, 0 is not

                            if (currentValue == 1)
                            {
                                starsCounted++;
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Found a non numerical value when expecting integer");
                    }
                }
            }

            levelStars[i] = starsCounted;
        }

        setPrefsCount++;  //add a pref count after setting stars, see Update() function for use
    }

    public void RandomStar()  //for testing purposes ONLY
    {
        int randLevel = Random.Range(1, numberOfLevels + 1);
        int randSlot = Random.Range(1, 4);

        if (randLevel > numberOfLevels)
        {
            randLevel = numberOfLevels;
        }

        if (randSlot > 3)
        {
            randSlot = 3;
        }

        Debug.Log("Random Level: " + randLevel);
        Debug.Log("Random Slot: " + randSlot);

        SaveNewStar(randLevel, randSlot);
    }

    public void SaveNewStar(int level, int slot)  // old was stars instead of slot // Call this to set new star values, passing in level and the slot of star earned, 1 is mess, 2 is dog, 3 is hidden
    {
        if (level < 1 || level > numberOfLevels)
        {
            return; //does not match expected level number (1 to assigned max levels), return out
        }

        if (slot < 1 || slot > 3)
        {
            return; //does not match expected number of slots (1-3), return out
        }

        //assign new star based on level and slot
        if (PlayerPrefs.HasKey("LevelStarsCompleted" + (level)))
        {
            string parseString = PlayerPrefs.GetString("LevelStarsCompleted" + (level));
            char[] charArray = parseString.ToCharArray();
            char valueReplace = (char)('0' + 1);
            charArray[slot] = valueReplace;
            string newString = new string(charArray);

            PlayerPrefs.SetString("LevelStarsCompleted" + (level), newString);

            levelCompleteStars[level-1] = PlayerPrefs.GetString("LevelStarsCompleted" + (level));

            string starNums = levelCompleteStars[level-1];                                 //set temporary string to pares through
            int starsCounted = 0;                                                       //set a temporary counter of stars

            for (int i = 0; i < starNums.Length; i++)                               //iterate through characters in string to parse, if a number then assign value in an array of types of stars
            {
                if (i > 0)                                                          //skip first j index, always 9, on purpose so int value not lost for rest of ints
                {
                    char currentChar = starNums[i];                                 //set a temporary char to indexed string character
                    int currentValue = starNums[i];                                 //set a temporary value to indexed string character       

                    if (int.TryParse(currentChar.ToString(), out currentValue))     //check if number
                    {
                        if (i == 1)
                        {
                            finishStars[level-1] = currentValue;                           //set level star for mess completion value to 1 or 0, 1 is earned, 0 is not

                            if (currentValue == 1)
                            {
                                starsCounted++;
                            }
                        }

                        if (i == 2)
                        {
                            avoidStars[level-1] = currentValue;                           //set level star for dog avoidance value to 1 or 0, 1 is earned, 0 is not

                            if (currentValue == 1)
                            {
                                starsCounted++;
                            }
                        }

                        if (i == 3)
                        {
                            hiddenStars[level-1] = currentValue;                           //set level star for get hidden item value to 1 or 0, 1 is earned, 0 is not

                            if (currentValue == 1)
                            {
                                starsCounted++;
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Found a non numerical value when expecting integer");
                    }
                }
            }

            levelStars[level-1] = starsCounted;
        }
    }

    private void AssignStarsToPrefs(int group, string newValue)
    {
        if (group == 1)
        {
            PlayerPrefs.SetString("LevelCompleteGroup1", newValue);
        }

        if (group == 2)
        {
            PlayerPrefs.SetString("LevelCompleteGroup2", newValue);
        }

        if (group == 3)
        {
            PlayerPrefs.SetString("LevelCompleteGroup3", newValue);
        }

        if (group == 4)
        {
            PlayerPrefs.SetString("LevelCompleteGroup4", newValue);
        }

        if (group == 5)
        {
            PlayerPrefs.SetString("LevelCompleteGroup5", newValue);
        }
    }

    private void LoadCompletionDurations()
    {
        //to do: add way to load completion times

        if (PlayerPrefs.HasKey("BestDuration1"))
        {
            for (int i = 0; i < levelCompleteDurations.Length; i++)
            {
                levelCompleteDurations[i] = PlayerPrefs.GetFloat("BestDuration" + (i + 1));
            }
        }
        else
        {
            Debug.Log("Tried to load level best durations without a key to do so");
        }

        setPrefsCount++;  //add a pref count after setting completeion durations, see Update() function for use
    }

    public bool CheckCompletionTime(int level, float duration)  //used to check if completion duration is less than best record, then save new if so, returning true only when also so calling script can do what it needs for a new record
    {
        if (hasSetPrefs)
        {
            if (level < 1 || level > numberOfLevels)
            {
                return false; //does not match expected level number (1 to assigned max levels), return out
            }

            if (duration <= 0f)
            {
                return false; //does not match expected duration, return out
            }

            if (PlayerPrefs.HasKey("BestDuration" + level))
            {
                float checkDuration = PlayerPrefs.GetFloat("BestDuration" + (level));

                if (duration < checkDuration)
                {
                    PlayerPrefs.SetFloat("BestDuration" + (level), duration);

                    levelCompleteDurations[level-1] = PlayerPrefs.GetFloat("BestDuration" + (level));

                    //do the things for a new record time
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Debug.Log("Tried to load level best durations without a key to do so when checking if duration greater");

                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void SaveTutotrialAsComplete()  //Only call when player has met conditions for learning game, whatever that is
    {
        doneTutorialCode = 1234567;

        PlayerPrefs.SetInt("DoneTutorialCode", doneTutorialCode);
    }
}
