using UnityEngine;

public class Catnip_Interact : MonoBehaviour
{
    [SerializeField]
    private GameObject catnipJar;
    [SerializeField]
    private ProgressionManager progressionManager;

    [SerializeField]
    private LevelManager levelManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (progressionManager == null)
            {
                if (GameObject.FindGameObjectWithTag("ProgressionManager").GetComponent<ProgressionManager>() != null)
                {
                    progressionManager = GameObject.FindGameObjectWithTag("ProgressionManager").GetComponent<ProgressionManager>();
                    progressionManager.AddStarForSpecialItem();
                    catnipJar.SetActive(false);
                    gameObject.GetComponent<ParticleSystem>().Play();
                }
            }

            if (levelManager == null)
            {
                if (LevelManager.instance != null)
                {
                    levelManager = LevelManager.instance;
                    levelManager.AddHiddenStar();
                }
            }
        }
    }
}
