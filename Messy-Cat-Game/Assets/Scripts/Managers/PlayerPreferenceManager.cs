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

    [Header("Level Group Strings")]
    [SerializeField] private string levelCompleteGroup1;    //string that will be broken down and built back up to save or load the number of stars a person has achieved for each level, this group is level 1 through 7
    [SerializeField] private string levelCompleteGroup2;    //string that will be broken down and built back up to save or load the number of stars a person has achieved for each level, this group is level 8 through 14
    [SerializeField] private string levelCompleteGroup3;    //string that will be broken down and built back up to save or load the number of stars a person has achieved for each level, this group is level 15 through 21
    [SerializeField] private string levelCompleteGroup4;    //string that will be broken down and built back up to save or load the number of stars a person has achieved for each level, this group is level 22 through 28
    [SerializeField] private string levelCompleteGroup5;    //string that will be broken down and built back up to save or load the number of stars a person has achieved for each level, this group is level 29 through 35

    [Header("Level Bools")]
    [SerializeField] private bool hasDoneTutorial;  // bool assigned by whatever we determine a player has done to finsh learning game

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

        setPrefsCount++;  //add a pref count after checking codes, see Update() function for use
    }

    private void LoadStars()
    {
        for (int i = 0; i < levelCompleteGroup1.Length; i++)  //iterate through characters in player pref string, if a number then assign variable in a levelStars array
        {
            if (i > 0) // first index skipped, always a letter, this is on purpose
            {
                char currentChar = levelCompleteGroup1[i];     //temporary char
                int currentValue = levelCompleteGroup1[i];     //temporary int

                if (int.TryParse(currentChar.ToString(), out currentValue))     //check if number
                {
                    levelStars[i-1] = currentValue;             //assign number value to array variable
                }
                else
                {
                    Debug.Log("Found a non numerical value when expecting integer");
                }
            }
        }

        for (int i = 0; i < levelCompleteGroup2.Length; i++)  //iterate through characters in player pref string, if a number then assign variable in a levelStars array
        {
            if (i > 0) // first index skipped, always a letter, this is on purpose
            {
                char currentChar = levelCompleteGroup2[i];
                int currentValue = levelCompleteGroup2[i];

                if (int.TryParse(currentChar.ToString(), out currentValue))     //check if number
                {
                    levelStars[i + 6] = currentValue;
                }
                else
                {
                    Debug.Log("Found a non numerical value when expecting integer");
                }
            }
        }

        for (int i = 0; i < levelCompleteGroup3.Length; i++)  //iterate through characters in player pref string, if a number then assign variable in a levelStars array
        {
            if (i > 0) // first index skipped, always a letter, this is on purpose
            {
                char currentChar = levelCompleteGroup3[i];
                int currentValue = levelCompleteGroup3[i];

                if (int.TryParse(currentChar.ToString(), out currentValue))     //check if number
                {
                    levelStars[i + 13] = currentValue;
                }
                else
                {
                    Debug.Log("Found a non numerical value when expecting integer");
                }
            }
        }

        for (int i = 0; i < levelCompleteGroup4.Length; i++)  //iterate through characters in player pref string, if a number then assign variable in a levelStars array
        {
            if (i > 0) // first index skipped, always a letter, this is on purpose
            {
                char currentChar = levelCompleteGroup4[i];
                int currentValue = levelCompleteGroup4[i];

                if (int.TryParse(currentChar.ToString(), out currentValue))     //check if number
                {
                    levelStars[i + 20] = currentValue;
                }
                else
                {
                    Debug.Log("Found a non numerical value when expecting integer");
                }
            }
        }

        for (int i = 0; i < levelCompleteGroup5.Length; i++)  //iterate through characters in player pref string, if a number then assign variable in a levelStars array
        {
            if (i > 0) // first index skipped, always a letter, this is on purpose
            {
                char currentChar = levelCompleteGroup5[i];
                int currentValue = levelCompleteGroup5[i];

                if (int.TryParse(currentChar.ToString(), out currentValue))     //check if number
                {
                    levelStars[i + 27] = currentValue;
                }
                else
                {
                    Debug.Log("Found a non numerical value when expecting integer");
                }
            }
        }

        setPrefsCount++;  //add a pref count after setting stars, see Update() function for use
    }

    public void SaveNewStar(int level, int stars)  // Call this to set new star values, passing in level and the number of stars earned
    {
        if (level < 1 || level > numberOfLevels)
        {
            return; //does not match expected level number (1 to assigned max levels), return out
        }

        if (stars < 1 || stars > 3)
        {
            return; //does not match expected number of stars (1-3), return out
        }

        //assign temporary variables

        int charIndex = 0; // set to zero then calculate, groups of 8 (0 - 7 array)
        int charGroup = 0; // set to zero then assign among the 5 groups
        string groupString = ""; //set to nothing then pass in appropriate string based on level range below

        if (level >= 1 && level <= 7)
        {
            charIndex = level;                                                   //assign temporary index value (0-7)
            charGroup = 1;                                                       //assign temporary group value
            levelCompleteGroup1 = PlayerPrefs.GetString("LevelCompleteGroup1");  //grab the existing string in prefs
            groupString = levelCompleteGroup1;                                   //assign temporary string value
        }

        if (level >= 8 && level <= 14)
        {
            charIndex = level - 8;                                               //assign temporary index value (0-7)
            charGroup = 2;                                                       //assign temporary group value
            levelCompleteGroup2 = PlayerPrefs.GetString("LevelCompleteGroup2");  //grab the existing string in prefs
            groupString = levelCompleteGroup2;                                   //assign temporary string value
        }

        if (level >= 15 && level <= 21)
        {
            charIndex = level - 15;                                              //assign temporary index value (0-7)
            charGroup = 3;                                                       //assign temporary group value
            levelCompleteGroup3 = PlayerPrefs.GetString("LevelCompleteGroup3");  //grab the existing string in prefs
            groupString = levelCompleteGroup3;                                   //assign temporary string value
        }

        if (level >= 22 && level <= 28)
        {
            charIndex = level - 22;                                              //assign temporary index value (0-7)
            charGroup = 4;                                                       //assign temporary group value
            levelCompleteGroup4 = PlayerPrefs.GetString("LevelCompleteGroup4");  //grab the existing string in prefs
            groupString = levelCompleteGroup4;                                   //assign temporary string value
        }

        if (level >= 29 && level <= 35)
        {
            charIndex = level - 29;                                              //assign temporary index value (0-7)
            charGroup = 5;                                                       //assign temporary group value
            levelCompleteGroup5 = PlayerPrefs.GetString("LevelCompleteGroup5");  //grab the existing string in prefs
            groupString = levelCompleteGroup5;                                   //assign temporary string value
        }

        if (groupString != "" || groupString != null)       //checking if groupString is empty or null
        {
            char[] charArray = groupString.ToCharArray();   //set a temporary char array to the temporary group string from above

            char newChar = (char)(stars + '0');             //assign the int star value passed into function, as a string character

            charArray[charIndex] = newChar;                 //change the temporary char array index to new star character

            string newString = new string(charArray);       //create a new string based on changed temporary char array

            AssignStarsToPrefs(charGroup, newString);      //run function to save the new star values in player prefs, passing group number and new string
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

        setPrefsCount++;  //add a pref count after setting completeion durations, see Update() function for use
    }

    public void SaveTutotrialAsComplete()  //Only call when player has met conditions for learning game, whatever that is
    {
        doneTutorialCode = 1234567;

        PlayerPrefs.SetInt("DoneTutorialCode", doneTutorialCode);
    }
}
