using UnityEngine;

public class MainMenuCat : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private int idleNum;
    [SerializeField] private int idleNumMax;

    [SerializeField] private float currentIdleDuration;
    [SerializeField] private float currentIdleCounter;
    [SerializeField] private float[] idleDurations;

    [SerializeField] private bool canSwitch;
    [SerializeField] private bool triggerSwitch;

    [SerializeField] private string newState;

    void Start()
    {
        idleNum = 1;
    }

    public void Switchready()
    {
        canSwitch = true;
    }

    void Update()
    {
        if(idleNum > idleNumMax)
        {
            idleNum = 1;
        }

        if (currentIdleCounter < currentIdleDuration)
        {
            currentIdleCounter += Time.deltaTime;
        }
        else
        {
            currentIdleCounter = currentIdleDuration;

            if (canSwitch)
            {
                if (!triggerSwitch)
                {
                    triggerSwitch = true;
                    canSwitch = false;

                    SwitchIdles();
                }
            }
        }
    }

    private void SwitchIdles()
    {
        currentIdleCounter = 0f;
        triggerSwitch = false;

        int tempNum = Random.Range(1, idleNumMax + 1);

        if (tempNum > idleNumMax)
        {
            tempNum = 1;
        }

        idleNum = tempNum;

        newState = "Idle-" + idleNum;

        if (anim != null)
        {
            //anim.Play(newState);

            anim.SetInteger("IdleNum",idleNum);
        }

        if(idleNum < idleDurations.Length)
        {
            currentIdleDuration = idleDurations[idleNum-1];
        }
    }

}
