using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject target;
    [SerializeField] private float speed;
    [SerializeField] private float distancePlayer;
    [SerializeField] private float minDistance;

    [SerializeField] private bool following;
    [SerializeField] private float moveCounter;
    [SerializeField] private float speedProRate;

    [SerializeField] private Transform focusCamera;


    //new

    public bool targetPlayer;
    public bool spawnOffset;

    [SerializeField] private GameObject player;

    [SerializeField] private float distance;
    [SerializeField] private float threshold;

    void Update()
    {
        if (target != null)
        {
            Follow();
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }

            if (player != null)
            {
                target = player;
            }
        }
    }

    private void Follow()
    {
        if (target != null)
        {
            if (!following)
            {
                distancePlayer = Vector3.Distance(transform.position, target.transform.position);

                moveCounter = 0f;

                speedProRate = (Vector3.Distance(transform.position, target.transform.position) / 3.14f) * speed;

                if (distancePlayer > minDistance)
                {
                    following = true;
                }
            }
            else
            {
                if(speedProRate == 0f)
                {
                    speedProRate = 1f;
                }

                moveCounter += Time.deltaTime;

                float pCom = moveCounter / speedProRate;

                transform.position = Vector3.Lerp(transform.position, target.transform.position, pCom);

                if (Vector3.Distance(transform.position, target.transform.position) <= minDistance)
                {
                    following = false;
                }              
            }
        }
    }
}
