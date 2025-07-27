using UnityEngine;

public class Human_Controller : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private GameObject player;
    private bool checkingForCat;
    private bool triggerCheckingForCat;

    [SerializeField] private GameObject[] allDogs;
    [SerializeField] private GameObject[] allAngryDogs;
    [SerializeField] private GameObject nextAngryDog;
    [SerializeField] private int angryDogCount;

    private MakeShiftCatController catController;
    private LevelManager levelManager;
    private WindowPointer windowPointer;
    private bool setPointer;

    private Vector3 startLocation;
    private bool isRetreating;
    private bool isMoving;
    private bool isPetting;

    [SerializeField] private float minDistance;
    [SerializeField] private float humanSpeed;
    [SerializeField] private Vector3 currentTargetLocation;

    [SerializeField] private float petDelay;
    [SerializeField] private float petCounter;

    void Awake()
    {
        startLocation = transform.position;

        nextAngryDog = null;
        angryDogCount = 0;

        allDogs = GameObject.FindGameObjectsWithTag("Dog");
        allAngryDogs = new GameObject[allDogs.Length];

        CheckForAngryDogs();
    }

    void Update()
    {
        if (player == null)
        {
            if(GameObject.FindGameObjectWithTag("Player") != null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }

        if (windowPointer == null)
        {
            if (GameObject.FindGameObjectWithTag("Pointer") != null)
            {
                windowPointer = GameObject.FindGameObjectWithTag("Pointer").GetComponent<WindowPointer>();
            }
        }
        else
        {
            if (!setPointer)
            {
                setPointer = true;
                windowPointer.SetTarget(gameObject.transform);
            }
        }

        if (player != null && catController == null)
        {
            if(player.GetComponent<MakeShiftCatController>() != null)
            {
                catController = player.GetComponent<MakeShiftCatController>();
            }
        }

        if(levelManager == null)
        {
            if(GameObject.FindGameObjectWithTag("LevelManager") !=null)
            {
                levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
            }
        }

        if (catController != null && levelManager != null)
        {
            if (!triggerCheckingForCat)
            {
                triggerCheckingForCat = true;
                checkingForCat = true;
            }

            if (checkingForCat)
            {
                CheckForCat();
            }
            else
            {
                Debug.Log("Retreating to human space so stopped checking for cat and move away");
            }
        }

        if (isMoving)
        {
            MoveToNextLocation(currentTargetLocation);
        }

        if (isPetting)
        {
            if (petCounter < petDelay)
            {
                petCounter += Time.deltaTime;
            }
            else
            {
                isPetting = false;
                petCounter = 0f;

                if (nextAngryDog.GetComponent<DogContext>() != null)
                {
                    nextAngryDog.GetComponent<DogContext>().CalmDog();
                }

                CheckForAngryDogs();
            }
        }
    }

    private void CheckForCat()
    {
        if(catController != null)
        {
            if (!catController.isHidden)
            {
                if (!catController.triggerHide)
                {
                    //Human Sees cat, fail level

                    HumanSeesCat();
                }
                else
                {
                    Debug.Log("CLOSE CALL cat almost seen!");
                }
            }
        }
    }

    private void HumanSeesCat()
    {
        isMoving = false;

        if(anim != null)
        {
            anim.SetBool("seenCat",true);
        }

        if (levelManager != null)
        {
            levelManager.HumanSeesCat();
        }
    }

    private void CheckForAngryDogs()
    {
        angryDogCount = 0;

        for (int i = 0; i < allDogs.Length; i++)
        {
            if (allDogs[i] != null)
            {
                //make shift code for checking if dog is angry and which ones

                if (allDogs[i].GetComponent<DogContext>() != null)
                {
                    if (allDogs[i].GetComponent<DogContext>().isAngry)
                    {
                        //Debug.Log("Setting an angry dog");
                        allAngryDogs[i] = allDogs[i];

                        //Debug.Log("increasing angry dog count");
                        angryDogCount++;

                        //Debug.Log("Angry dog count should be " + angryDogCount);
                    }
                    else
                    {
                        allAngryDogs[i] = null;
                    }                
                }
                else
                {
                    Debug.Log("Was not able to reference dog context");
                }
            }
        }

        if (angryDogCount > 0)
        {
            TargetNextAngryDog();
        }
        else
        {
            //no more dogs are angry so Retreat

            RetreatToHumanSpace();
        }
    }

    private void TargetNextAngryDog()
    {
        for (int i = 0; i < allAngryDogs.Length; i++)
        {
            if (allAngryDogs[i] != null)
            {
                nextAngryDog = allAngryDogs[i];

                TravelToNextAngryDog();
                return;
            }
        }

        nextAngryDog = null;

        //no angry dogs found

        RetreatToHumanSpace();
    }

    private void TravelToNextAngryDog()
    {
        if (nextAngryDog == null)
        {
            CheckForAngryDogs();
        }
        else
        {
            currentTargetLocation = nextAngryDog.transform.position;
            MoveToNextLocation(currentTargetLocation);

            isMoving = true;

            if (anim != null)
            {
                anim.SetBool("seenDog", true);
            }
        }
    }

    private void RetreatToHumanSpace()
    {
        isMoving = true;
        checkingForCat = false;
        isRetreating = true;

        if (anim != null)
        {
            anim.SetBool("dogReached", false);
        }

        currentTargetLocation = startLocation;
        MoveToNextLocation(startLocation);

        if (windowPointer != null)
        {
            windowPointer.target = gameObject.transform;
            windowPointer.SetOff();
        }
    }

    private void MoveToNextLocation(Vector3 targetLocation)
    {
        //move human to target location

        if (Vector3.Distance(transform.position, targetLocation) < minDistance)
        {
            //transform.position = targetLocation; // Snap to the exact target if needed

            isMoving = false;

            if (isRetreating)
            {
                KillHuman();
            }
            else
            {
                if (anim != null)
                {
                    anim.SetBool("dogReached", true);
                }

                isPetting = true;
            }
        }
        else
        {
            transform.LookAt(targetLocation);

            transform.position = Vector3.MoveTowards(transform.position, targetLocation, humanSpeed * Time.deltaTime);
        }
    }
    

    public void KillHuman()
    {
        if(gameObject != null)
        {
            if(levelManager != null)
            {
                levelManager.ResetHuman();
            }

            Debug.Log("Killed Human");

            Destroy(gameObject);
        }
    }
}
