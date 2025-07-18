using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Accesible Params")]
    public bool levelActive;
    public bool catHidden;
    public bool dogSeenCat;
    public bool humanSeenCat;

    [Header("References")]
    public LevelDetails levelDetails;
    public Transform spawnPoint;

    [Header("Triggers and Checks")]
    [SerializeField] private bool triggerFail;
    [SerializeField] private bool triggerVictory;
    [SerializeField] private bool triggerAvoidStarLoss;
    [SerializeField] private bool triggerMessGain;
    [SerializeField] private bool triggerMessStar;
    [SerializeField] private bool triggerHiddenStar;

    [SerializeField] private bool hiddenFound;
    [SerializeField] private bool messMade;
    [SerializeField] private bool exitRevealed;

    [Header("Level Duration Params")]
    public float levelDuration;
    [SerializeField] private bool countDuration;
    [SerializeField] private int hours;
    [SerializeField] private int minutes;
    [SerializeField] private int seconds;
    [SerializeField] private int miliseconds;

    void Update()
    {
        if (levelActive)
        {

        }
    }

}
