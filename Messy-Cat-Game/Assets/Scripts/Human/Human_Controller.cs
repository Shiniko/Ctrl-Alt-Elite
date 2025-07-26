using UnityEngine;

public class Human_Controller : MonoBehaviour
{
    private Animator anim;

    private GameObject player;
    private bool checkingForCat;
    private bool triggerCheckingForCat;

    private GameObject[] allDogs;
    private GameObject[] allAngryDogs;
    private GameObject nextAngryDog;
    private int angryDogCount;

    private MakeShiftCatController catController;
    private LevelManager levelManager;

    private Vector3 startLocation;
    private bool isRetreating;
    private bool isMoving;

    [SerializeField] private float minDistance;
    [SerializeField] private float humanSpeed;
    [SerializeField] private Vector3 currentTargetLocation;

    void Awake()
    {
        startLocation = gameObject.transform.position;

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

        if(player != null && catController == null)
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
    }

    private void CheckForCat()
    {
        if(catController != null)
        {
            if (!catController.isHidden)
            {
                //Human Sees cat, fail level

                HumanSeesCat();
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
        for (int i = 0; i < allDogs.Length; i++)
        {
            if (allDogs[i] != null)
            {
                //make shift code for checking if dog is angry and which ones

                int tempAngryDogs = 0;
                if (allDogs[i].GetComponent<DogContext>() != null)
                {
                    if (allDogs[i].GetComponent<DogContext>().isAngry)
                    {
                        allAngryDogs[i] = allDogs[i];
                        tempAngryDogs++;
                    }
                    else
                    {
                        Debug.Log("bool for barking came back false for " + allDogs[i]);
                    }

                    angryDogCount = tempAngryDogs;
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
        checkingForCat = false;
        isRetreating = true;

        currentTargetLocation = startLocation;
        MoveToNextLocation(startLocation);
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

                if (nextAngryDog.GetComponent<DogContext>() != null)
                {
                    nextAngryDog.GetComponent<DogContext>().CalmDog();
                }
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
                levelManager.triggerHuman = false;
            }

            Destroy(gameObject);
        }
    }
}
